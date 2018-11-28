using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;
using Calligraphy;
using Android.Support.V7.App;
using System.Collections.Generic;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Languages_Learn : AppCompatActivity
    {
        MyTextToSpeech MySpeech { get; set; }
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
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Languages_Learn);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguagesLearn);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            MySpeech = new MyTextToSpeech();
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
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}