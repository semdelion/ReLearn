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
            get => FindViewById<EditText>(Resource.Id.editText_foreign_word).Text.ToLower(); 
            set => FindViewById<EditText>(Resource.Id.editText_foreign_word).Text = value.ToLower(); 
        }

        string TranslationWord
        {
            get => FindViewById<EditText>(Resource.Id.editText_translation_word).Text.ToLower(); 
            set => FindViewById<EditText>(Resource.Id.editText_translation_word).Text = value.ToLower(); 
        }

        [Java.Interop.Export("Button_Languages_Add_Word_Click")]
        public void Button_Languages_Add_Click(View button)
        {
            button.Enabled = false;
            if (Word == "" || TranslationWord == "")
                Toast.MakeText(this, GetString(Resource.String.Enter_word), ToastLength.Short).Show();
            else if (DBWords.WordIsContained(Word))
                Toast.MakeText(this, GetString(Resource.String.Word_exists), ToastLength.Short).Show();               
            else
            {
                DBWords.Insert(Word, TranslationWord);
                Word = TranslationWord = "";
                Toast.MakeText(this, GetString(Resource.String.Word_Added), ToastLength.Short).Show();
            }
            button.Enabled = true;
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

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_english_add, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.dictionary_replenishment)
                StartActivity(typeof(Languages_DictionaryReplenishment));
            else if (item.ItemId == Android.Resource.Id.Home)
                Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}