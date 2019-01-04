using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels.Languages;

namespace ReLearn.Droid.Languages
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class DictionaryReplenishmentActivity : MvxAppCompatActivity<DictionaryReplenishmentViewModel>
    {

        [Java.Interop.Export("Button_Languages_Add_DictionaryReplenishment")]
        public void Button_Languages_Add_DictionaryReplenishment(View button)
        {
            var text = FindViewById<EditText>(Resource.Id.editText_DictionaryReplenishment).Text.Trim('\n').ToLower().Split('\n');
            if (ValidationOfEnteredData(text))
            {
                for (int i = 0; i < text.Length; i++)
                {
                    var str = text[i].Split('|');
                    if (!DBWords.WordIsContained(str[0].Trim()))
                        DBWords.Insert(str[0].Trim(), str[1].Trim());
                }

                Toast.MakeText(this, GetString(Resource.String.WordsAdded), ToastLength.Short).Show();
                FindViewById<EditText>(Resource.Id.editText_DictionaryReplenishment).Text = "";
            }
            else
                Toast.MakeText(this, GetString(Resource.String.DataCorrectness), ToastLength.Short).Show();
        }

        public bool ValidationOfEnteredData(string[] text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Trim().Split('|').Length != 2)
                    return false;
                //else if (text[i].Split('|')[0].Any(wordByte => wordByte > 191) || text[i].Split('|')[1].Any(wordByte => wordByte > 164 && wordByte < 123 )) // первое - английское, второе - русское
                //    return false;
            }
            return true;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LanguagesDictionaryReplenishmentActivity);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarDictionaryReplenishment);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }    

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_DictionaryReplenishment, menu);
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.dictionary_replenishment_instruction:
                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                    LayoutInflater factory = LayoutInflater.From(this);
                    alert.SetView(factory.Inflate(Resource.Layout.LanguagesAlertImageActivity, null));
                    alert.SetTitle(Resource.String.Instruction);
                    alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
                    alert.Show();
                    return true;
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}