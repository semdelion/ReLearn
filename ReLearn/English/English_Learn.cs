using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Plugin.TextToSpeech;
using Android.Content;
using Calligraphy;
using Android.Support.V7.App;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class English_Learn : AppCompatActivity
    {
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

        [Java.Interop.Export("Button_English_Learn_Next_Click")]
        public void Button_English_Learn_Next_Click(View v) => NextRandomWord();              
        
        [Java.Interop.Export("Button_English_Learn_Voice_Click")]
        public void Button_English_Learn_Voice_Click(View v) => CrossTextToSpeech.Current.Speak(Word);
        
        [Java.Interop.Export("Button_English_Learn_Voice_Enable")]
        public void Button_English_Learn_Voice_Enable(View v)
        {
            ImageButton button = FindViewById<ImageButton>(Resource.Id.Button_Speak_TurnOn_TurnOff);
            if (Voice_Enable)
            {
                Voice_Enable = false;
                button.SetImageDrawable(GetDrawable(Resource.Mipmap.speak_off));
                Toast.MakeText(this, Additional_functions.GetResourceString("Voice_off", this.Resources), ToastLength.Short).Show();
            }
            else
            {
                Voice_Enable = true;
                button.SetImageDrawable(GetDrawable(Resource.Mipmap.speak_on));
                Toast.MakeText(this, Additional_functions.GetResourceString("Voice_on", this.Resources), ToastLength.Short).Show();
            }
        }
       
        [Java.Interop.Export(" Button_English_Learn_NotRepeat_Click")]
        public void Button_English_Learn_NotRepeat_Click(View v)
        {
            var db = DataBase.Connect(Database_Name.English_DB);
            db.Query<Database_Words>("UPDATE " + DataBase.TableNameLanguage + " SET NumberLearn = 0 WHERE Word = ?", Word);
            Toast.MakeText(this, Additional_functions.GetResourceString("notRepeat", this.Resources) + ": " + Word, ToastLength.Short).Show();
        }

        void NextRandomWord()
        {
            try
            {
                var db = DataBase.Connect(Database_Name.English_DB);
                var dataBase = db.Query<Database_Words>("SELECT * FROM " + DataBase.TableNameLanguage + " WHERE NumberLearn != 0");
                Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                int rand_word = rnd.Next(dataBase.Count);
                Word = dataBase[rand_word].Word;
                TranslationWord = dataBase[rand_word].TranslationWord;

                if (Voice_Enable)
                    CrossTextToSpeech.Current.Speak(Word);          
            }
            catch
            {
                Toast.MakeText(this, Additional_functions.GetResourceString("databaseNotConnect", this.Resources), ToastLength.Short).Show();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Learn);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEnglishLearn);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            NextRandomWord();              
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
} 