using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Calligraphy;
using Android.Views;
using Android.Widget;
using SQLite;
using Android.Support.V7.App;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Languages_Repeat : AppCompatActivity
    {
        int Count = -1;
        int CurrentWordNumber { get; set; }

        List<Button> Buttons { get; set; }
        ButtonNext Button_next { get; set; }
        List<DBWords> DBWords { get; set; }
        
        MyTextToSpeech MySpeech { get; set; }
        readonly List<Statistics> Stats = new List<Statistics>();

        string TitleCount
        {
            set { FindViewById<TextView>(Resource.Id.Repeat_toolbar_textview).Text = value; }
        }

        string Word
        {
            get { return FindViewById<TextView>(Resource.Id.textView_Eng_Word).Text; }
            set { FindViewById<TextView>(Resource.Id.textView_Eng_Word).Text = value; }
        }

        void Button_enable(bool state)
        {
            foreach(var button in Buttons) button.Enabled = state;
            if (state)
            {
                Button_next.State = StateButton.Unknown;
                Button_next.button.Text = GetString(Resource.String.Unknown);
                foreach (var button in Buttons) button.Background = GetDrawable(Resource.Drawable.button_style_standard);
            }
            else
            {
                Button_next.State = StateButton.Next;
                Button_next.button.Text = GetString(Resource.String.Next);
            }
        }

        void Random_Button(params Button[] buttons)   //загружаем варианты ответа в текст кнопок
        {
            AdditionalFunctions.RandomFourNumbers(CurrentWordNumber, DBWords.Count, out List<int> random_numbers);
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].Text = DBWords[random_numbers[i]].TranslationWord;
        }

        void NextWord() 
        {
            Word = DBWords[CurrentWordNumber].Word;
            const int four = 4;
            int first = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(four);
            List<int> random_numbers = new List<int> { first, 0, 0, 0 };
            for (int i = 1; i < four; i++)
                random_numbers[i] = (first + i) % four;
           Random_Button(Buttons[random_numbers[0]], Buttons[random_numbers[1]], Buttons[random_numbers[2]], Buttons[random_numbers[3]]);        
        }

        void Answer(params Button[] buttons) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            Button_enable(false);
            if (buttons[0].Text == DBWords[CurrentWordNumber].TranslationWord)
            {
                Statistics.Add(Stats, DBWords[CurrentWordNumber].Word, CurrentWordNumber, DBWords[CurrentWordNumber].NumberLearn, -Settings.TrueAnswer);
                Statistics.AnswerTrue++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_true);
            }
            else
            {
                Statistics.Add(Stats, DBWords[CurrentWordNumber].Word, CurrentWordNumber, DBWords[CurrentWordNumber].NumberLearn, Settings.FalseAnswer);
                Statistics.AnswerFalse++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_false);
                for (int i = 1; i < buttons.Length; i++)
                    if (buttons[i].Text == DBWords[CurrentWordNumber].TranslationWord)
                    {
                        buttons[i].Background = GetDrawable(Resource.Drawable.button_true);
                        return;
                    }
            }
        }

        void Unknown()
        {
            Statistics.AnswerFalse++;
            Statistics.Add(Stats, DBWords[CurrentWordNumber].Word, CurrentWordNumber, DBWords[CurrentWordNumber].NumberLearn, Settings.NeutralAnswer);
            for (int i = 0; i < Buttons.Count; i++)
                if (Buttons[i].Text == DBWords[CurrentWordNumber].TranslationWord)
                {
                    Buttons[i].Background = GetDrawable(Resource.Drawable.button_true);
                    return;
                }
        }

        void Update_Database() // изменение у BD элемента NumberLearn
        {
            var database = DataBase.Connect(Database_Name.English_DB);
            for (int i = 0; i < Stats.Count; i++)
            {
                var query = $"UPDATE {DataBase.TableNameLanguage} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?";
                database.Execute(query, DateTime.Now, Stats[i].Learn, Stats[i].Word);
            }
        }

        [Java.Interop.Export("Button_Speak_Languages_Click")]
        public void Button_Speak_Languages_Click(View v) => MySpeech.Speak(Word, this);

        [Java.Interop.Export("Button_Languages_1_Click")]
        public void Button_Languages_1_Click(View v) => Answer(Buttons[0], Buttons[1], Buttons[2], Buttons[3]);

        [Java.Interop.Export("Button_Languages_2_Click")] 
        public void Button_Languages_2_Click(View v) => Answer(Buttons[1], Buttons[0], Buttons[2], Buttons[3]);

        [Java.Interop.Export("Button_Languages_3_Click")] 
        public void Button_Languages_3_Click(View v) => Answer(Buttons[2], Buttons[0], Buttons[1], Buttons[3]);

        [Java.Interop.Export("Button_Languages_4_Click")] 
        public void Button_Languages_4_Click(View v) => Answer(Buttons[3], Buttons[0], Buttons[1], Buttons[2]);

        [Java.Interop.Export("Button_Languages_Next_Click")]
        public void Button_Languages_Next_Click(View v)
        {
            Button_next.button.Enabled = false;
            if (Button_next.State == StateButton.Unknown)
            {
                Button_next.State = StateButton.Next;
                Button_enable(false);
                Unknown();
            }
            else
            {
                if (Count < Settings.NumberOfRepeatsLanguage - 1)
                {
                    Count++;
                    Random rnd = new System.Random(unchecked((int)(DateTime.Now.Ticks)));
                    CurrentWordNumber = rnd.Next(DBWords.Count);
                    NextWord();
                    Button_enable(true);
                    TitleCount = Convert.ToString(GetString(Resource.String.Repeat) + " " + (Count + 1) + "/" + Settings.NumberOfRepeatsLanguage);
                }
                else
                {
                    DBStatistics.Insert(Statistics.AnswerTrue, Statistics.AnswerFalse, DataBase.TableNameLanguage.ToString());
                    Update_Database();
                    Intent intent_english_stat = new Intent(this, typeof(Languages_Stat));
                    StartActivity(intent_english_stat);
                    this.Finish();
                }
            }
            Button_next.button.Enabled = true;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Languages_Repeat);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguagesRepeat);
           
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            Statistics.Statistics_update();
            MySpeech = new MyTextToSpeech();
            Buttons = new List<Button>{
                FindViewById<Button>(Resource.Id.button_Languages_choice1),
                FindViewById<Button>(Resource.Id.button_Languages_choice2),
                FindViewById<Button>(Resource.Id.button_Languages_choice3),
                FindViewById<Button>(Resource.Id.button_Languages_choice4),
            };
            
            Button_next = new ButtonNext
            {
                button = FindViewById<Button>(Resource.Id.button_Languages_Next),
                State = StateButton.Next
            };

            try
            {
                SQLiteConnection db = DataBase.Connect(Database_Name.English_DB);
                DBWords = db.Query<DBWords>($"SELECT * FROM {DataBase.TableNameLanguage} WHERE NumberLearn > 0");
                Button_Languages_Next_Click(null);
            }
            catch
            {
                Toast.MakeText(this, GetString(Resource.String.DatabaseNotConnect), ToastLength.Short).Show();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}