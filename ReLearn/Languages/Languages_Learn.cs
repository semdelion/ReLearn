using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Android.Content;
using Calligraphy;
using Android.Support.V7.App;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Net;
using Android.Speech.Tts;
using Java.Util;
using Android.Runtime;
using System.Threading.Tasks;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Languages_Learn : AppCompatActivity
    {
        MyTextToSpeech MySpeech { get; set; }
        List<DBWords> WordDatabase { get; set; }
        SQLite.SQLiteConnection DatabaseConnect { get; set; }
        int Count { get; set; }
        bool Voice_Enable = true;

        string Word
        {
            get { return FindViewById<TextView>(Resource.Id.textView_learn_en).Text; }
            set { FindViewById<TextView>(Resource.Id.textView_learn_en).Text = value; }
        }

        string TranslationWord
        {
            get { return FindViewById<TextView>(Resource.Id.textView_learn_ru).Text; }
            set { FindViewById<TextView>(Resource.Id.textView_learn_ru).Text = value; }
        }

        [Java.Interop.Export("Button_Languages_Learn_Next_Click")]
        public void Button_Languages_Learn_Next_Click(View v)
        {
            if (Count < WordDatabase.Count)
            {
                Word = WordDatabase[Count].Word;
                TranslationWord = WordDatabase[Count++].TranslationWord;
                if (Voice_Enable)
                    MySpeech.Speak(Word, this);
                var query = $"UPDATE {DataBase.TableNameLanguage} SET DateRecurrence = ? WHERE Word = ?";
                DatabaseConnect.Execute(query, DateTime.Now, Word);
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
            var query = $"UPDATE {DataBase.TableNameLanguage} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?";
            DatabaseConnect.Execute(query, DateTime.Now, 0, Word);
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
            try
            {
                DatabaseConnect = DataBase.Connect(Database_Name.English_DB);
                WordDatabase = DatabaseConnect.Query<DBWords>($"SELECT * FROM {DataBase.TableNameLanguage} WHERE NumberLearn != 0 ORDER BY DateRecurrence ASC");
                Button_Languages_Learn_Next_Click(null);
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