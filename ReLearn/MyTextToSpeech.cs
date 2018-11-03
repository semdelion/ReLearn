using Android.Content;
using Android.Speech.Tts;

namespace ReLearn
{
    class MyTextToSpeech : Java.Lang.Object, TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        string toSpeak;
        public void Speak(string text, Context context)
        {
            toSpeak = text;
            if (speaker == null)
                speaker = new TextToSpeech(context, this);
            else
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
        }

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.SetLanguage(Settings.CurrentPronunciation == Pronunciation.en.ToString() ? Java.Util.Locale.Us : Java.Util.Locale.Uk);
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }

    }
}