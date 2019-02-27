using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Languages;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Services;
using System.Collections.Generic;

namespace ReLearn.Droid.Languages
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LearnActivity : MvxAppCompatActivity<LearnViewModel>
    {
        [Java.Interop.Export("Button_Languages_Learn_Next_Click")]
        public void Button_Languages_Learn_Next_Click(View v)
        {
            if (ViewModel.Count < ViewModel.Database.Count)
            {
                ViewModel.Word = ViewModel.Database[ViewModel.Count].Word;
                ViewModel.Text = $"{ViewModel.Word}{(ViewModel.Database[ViewModel.Count].Transcription == null ? "" : $"\n\n{ViewModel.Database[ViewModel.Count].Transcription}")}" +
                       $"\n\n{ViewModel.Database[ViewModel.Count++].TranslationWord}";
                DBWords.UpdateLearningNext(ViewModel.Word);
                if (ViewModel.VoiceEnable)
                    ViewModel.TextToSpeech.Speak(ViewModel.Word);
            }
            else
                Toast.MakeText(this, GetString(Resource.String.DictionaryOver), ToastLength.Short).Show();
        }

        [Java.Interop.Export("Button_Languages_Learn_Voice_Click")]
        public void Button_Languages_Learn_Voice_Click(View v) => ViewModel.TextToSpeech.Speak(ViewModel.Word);


        [Java.Interop.Export("Button_Languages_Learn_Voice_Enable")]
        public void Button_Languages_Learn_Voice_Enable(View v)
        {
            ViewModel.VoiceEnable = !ViewModel.VoiceEnable;
            FindViewById<ImageButton>(Resource.Id.Button_Speak_TurnOn_TurnOff).SetImageDrawable(
                GetDrawable(ViewModel.VoiceEnable ? Resource.Mipmap.speak_on :
                                           Resource.Mipmap.speak_off));
        }

        [Java.Interop.Export(" Button_Languages_Learn_NotRepeat_Click")]
        public void Button_Languages_Learn_NotRepeat_Click(View v)
        {
            DBWords.UpdateLearningNotRepeat(ViewModel.Word);
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

            if (ViewModel.Database.Count == 0)
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