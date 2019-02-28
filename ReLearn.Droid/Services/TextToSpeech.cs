﻿using Android.App;
using Android.Content;
using Android.Speech.Tts;
using MvvmCross;
using MvvmCross.Platforms.Android;
using ReLearn.API;
using ReLearn.Core.Services;

namespace ReLearn.Droid.Services
{
    public class TextToSpeech : Java.Lang.Object, Android.Speech.Tts.TextToSpeech.IOnInitListener, ITextToSpeech
    {
        public Activity CurrentActivity => Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
        Android.Speech.Tts.TextToSpeech speaker;
        private string ToSpeak { get; set; }
        public void Speak(string text)
        {
            ToSpeak = text;
            if (speaker == null)
                speaker = new Android.Speech.Tts.TextToSpeech(CurrentActivity.BaseContext, this);
            else
                speaker.Speak(ToSpeak, QueueMode.Flush, null, null);
        }

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.SetLanguage(Settings.CurrentPronunciation == $"{Pronunciation.en}" ? Java.Util.Locale.Us : Java.Util.Locale.Uk);
                speaker.Speak(ToSpeak, QueueMode.Flush, null, null);
            }
        }
    }
}