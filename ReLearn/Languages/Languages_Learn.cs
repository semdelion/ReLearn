using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Plugin.TextToSpeech;
using Android.Content;
using Calligraphy;
using Android.Support.V7.App;
using System.Collections.Generic;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Languages_Learn : AppCompatActivity
    {
        List<Database_Words> WordDatabase { get; set; }
        
        
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
        public void Button_Languages_Learn_Next_Click(View v) => NextRandomWord();              
        
        [Java.Interop.Export("Button_Languages_Learn_Voice_Click")]
        public void Button_Languages_Learn_Voice_Click(View v) => CrossTextToSpeech.Current.Speak(Word);
        
        [Java.Interop.Export("Button_Languages_Learn_Voice_Enable")]
        public void Button_Languages_Learn_Voice_Enable(View v)
        {
            Voice_Enable = !Voice_Enable;
            FindViewById<ImageButton>(Resource.Id.Button_Speak_TurnOn_TurnOff).SetImageDrawable(
                GetDrawable(Voice_Enable?Resource.Mipmap.speak_off: Resource.Mipmap.speak_on));
        }
       
        [Java.Interop.Export(" Button_Languages_Learn_NotRepeat_Click")]
        public void Button_Languages_Learn_NotRepeat_Click(View v)
        {
            var db = DataBase.Connect(Database_Name.English_DB);
            var query = $"UPDATE " + DataBase.TableNameLanguage + " " + $" SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?";
            db.Execute(query, DateTime.Now, 0, Word);
            NextRandomWord();
        }

        void NextRandomWord()
        {
                      
                //Word = WordDatabase[rand_word].Word;
                //TranslationWord = WordDatabase[rand_word].TranslationWord;
                //if (Voice_Enable)
                //    CrossTextToSpeech.Current.Speak(Word);          

                //Toast.MakeText(this, GetString(Resource.String.DatabaseNotConnect), ToastLength.Short).Show();
            
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Languages_Learn);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguagesLearn);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            var db = DataBase.Connect(Database_Name.English_DB);
            WordDatabase = db.Query<Database_Words>("SELECT * FROM " + DataBase.TableNameLanguage + " WHERE NumberLearn != 0 ORDER BY DateRecurrence ASC");
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