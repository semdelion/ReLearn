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
            {
                tts.SetLanguage(Locale.Uk);
                tts.SetPitch(3.6f);
                tts.SetSpeechRate(1.0f);
                SpeakOut("");
            }
        }
        private void SpeakOut(string text) => tts.Speak(text, QueueMode.Flush, null, null);

        void Random_Button(Button B1, Button B2, Button B3, Button B4, List<DatabaseOfWords> dataBase, int i)   //загружаем варианты ответа в текст кнопок
        {
            System.Random rand = new System.Random(unchecked((int)(DateTime.Now.Ticks)));
            B1.Text = dataBase[i].ruWords;
            B2.Text = dataBase[rand.Next(dataBase.Count)].ruWords;
            B3.Text = dataBase[rand.Next(dataBase.Count)].ruWords;
            B4.Text = dataBase[rand.Next(dataBase.Count)].ruWords;
        }
        void Answer(Button B1, Button B2, Button B3, Button B4,Button BNext, List<DatabaseOfWords> dataBase, List<Statistics_learn> Stats,int rand_word ) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            GUI.Button_enable(B1, B2, B3, B4, BNext);
            if (B1.Text == dataBase[rand_word].ruWords){
                Repeat_work.DeleteRepeat(Stats, dataBase[rand_word].enWords, rand_word, dataBase[rand_word].numberLearn -= magic_constants.true_answer);
                Statistics_learn.answerTrue++;
                GUI.Button_true(B1);
            }
            else{
                Repeat_work.DeleteRepeat(Stats, dataBase[rand_word].enWords, rand_word, dataBase[rand_word].numberLearn += magic_constants.false_answer);
                Statistics_learn.answerFalse++;
                GUI.Button_false(B1);
                if (B2.Text == dataBase[rand_word].ruWords)
                    GUI.Button_true(B2);
                else if (B3.Text == dataBase[rand_word].ruWords)
                    GUI.Button_true(B3);
                else
                    GUI.Button_true(B4);
            }
        }

        void Function_Next_Test(Button B1, Button B2, Button B3, Button B4, Button BNext, TextView textView, List<DatabaseOfWords> dataBase,int rand_word,int i_rand) //новый тест
        {
            textView.Text = dataBase[rand_word].enWords.ToString();
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
        public void Update_Database(List<Statistics_learn> listdataBase) // изменение у бвзы данных элемента numberLearn
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), NameDatabase.English_DB);
            var database = new SQLiteConnection(databasePath); 
            database.CreateTable<Database>();
            int month = DateTime.Today.Month;
            for (int i = 0; i < listdataBase.Count; i++)
            {               
                database.Query<Database>("UPDATE Database SET dateRepeat = " + month + " WHERE enWords = ?", listdataBase[i].Word);
                database.Query<Database>("UPDATE Database SET numberLearn = " + listdataBase[i].Learn + " WHERE enWords = ?", listdataBase[i].Word);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            base.OnCreate(savedInstanceState);
            GUI.Button_default(English.button_english_repeat);
            SetContentView(Resource.Layout.English_Repeat);
            tts = new TextToSpeech(this, this);

            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglishRepeat);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundEnFl));
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(127, 0, 0, 0));

            GUI.Button_default(English.button_english_repeat);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Statistics_learn.answerFalse = 0;
            Statistics_learn.answerTrue = 0;
            int rand_word = 0, i_rand = 0,count=0;

            TextView textView = FindViewById<TextView>(Resource.Id.textView_Eng_Word);
            List<Statistics_learn> Stats = new List<Statistics_learn>();
            
            Button button1 = FindViewById<Button>(Resource.Id.button_E_choice1);
            Button button2 = FindViewById<Button>(Resource.Id.button_E_choice2);
            Button button3 = FindViewById<Button>(Resource.Id.button_E_choice3);
            Button button4 = FindViewById<Button>(Resource.Id.button_E_choice4);
            Button button_next = FindViewById<Button>(Resource.Id.button_E_Next);
            Button btnSpeak = FindViewById<Button>(Resource.Id.Button_Speak);
            button1.Touch += GUI.Button_Click;
            button2.Touch += GUI.Button_Click;
            button3.Touch += GUI.Button_Click;
            button4.Touch += GUI.Button_Click;
            button_next.Touch += GUI.Button_Click;
            btnSpeak.Touch+=GUI.Button_Click;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            try
            {
                var db = DataBase.Connect(NameDatabase.English_DB);
                db.CreateTable<Database>();
                List<DatabaseOfWords> dataBase = new List<DatabaseOfWords>();
                var table = db.Table<Database>();
                foreach (var word in table){ // создание БД в виде  List<DatabaseOfWords>
                    DatabaseOfWords w = new DatabaseOfWords();
                    if (word.numberLearn != 0)
                    {
                        w.Add(word.enWords, word.ruWords, word.numberLearn, word.dateRepeat);
                        dataBase.Add(w);
                    }    
                }

                System.Random rnd = new System.Random(unchecked((int)(DateTime.Now.Ticks)));
                rand_word = rnd.Next(dataBase.Count); //ПЕРЕДЕЛАЙ сделать защиту от дурака, если все элементы базы имеют numberLearn = 0
                i_rand = rnd.Next(4);                           //рандом для 4 кнопок
                Function_Next_Test(button1, button2, button3, button4, button_next, textView, dataBase, rand_word, i_rand);
                button1.Click += (s, e) => { Answer(button1, button2, button3, button4, button_next, dataBase, Stats, rand_word); }; //лямбда оператор для подсветки ответа // true ? green:red
                button2.Click += (s, e) => { Answer(button2, button1, button3, button4, button_next, dataBase, Stats, rand_word); };
                button3.Click += (s, e) => { Answer(button3, button1, button2, button4, button_next, dataBase, Stats, rand_word); };
                button4.Click += (s, e) => { Answer(button4, button1, button2, button3, button_next, dataBase, Stats, rand_word); };

                btnSpeak.Click += delegate
                {
                    SpeakOut(textView.Text);
                };
                button_next.Click += (s, e) =>
                {
                    if (count < magic_constants.repeat_count - 1)
                    {
                        i_rand = rnd.Next(4);
                        rand_word = rnd.Next(dataBase.Count);
                        Function_Next_Test(button1, button2, button3, button4, button_next, textView, dataBase, rand_word, i_rand);
                        GUI.Button_Refresh(button1, button2, button3, button4, button_next);
                        count++;
                        ActionBar.Title = Convert.ToString("Repeat " + (count + 1) + "/" + magic_constants.repeat_count); // ПЕРЕДЕЛАЙ Костыль счётчик 
                    }
                    else
                    {
                        Repeat_work.AddStatistics(Statistics_learn.answerTrue, Statistics_learn.answerFalse, NameDatabase.English_Stat_DB/*English_DB*/);
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