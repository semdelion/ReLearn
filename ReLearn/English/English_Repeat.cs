using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using Plugin.TextToSpeech;

namespace ReLearn
{
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class English_Repeat : Activity
    {
        TextView textView;
        Button Button1;
        Button Button2;
        Button Button3;
        Button Button4;
        Button Button_next;
        List<Database_Words> dataBase;
        readonly List<Statistics> Stats = new List<Statistics>();
        int Count = -1;
        int Rand_word { get; set; }

        void Button_enable() 
        {
            Button1.Enabled = false;
            Button2.Enabled = false;
            Button3.Enabled = false;
            Button4.Enabled = false;
            Button_next.Enabled = true;
        }

        void Button_Refresh() 
        {
            Button1.Enabled = true; Button2.Enabled = true; Button3.Enabled = true; Button4.Enabled = true;
            Button1.Background = GetDrawable(Resource.Drawable.button_style_standard);
            Button2.Background = GetDrawable(Resource.Drawable.button_style_standard);
            Button3.Background = GetDrawable(Resource.Drawable.button_style_standard);
            Button4.Background = GetDrawable(Resource.Drawable.button_style_standard);
        }

        void Random_Button(Button B1, Button B2, Button B3, Button B4, int i)   //загружаем варианты ответа в текст кнопок
        {
            Additional_functions.Random_4_numbers(i, dataBase.Count, out List<int> random_numbers);
            B1.Text = dataBase[random_numbers[0]].TranslationWord;
            B2.Text = dataBase[random_numbers[1]].TranslationWord;
            B3.Text = dataBase[random_numbers[2]].TranslationWord;
            B4.Text = dataBase[random_numbers[3]].TranslationWord;
        }

        void Answer(Button B1, Button B2, Button B3, Button B4,Button BNext,int rand_word ) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            Button_enable();
            if (B1.Text == dataBase[rand_word].TranslationWord){
                Additional_functions.Update_number_learn(Stats, dataBase[rand_word].Word, rand_word, dataBase[rand_word].NumberLearn -= Magic_constants.true_answer);
                Statistics.AnswerTrue++;
                B1.Background = GetDrawable(Resource.Drawable.button_true);
            }
            else{
                Additional_functions.Update_number_learn(Stats, dataBase[rand_word].Word, rand_word, dataBase[rand_word].NumberLearn += Magic_constants.false_answer);
                Statistics.AnswerFalse++;
                B1.Background = GetDrawable(Resource.Drawable.button_false);
                if (B2.Text == dataBase[rand_word].TranslationWord)
                    B2.Background = GetDrawable(Resource.Drawable.button_true);
                else if (B3.Text == dataBase[rand_word].TranslationWord)
                    B3.Background = GetDrawable(Resource.Drawable.button_true);
                else
                    B4.Background = GetDrawable(Resource.Drawable.button_true);
            }
        }

        void Function_Next_Test(int rand_word,int i_rand) //новый тест
        {
            textView.Text = dataBase[rand_word].Word.ToString();
            switch (i_rand){// задаём рандоммную кнопку                            
                case 0:{
                    Random_Button(Button1, Button2, Button3, Button4, rand_word);
                    break;
                }
                case 1:{
                    Random_Button(Button2, Button1, Button3, Button4, rand_word);
                    break;
                }
                case 2:{
                    Random_Button(Button3, Button1, Button2, Button4, rand_word);
                    break;
                }
                case 3:{
                    Random_Button(Button4, Button1, Button2, Button3, rand_word);
                    break;
                }
            }
        }

        public void Update_Database() // изменение у бвзы данных элемента NumberLearn
        {
            var database = DataBase.Connect(Database_Name.English_DB);          
            database.CreateTable<Database_Words>();
            for (int i = 0; i < Stats.Count; i++)
            {
                database.Query<Database_Words>("UPDATE " + DataBase.Table_Name + " SET DateRecurrence = DATETIME('NOW') WHERE Word = ?", Stats[i].Word);
                database.Query<Database_Words>("UPDATE " + DataBase.Table_Name + " SET NumberLearn = " + Stats[i].Learn + " WHERE Word = ?", Stats[i].Word);
            }
        }

        [Java.Interop.Export("Button_Speak_Eng_Click")]
        public async void Button_Speak_Eng_Click(View v) => await CrossTextToSpeech.Current.Speak(textView.Text);

        [Java.Interop.Export("Button_English_1_Click")]
        public void Button_English_1_Click(View v) =>  Answer(Button1, Button2, Button3, Button4, Button_next, Rand_word);
    

        [Java.Interop.Export("Button_English_2_Click")]
        public void Button_English_2_Click(View v) => Answer(Button2, Button1, Button3, Button4, Button_next, Rand_word);


        [Java.Interop.Export("Button_English_3_Click")]
        public void Button_English_3_Click(View v) => Answer(Button3, Button1, Button2, Button4, Button_next, Rand_word);


        [Java.Interop.Export("Button_English_4_Click")]
        public void Button_English_4_Click(View v) => Answer(Button4, Button1, Button2, Button3, Button_next, Rand_word);


        [Java.Interop.Export("Button_English_Next_Click")]
        public void Button_English_Next_Click(View v)
        {
            Button_next.Enabled = false;
            if (Count < Magic_constants.repeat_count - 1)
            {
                Count++;
                System.Random rnd = new System.Random(unchecked((int)(DateTime.Now.Ticks)));
                Rand_word = rnd.Next(dataBase.Count);
                Function_Next_Test( Rand_word, rnd.Next(4));
                Button_Refresh();
                ActionBar.Title = Convert.ToString("Repeat " + (Count + 1) + "/" + Magic_constants.repeat_count); // ПЕРЕДЕЛАЙ Костыль счётчик 
            }
            else
            {
                Statistics.Add_Statistics(Statistics.AnswerTrue, Statistics.AnswerFalse);
                Update_Database();
                Intent intent_english_stat = new Intent(this, typeof(English_Stat));
                StartActivity(intent_english_stat);
                this.Finish();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Repeat);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglishRepeat);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            Statistics.Statistics_update();
            textView = FindViewById<TextView>(Resource.Id.textView_Eng_Word);
            Button1 = FindViewById<Button>(Resource.Id.button_E_choice1);
            Button2 = FindViewById<Button>(Resource.Id.button_E_choice2);
            Button3 = FindViewById<Button>(Resource.Id.button_E_choice3);
            Button4 = FindViewById<Button>(Resource.Id.button_E_choice4);
            Button_next = FindViewById<Button>(Resource.Id.button_E_Next);
            try
            {
                SQLiteConnection db = DataBase.Connect(Database_Name.English_DB);
                dataBase = db.Query<Database_Words>("SELECT * FROM " + DataBase.Table_Name + " WHERE NumberLearn > 0");
                Button_English_Next_Click(null);
            }
            catch { Toast.MakeText(this, "Error : can't connect to database of Language in Repeat", ToastLength.Long).Show(); }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
}