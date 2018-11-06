using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Calligraphy;
using Android.Support.V7.App;
using System;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Languages_Add : AppCompatActivity
    {
        string Word
        {
            get{ return FindViewById<EditText>(Resource.Id.editText_foreign_word).Text.ToLower(); }
            set{ FindViewById<EditText>(Resource.Id.editText_foreign_word).Text = value.ToLower(); }
        }

        string TranslationWord
        {
            get { return FindViewById<EditText>(Resource.Id.editText_translation_word).Text.ToLower(); }
            set { FindViewById<EditText>(Resource.Id.editText_translation_word).Text = value.ToLower(); }
        }

        [Java.Interop.Export("Button_Languages_Add_Word_Click")]
        public void Button_Languages_Add_Click(View button)
        {
            button.Enabled = false;
            try
            {
                var database = DataBase.Connect(Database_Name.English_DB);
                database.CreateTable<DBWords>();
                // поиск вхождения слова в БД
                if (Word == "" || TranslationWord == "")
                    Toast.MakeText(this, GetString(Resource.String.Enter_word), ToastLength.Short).Show();
                else if (database.Query<DBWords>($"SELECT * FROM {TableNamesLanguage.My_Directly.ToString()} WHERE Word = ?", Word).Count != 0)
                    Toast.MakeText(this, GetString(Resource.String.Word_exists), ToastLength.Short).Show();               
                else
                {
                    var query = $"INSERT INTO {TableNamesLanguage.My_Directly.ToString()} (Word, TranslationWord, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?)";
                    database.Execute(query, Word, TranslationWord, Settings.StandardNumberOfRepeats, DateTime.Now);                                    
                    Toast.MakeText(this, GetString(Resource.String.Word_Added), ToastLength.Short).Show();
                }
                Word = TranslationWord = "";
                button.Enabled = true;
            }
            catch
            {
                Toast.MakeText(this, GetString(Resource.String.DatabaseNotConnect), ToastLength.Short).Show();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Languages_Add);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEnglishAdd);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}