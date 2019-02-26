using Android.Content;
using Android.Speech.Tts;
using ReLearn.API;

namespace ReLearn.Droid.Helpers
{
    class TextToSpeech : Java.Lang.Object, Android.Speech.Tts.TextToSpeech.IOnInitListener
    {
        Android.Speech.Tts.TextToSpeech speaker;
        string toSpeak;
        public void Speak(string text, Context context)
        {
            toSpeak = text;
            if (speaker == null)
                speaker = new Android.Speech.Tts.TextToSpeech(context, this);
            else
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
        }

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.SetLanguage(Settings.CurrentPronunciation == $"{Pronunciation.en}" ? Java.Util.Locale.Us : Java.Util.Locale.Uk);
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }
    }
}