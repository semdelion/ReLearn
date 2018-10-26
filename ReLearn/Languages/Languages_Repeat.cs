﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Calligraphy;
using Android.Views;
using Android.Widget;
using SQLite;
using Plugin.TextToSpeech;
using Android.Support.V7.App;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Languages_Repeat : AppCompatActivity
    {
        TextView textView;
        Button Button1;
        Button Button2;
        Button Button3;
        Button Button4;
        ButtonNext Button_next;
        List<Database_Words> dataBase;
        TextView Title_textView;
        readonly List<Statistics> Stats = new List<Statistics>();
        int Count = -1;
        int CurrentWordNumber { get; set; }

        void Button_enable(bool state)
        {
            Button1.Enabled = Button2.Enabled = Button3.Enabled = Button4.Enabled = state;
            if (state)
            {
                Button_next.State = StateButton.Unknown;
                Button_next.button.Text = GetString(Resource.String.Unknown);               
                Button1.Background = GetDrawable(Resource.Drawable.button_style_standard);
                Button2.Background = GetDrawable(Resource.Drawable.button_style_standard);
                Button3.Background = GetDrawable(Resource.Drawable.button_style_standard);
                Button4.Background = GetDrawable(Resource.Drawable.button_style_standard);
            }
            else
            {
                Button_next.State = StateButton.Next;
                Button_next.button.Text = GetString(Resource.String.Next);
            }
        }

        void Random_Button(Button B1, Button B2, Button B3, Button B4)   //загружаем варианты ответа в текст кнопок
        {
            Additional_functions.Random_4_numbers(CurrentWordNumber, dataBase.Count, out List<int> random_numbers);
            B1.Text = dataBase[random_numbers[0]].TranslationWord;
            B2.Text = dataBase[random_numbers[1]].TranslationWord;
            B3.Text = dataBase[random_numbers[2]].TranslationWord;
            B4.Text = dataBase[random_numbers[3]].TranslationWord;
        }

        void NextWord() //new
        {
            Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
            textView.Text = dataBase[CurrentWordNumber].Word.ToString();
            switch (rnd.Next(4))
            {                        
                case 0:
                    {
                        Random_Button(Button1, Button2, Button3, Button4);
                        break;
                    }
                case 1:
                    {
                        Random_Button(Button2, Button1, Button3, Button4);
                        break;
                    }
                case 2:
                    {
                        Random_Button(Button3, Button1, Button2, Button4);
                        break;
                    }
                case 3:
                    {
                        Random_Button(Button4, Button1, Button2, Button3);
                        break;
                    }
            }
        }

        void Answer(Button B1, Button B2, Button B3, Button B4) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            Button_enable(false);
            if (B1.Text == dataBase[CurrentWordNumber].TranslationWord){
                Additional_functions.Update_number_learn(Stats, dataBase[CurrentWordNumber].Word, CurrentWordNumber, dataBase[CurrentWordNumber].NumberLearn -= Magic_constants.TrueAnswer);
                Statistics.AnswerTrue++;
                B1.Background = GetDrawable(Resource.Drawable.button_true);
            }
            else{
                Additional_functions.Update_number_learn(Stats, dataBase[CurrentWordNumber].Word, CurrentWordNumber, dataBase[CurrentWordNumber].NumberLearn += Magic_constants.FalseAnswer);
                Statistics.AnswerFalse++;
                B1.Background = GetDrawable(Resource.Drawable.button_false);
                if (B2.Text == dataBase[CurrentWordNumber].TranslationWord)
                    B2.Background = GetDrawable(Resource.Drawable.button_true);
                else if (B3.Text == dataBase[CurrentWordNumber].TranslationWord)
                    B3.Background = GetDrawable(Resource.Drawable.button_true);
                else
                    B4.Background = GetDrawable(Resource.Drawable.button_true);
            }
        }

        void Unknown()
        {
            Button tmp;
            if      (Button1.Text == dataBase[CurrentWordNumber].TranslationWord) tmp = Button1;
            else if (Button2.Text == dataBase[CurrentWordNumber].TranslationWord) tmp = Button2;
            else if (Button3.Text == dataBase[CurrentWordNumber].TranslationWord) tmp = Button3;
            else tmp = Button4;
            Statistics.AnswerFalse++;
            Additional_functions.Update_number_learn(Stats, dataBase[CurrentWordNumber].Word, CurrentWordNumber, dataBase[CurrentWordNumber].NumberLearn += Magic_constants.NeutralAnswer);
            tmp.Background = GetDrawable(Resource.Drawable.button_true);
        }

        void Update_Database() // изменение у BD элемента NumberLearn
        {
            var database = DataBase.Connect(Database_Name.English_DB);          
            database.CreateTable<Database_Words>();
            for (int i = 0; i < Stats.Count; i++)
            {
                database.Query<Database_Words>("UPDATE " + DataBase.TableNameLanguage + " SET DateRecurrence = DATETIME('NOW') WHERE Word = ?", Stats[i].Word);
                database.Query<Database_Words>("UPDATE " + DataBase.TableNameLanguage + " SET NumberLearn = " + Stats[i].Learn + " WHERE Word = ?", Stats[i].Word);
            }
        }

        [Java.Interop.Export("Button_Speak_Languages_Click")]
        public async void Button_Speak_Languages_Click(View v) => await CrossTextToSpeech.Current.Speak(textView.Text);

        [Java.Interop.Export("Button_Languages_1_Click")]
        public void Button_Languages_1_Click(View v) =>  Answer(Button1, Button2, Button3, Button4);
    
        [Java.Interop.Export("Button_Languages_2_Click")]
        public void Button_Languages_2_Click(View v) => Answer(Button2, Button1, Button3, Button4);

        [Java.Interop.Export("Button_Languages_3_Click")]
        public void Button_Languages_3_Click(View v) => Answer(Button3, Button1, Button2, Button4);

        [Java.Interop.Export("Button_Languages_4_Click")]
        public void Button_Languages_4_Click(View v) => Answer(Button4, Button1, Button2, Button3);

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
                if (Count < Magic_constants.NumberOfRepeatsLanguage - 1)
                {
                    Count++;
                    Random rnd = new System.Random(unchecked((int)(DateTime.Now.Ticks)));
                    CurrentWordNumber = rnd.Next(dataBase.Count);
                    NextWord();
                    Button_enable(true);
                    Title_textView.Text = Convert.ToString(GetString(Resource.String.Repeat) + " " + (Count + 1) + "/" + Magic_constants.NumberOfRepeatsLanguage); // ПЕРЕДЕЛАЙ Костыль счётчик 
                }
                else
                {
                    Statistics.Add_Statistics(Statistics.AnswerTrue, Statistics.AnswerFalse, DataBase.TableNameLanguage.ToString());
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
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Languages_Repeat);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguagesRepeat);
            Title_textView = toolbarMain.FindViewById<TextView>(Resource.Id.Repeat_toolbar_textview);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            Statistics.Statistics_update();
            textView = FindViewById<TextView>(Resource.Id.textView_Eng_Word);
            Button1 = FindViewById<Button>(Resource.Id.button_Languages_choice1);
            Button2 = FindViewById<Button>(Resource.Id.button_Languages_choice2);
            Button3 = FindViewById<Button>(Resource.Id.button_Languages_choice3);
            Button4 = FindViewById<Button>(Resource.Id.button_Languages_choice4);

            Button_next = new ButtonNext
            {
                button = FindViewById<Button>(Resource.Id.button_Languages_Next),
                State = StateButton.Next
            };

            try
            {
                SQLiteConnection db = DataBase.Connect(Database_Name.English_DB);
                dataBase = db.Query<Database_Words>("SELECT * FROM " + DataBase.TableNameLanguage + " WHERE NumberLearn > 0");
                Button_Languages_Next_Click(null);
            }
            catch
            {
                Toast.MakeText(this, GetString(Resource.String.DatabaseNotConnect), ToastLength.Short).Show();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}