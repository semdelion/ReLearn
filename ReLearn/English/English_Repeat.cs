using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Android.Speech.Tts;
using Java.Util;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace ReLearn
{
    [Activity(Label = "Repeat 1/20", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class English_Repeat : Activity, TextToSpeech.IOnInitListener
    {
        private TextToSpeech tts;

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            if (status == OperationResult.Success)
                tts.SetLanguage(Locale.Uk);
            
        }

        private void SpeakOut(string text) => tts.Speak(text, QueueMode.Flush, null, null);

        void Random_Button(Button B1, Button B2, Button B3, Button B4, List<Database_Words> dataBase, int i)   //загружаем варианты ответа в текст кнопок
        {
            Additional_functions.Random_4_numbers(i, dataBase.Count, out List<int> random_numbers);
            B1.Text = dataBase[random_numbers[0]].TranslationWord;
            B2.Text = dataBase[random_numbers[1]].TranslationWord;
            B3.Text = dataBase[random_numbers[2]].TranslationWord;
            B4.Text = dataBase[random_numbers[3]].TranslationWord;
        }

        void Answer(Button B1, Button B2, Button B3, Button B4,Button BNext, List<Database_Words> dataBase, List<Statistics_learn> Stats,int rand_word ) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            GUI.Button_enable(B1, B2, B3, B4, BNext);
            if (B1.Text == dataBase[rand_word].TranslationWord){
                Repeat_work.Delete_Repeat(Stats, dataBase[rand_word].Word, rand_word, dataBase[rand_word].NumberLearn -= Magic_constants.true_answer);
                Statistics_learn.AnswerTrue++;
                GUI.Button_true(B1);
            }
            else{
                Repeat_work.Delete_Repeat(Stats, dataBase[rand_word].Word, rand_word, dataBase[rand_word].NumberLearn += Magic_constants.false_answer);
                Statistics_learn.AnswerFalse++;
                GUI.Button_false(B1);
                if (B2.Text == dataBase[rand_word].TranslationWord)
                    GUI.Button_true(B2);
                else if (B3.Text == dataBase[rand_word].TranslationWord)
                    GUI.Button_true(B3);
                else
                    GUI.Button_true(B4);
            }
        }

        void Function_Next_Test(Button B1, Button B2, Button B3, Button B4, Button BNext, TextView textView, List<Database_Words> dataBase,int rand_word,int i_rand) //новый тест
        {
            textView.Text = dataBase[rand_word].Word.ToString();
            GUI.Button_ebabled(BNext);
            switch (i_rand){// задаём рандоммную кнопку                            
                case 0:{
                        Random_Button(B1, B2, B3, B4, dataBase, rand_word);
                        break;
                    }
                case 1:{
                        Random_Button(B2, B1, B3, B4, dataBase, rand_word);
                        break;
                    }
                case 2:{
                        Random_Button(B3, B1, B2, B4, dataBase, rand_word);
                        break;
                    }
                case 3:{
                        Random_Button(B4, B1, B2, B3, dataBase, rand_word);
                        break;
                }
            }
        }

        public void Update_Database(List<Statistics_learn> listdataBase) // изменение у бвзы данных элемента NumberLearn
        {
            var database = DataBase.Connect(Database_Name.English_DB);
            int month = DateTime.Today.Month;
            database.CreateTable<Database_Words>();
            for (int i = 0; i < listdataBase.Count; i++)
            {
                database.Query<Database_Words>("UPDATE " + DataBase.Table_Name + " SET DateRecurrence = " + month + " WHERE Word = ?", listdataBase[i].Word);
                database.Query<Database_Words>("UPDATE " + DataBase.Table_Name + " SET NumberLearn = " + listdataBase[i].Learn + " WHERE Word = ?", listdataBase[i].Word);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            base.OnCreate(savedInstanceState);
            GUI.Button_default(English.button_english_repeat);
            SetContentView(Resource.Layout.English_Repeat);
            tts = new TextToSpeech(this, this);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglishRepeat);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            GUI.Button_default(English.button_english_repeat);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Statistics_learn.AnswerFalse = 0;
            Statistics_learn.AnswerTrue = 0;
            int rand_word = 0, i_rand = 0,count=0;

            TextView textView = FindViewById<TextView>(Resource.Id.textView_Eng_Word);
            List<Statistics_learn> Stats = new List<Statistics_learn>();
            
            Button button1 = FindViewById<Button>(Resource.Id.button_E_choice1);
            Button button2 = FindViewById<Button>(Resource.Id.button_E_choice2);
            Button button3 = FindViewById<Button>(Resource.Id.button_E_choice3);
            Button button4 = FindViewById<Button>(Resource.Id.button_E_choice4);
            Button button_next = FindViewById<Button>(Resource.Id.button_E_Next);
            Button button_Speak = FindViewById<Button>(Resource.Id.Button_Speak);

            button1.Touch += GUI.Button_Click;
            button2.Touch += GUI.Button_Click;
            button3.Touch += GUI.Button_Click;
            button4.Touch += GUI.Button_Click;
            button_next.Touch += GUI.Button_Click;
            button_Speak.Touch+=GUI.Button_Click;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            try
            {
                var db = DataBase.Connect(Database_Name.English_DB);
                var dataBase = db.Query<Database_Words>("SELECT * FROM " + DataBase.Table_Name + " WHERE NumberLearn > 0");

                System.Random rnd = new System.Random(unchecked((int)(DateTime.Now.Ticks)));
                rand_word = rnd.Next(dataBase.Count); //ПЕРЕДЕЛАЙ сделать защиту от дурака, если все элементы базы имеют NumberLearn = 0
                i_rand = rnd.Next(4);                           //рандом для 4 кнопок
                Function_Next_Test(button1, button2, button3, button4, button_next, textView, dataBase, rand_word, i_rand);

                button1.Click += (s, e) => { Answer(button1, button2, button3, button4, button_next, dataBase, Stats, rand_word); }; //лямбда оператор для подсветки ответа // true ? green:red
                button2.Click += (s, e) => { Answer(button2, button1, button3, button4, button_next, dataBase, Stats, rand_word); };
                button3.Click += (s, e) => { Answer(button3, button1, button2, button4, button_next, dataBase, Stats, rand_word); };
                button4.Click += (s, e) => { Answer(button4, button1, button2, button3, button_next, dataBase, Stats, rand_word); };

                button_Speak.Click += delegate
                {
                    SpeakOut(textView.Text);
                };

                button_next.Click += (s, e) =>
                {
                    button_next.Enabled = false;
                    if (count < Magic_constants.repeat_count - 1)
                    {
                        count++;
                        i_rand = rnd.Next(4);
                        rand_word = rnd.Next(dataBase.Count);
                        Function_Next_Test(button1, button2, button3, button4, button_next, textView, dataBase, rand_word, i_rand);
                        GUI.Button_Refresh(button1, button2, button3, button4, button_next);
                        ActionBar.Title = Convert.ToString("Repeat " + (count + 1) + "/" + Magic_constants.repeat_count); // ПЕРЕДЕЛАЙ Костыль счётчик 
                    }
                    else
                    {
                        Repeat_work.Add_Statistics(Statistics_learn.AnswerTrue, Statistics_learn.AnswerFalse);
                        Update_Database(Stats);
                        Intent intent_english_stat = new Intent(this, typeof(English_Stat));
                        StartActivity(intent_english_stat);
                        this.Finish();
                    }
                };
            }
            catch{ Toast.MakeText(this, "Error : can't connect to database of Language in Repeat", ToastLength.Long).Show(); }

        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
}