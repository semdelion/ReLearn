using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Languages;
using ReLearn.Droid.Helpers;
using System.Collections.Generic;

namespace ReLearn.Droid.Languages
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LearnActivity : MvxAppCompatActivity<LearnViewModel>
    {
        TextToSpeech MySpeech { get; set; }
        List<DBWords> WordDatabase { get; set; }
        int Count { get; set; }
        bool Voice_Enable = true;

        string Word {get;set;}
        string Text
        {
            get => FindViewById<TextView>(Resource.Id.textView_learn_en).Text;
            set => FindViewById<TextView>(Resource.Id.textView_learn_en).Text = value;
        }

        [Java.Interop.Export("Button_Languages_Learn_Next_Click")]
        public void Button_Languages_Learn_Next_Click(View v)
        {
            if (Count < WordDatabase.Count)
            {
                Word = WordDatabase[Count].Word;
                Text = $"{Word}{(WordDatabase[Count].Transcription == null ? "" : $"\n\n{WordDatabase[Count].Transcription}")}" +
                       $"\n\n{WordDatabase[Count++].TranslationWord}";
                DBWords.UpdateLearningNext(Word);
                if (Voice_Enable)
                    MySpeech.Speak(Word, this);
            }
            else
                Toast.MakeText(this, GetString(Resource.String.DictionaryOver), ToastLength.Short).Show();
        }

        [Java.Interop.Export("Button_Languages_Learn_Voice_Click")]
        public void Button_Languages_Learn_Voice_Click(View v) => MySpeech.Speak(Word, this);


        [Java.Interop.Export("Button_Languages_Learn_Voice_Enable")]
        public void Button_Languages_Learn_Voice_Enable(View v)
        {
            Voice_Enable = !Voice_Enable;
            FindViewById<ImageButton>(Resource.Id.Button_Speak_TurnOn_TurnOff).SetImageDrawable(
                GetDrawable(Voice_Enable ? Resource.Mipmap.speak_on :
                                           Resource.Mipmap.speak_off));
        }

        [Java.Interop.Export(" Button_Languages_Learn_NotRepeat_Click")]
        public void Button_Languages_Learn_NotRepeat_Click(View v)
        {
            DBWords.UpdateLearningNotRepeat(Word);
            Button_Languages_Learn_Next_Click(null);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_languages_learn);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_languages_learn);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);
            var _background = BitmapHelper.GetBackgroung(Resources,
            displayMetrics.WidthPixels - PixelConverter.DpToPX(70),
            PixelConverter.DpToPX(300));

            FindViewById<TextView>(Resource.Id.textView_learn_en).Background = _background;


            MySpeech = new TextToSpeech();
            WordDatabase = DBWords.GetDataNotLearned;

            if (WordDatabase.Count == 0)
            {
                Toast.MakeText(this, GetString(Resource.String.RepeatedAllWords), ToastLength.Short).Show();
                Finish();
                return;
            }

            Button_Languages_Learn_Next_Click(null);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}