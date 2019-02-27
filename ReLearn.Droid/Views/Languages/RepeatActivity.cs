using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Languages;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Services;
using System;
using System.Collections.Generic;

namespace ReLearn.Droid.Languages
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RepeatActivity : MvxAppCompatActivity<RepeatViewModel>
    {
        int CurrentWordNumber { get; set; }
        List<Button> Buttons { get; set; }
        ButtonNext ButtonNext { get; set; }
        string Word { get; set; }

        string TitleCount { set => FindViewById<TextView>(Resource.Id.Repeat_toolbar_textview).Text = value;}
        string Text { set => FindViewById<TextView>(Resource.Id.textView_Eng_Word).Text = value;}

        void Button_enable(bool state)
        {
            foreach (var button in Buttons) button.Enabled = state;
            if (state)
            {
                ButtonNext.State = StateButton.Unknown;
                ButtonNext.button.Text = GetString(Resource.String.Unknown);
                foreach (var button in Buttons) button.Background = GetDrawable(Resource.Drawable.button_style_standard);
            }
            else
            {
                ButtonNext.State = StateButton.Next;
                ButtonNext.button.Text = GetString(Resource.String.Next);
            }
        }

        void RandomButton(params Button[] buttons)   //загружаем варианты ответа в текст кнопок
        {
            RandomNumbers.RandomFourNumbers(CurrentWordNumber, ViewModel.Database.Count, out List<int> random_numbers);
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].Text = ViewModel.Database[random_numbers[i]].TranslationWord;
        }

        void NextWord()
        {
            Word = ViewModel.Database[CurrentWordNumber].Word;
            Text = $"{ ViewModel.Database[CurrentWordNumber].Word}{(ViewModel.Database[CurrentWordNumber].Transcription==null ? "" :$"\n{ ViewModel.Database[CurrentWordNumber].Transcription}")}";
            const int four = 4;
            int first = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(four);
            List<int> random_numbers = new List<int> { first, 0, 0, 0 };
            for (int i = 1; i < four; i++)
                random_numbers[i] = (first + i) % four; 
           RandomButton(Buttons[random_numbers[0]], Buttons[random_numbers[1]], Buttons[random_numbers[2]], Buttons[random_numbers[3]]);        
        }

        void Answer(params Button[] buttons) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            API.Statistics.Count++;
            Button_enable(false);
            if (buttons[0].Text == ViewModel.Database[CurrentWordNumber].TranslationWord)
            {
                API.Statistics.Add(ViewModel.Database, CurrentWordNumber, -Settings.TrueAnswer);
                API.Statistics.True++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_true);
            }
            else
            {
                API.Statistics.Add(ViewModel.Database, CurrentWordNumber, Settings.FalseAnswer);
                API.Statistics.False++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_false);
                int index = Buttons.FindIndex(s => s.Text == ViewModel.Database[CurrentWordNumber].TranslationWord);
                Buttons[index].Background = GetDrawable(Resource.Drawable.button_true);
            }
        }

        void Unknown()
        {
            API.Statistics.Count++;
            API.Statistics.False++;
            API.Statistics.Add(ViewModel.Database, CurrentWordNumber, Settings.NeutralAnswer);
            int index =  Buttons.FindIndex(s => s.Text == ViewModel.Database[CurrentWordNumber].TranslationWord);
            Buttons[index].Background = GetDrawable(Resource.Drawable.button_true);
        }

        [Java.Interop.Export("Button_Speak_Languages_Click")]
        public void Button_Speak_Languages_Click(View v) => ViewModel.TextToSpeech.Speak(Word);

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
            ButtonNext.button.Enabled = false;
            if (ButtonNext.State == StateButton.Unknown)
            {
                ButtonNext.State = StateButton.Next;
                Button_enable(false);
                Unknown();
            }
            else
            {
                if (API.Statistics.Count < Settings.NumberOfRepeatsLanguage)
                {
                    CurrentWordNumber = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(ViewModel.Database.Count);
                    NextWord();
                    Button_enable(true);
                    TitleCount = $"{GetString(Resource.String.Repeated)} {API.Statistics.Count + 1}/{Settings.NumberOfRepeatsLanguage}";
                }
                else
                {
                    DBStatistics.Insert(API.Statistics.True, API.Statistics.False, $"{DataBase.TableName}");
                    API.Statistics.Delete();
                    ViewModel.ToStatistic.Execute();
                    Finish();
                }
            }
            ButtonNext.button.Enabled = true;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_languages_repeat);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_languages_repeat);
           
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);
            var _background = BitmapHelper.GetBackgroung(Resources,
            displayMetrics.WidthPixels - PixelConverter.DpToPX(70),
            PixelConverter.DpToPX(190));
            FindViewById<TextView>(Resource.Id.textView_Eng_Word).Background = _background;

            ViewModel.Database = DBWords.GetDataNotLearned;
            Buttons = new List<Button>{
                FindViewById<Button>(Resource.Id.button_Languages_choice1),
                FindViewById<Button>(Resource.Id.button_Languages_choice2),
                FindViewById<Button>(Resource.Id.button_Languages_choice3),
                FindViewById<Button>(Resource.Id.button_Languages_choice4),
            };

            ButtonNext = new ButtonNext
            {
                button = FindViewById<Button>(Resource.Id.button_Languages_Next),
                State = StateButton.Next
            };
            if (ViewModel.Database.Count == 0)
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