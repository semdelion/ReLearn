using System;
using System.Collections.Generic;

using Calligraphy;
using SQLite;

using Android.Views;
using Android.Widget;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels.Languages;
using Android.Util;
using Android.Graphics.Drawables;

namespace ReLearn.Droid.Languages
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RepeatActivity : MvxAppCompatActivity<RepeatViewModel>
    {
        int CurrentWordNumber { get; set; }
        List<Button> Buttons { get; set; }
        ButtonNext Button_next { get; set; }
        List<DBWords> WordDatabase { get; set; }
        MyTextToSpeech MySpeech { get; set; }
        string Word { get; set; }

        string TitleCount { set => FindViewById<TextView>(Resource.Id.Repeat_toolbar_textview).Text = value;}
        string Text { set => FindViewById<TextView>(Resource.Id.textView_Eng_Word).Text = value;}

        void Button_enable(bool state)
        {
            foreach (var button in Buttons) button.Enabled = state;
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
            AdditionalFunctions.RandomFourNumbers(CurrentWordNumber, WordDatabase.Count, out List<int> random_numbers);
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].Text = WordDatabase[random_numbers[i]].TranslationWord;
        }

        void NextWord()
        {
            Word = WordDatabase[CurrentWordNumber].Word;
            Text = $"{WordDatabase[CurrentWordNumber].Word}{(WordDatabase[CurrentWordNumber].Transcription==null ? "" :$"\n{WordDatabase[CurrentWordNumber].Transcription}")}";
            const int four = 4;
            int first = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(four);
            List<int> random_numbers = new List<int> { first, 0, 0, 0 };
            for (int i = 1; i < four; i++)
                random_numbers[i] = (first + i) % four; 
           Random_Button(Buttons[random_numbers[0]], Buttons[random_numbers[1]], Buttons[random_numbers[2]], Buttons[random_numbers[3]]);        
        }

        void Answer(params Button[] buttons) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            Statistics.Count++;
            Button_enable(false);
            if (buttons[0].Text == WordDatabase[CurrentWordNumber].TranslationWord)
            {
                Statistics.Add(WordDatabase, CurrentWordNumber, -Settings.TrueAnswer);
                Statistics.True++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_true);
            }
            else
            {
                Statistics.Add(WordDatabase, CurrentWordNumber, Settings.FalseAnswer);
                Statistics.False++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_false);
                int index = Buttons.FindIndex(s => s.Text == WordDatabase[CurrentWordNumber].TranslationWord);
                Buttons[index].Background = GetDrawable(Resource.Drawable.button_true);
            }
        }

        void Unknown()
        {
            Statistics.Count++;
            Statistics.False++;
            Statistics.Add(WordDatabase, CurrentWordNumber, Settings.NeutralAnswer);
            int index =  Buttons.FindIndex(s => s.Text == WordDatabase[CurrentWordNumber].TranslationWord);
            Buttons[index].Background = GetDrawable(Resource.Drawable.button_true);
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
                if (Statistics.Count < Settings.NumberOfRepeatsLanguage - 1)
                {
                    CurrentWordNumber = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(WordDatabase.Count);
                    NextWord();
                    Button_enable(true);
                    TitleCount = $"{GetString(Resource.String.Repeated)} {Statistics.Count + 1}/{Settings.NumberOfRepeatsLanguage}";
                }
                else
                {
                    DBStatistics.Insert(Statistics.True, Statistics.False, DataBase.TableNameLanguage.ToString());
                    Statistics.Count = Statistics.True = Statistics.False = 0;
                    StartActivity(typeof(StatisticActivity));
                    Finish();
                }
            }
            Button_next.button.Enabled = true;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LanguagesRepeatActivity);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguagesRepeat);
           
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);
            var _background = new BitmapDrawable(Resources, Background.GetBackgroung(
            displayMetrics.WidthPixels - AdditionalFunctions.DpToPX(70),
            AdditionalFunctions.DpToPX(190)));
            FindViewById<TextView>(Resource.Id.textView_Eng_Word).Background = _background;

            MySpeech = new MyTextToSpeech();
            WordDatabase = DBWords.GetDataNotLearned;
            Statistics.Table = DataBase.TableNameLanguage.ToString();
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
            if (WordDatabase.Count == 0)
            {
                Toast.MakeText(this, GetString(Resource.String.RepeatedAllWords), ToastLength.Short).Show();
                Finish();
                return;
            }
            Button_Languages_Next_Click(null);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}