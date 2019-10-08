using Android.App;
using Android.Speech.Tts;
using Java.Lang;
using Java.Util;
using MvvmCross;
using MvvmCross.Platforms.Android;
using ReLearn.API;
using ReLearn.Core.Services;

namespace ReLearn.Droid.Services
{
    public class TextToSpeech : Object, Android.Speech.Tts.TextToSpeech.IOnInitListener, ITextToSpeech
    {
        private Android.Speech.Tts.TextToSpeech speaker;
        public Activity CurrentActivity => Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
        private string ToSpeak { get; set; }

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.SetLanguage(Settings.CurrentPronunciation == $"{Pronunciation.en}" ? Locale.Us : Locale.Uk);
                speaker.Speak(ToSpeak, QueueMode.Flush, null, null);
            }
        }

        public void Speak(string text)
        {
            ToSpeak = text;
            if (speaker == null)
                speaker = new Android.Speech.Tts.TextToSpeech(CurrentActivity.BaseContext, this);
            else
                speaker.Speak(ToSpeak, QueueMode.Flush, null, null);
        }
    }
}