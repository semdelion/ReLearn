using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Calligraphy;
using Android.Support.V7.App;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class English_Add : AppCompatActivity
    {
        string Word
        {
            get{ return FindViewById<EditText>(Resource.Id.editText_foreign_word).Text; }
            set{ FindViewById<EditText>(Resource.Id.editText_foreign_word).Text = value; }
        }

        string TranslationWord
        {
            get { return FindViewById<EditText>(Resource.Id.editText_translation_word).Text; }
            set { FindViewById<EditText>(Resource.Id.editText_translation_word).Text = value; }
        }

        [Java.Interop.Export("Button_English_Add_Word_Click")]
        public void Button_English_Add_Click(View button)
        {
            button.Enabled = false;
            try
            {
                var database = DataBase.Connect(Database_Name.English_DB);
                database.CreateTable<Database_Words>();
                var search_occurrences = database.Query<Database_Words>("SELECT * FROM " + TableNames.My_Directly.ToString() + " WHERE Word = ?", Word);// поиск вхождения слова в БД
                if (Word == "" || TranslationWord == "")
                    Toast.MakeText(this, Additional_functions.GetResourceString("Enter_word", this.Resources), ToastLength.Short).Show();
                else if (search_occurrences.Count != 0)
                    Toast.MakeText(this, Additional_functions.GetResourceString("Word_exists", this.Resources), ToastLength.Short).Show();               
                else
                {
                    database.Query<Database_Words>($"INSERT INTO "+ TableNames.My_Directly.ToString() + " " + $"(Word, TranslationWord, NumberLearn, DateRecurrence) VALUES (\"" 
                        + Word.ToLower() + "\",\"" + TranslationWord.ToLower() + "\","  + Magic_constants.StandardNumberOfRepeats + ", DATETIME('NOW'))");                   
                    Toast.MakeText(this, Additional_functions.GetResourceString("Word_Added", this.Resources), ToastLength.Short).Show();
                }
                Word = TranslationWord = "";
                button.Enabled = true;
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
            SetContentView(Resource.Layout.English_Add);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEnglishAdd);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}