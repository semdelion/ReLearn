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
            if (Word == "" || TranslationWord == "")
                Toast.MakeText(this, GetString(Resource.String.Enter_word), ToastLength.Short).Show();
            else if (DBWords.WordIsContained(Word))
                Toast.MakeText(this, GetString(Resource.String.Word_exists), ToastLength.Short).Show();               
            else
            {
                DBWords.Add(Word, TranslationWord);
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

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}