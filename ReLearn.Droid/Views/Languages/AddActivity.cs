using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Calligraphy;
using Android.Support.V7.App;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels.Languages;

namespace ReLearn.Droid.Languages
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AddActivity : MvxAppCompatActivity<AddViewModel>
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
            SetContentView(Resource.Layout.LanguagesAddActivity);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEnglishAdd);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_english_add, menu);
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.dictionary_replenishment:
                    StartActivity(typeof(DictionaryReplenishmentActivity));
                    return true;
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}