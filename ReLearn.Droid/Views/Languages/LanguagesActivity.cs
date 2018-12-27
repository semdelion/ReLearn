using Android.Content;
using Calligraphy;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace ReLearn.Droid.Languages
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LanguagesActivity : MvxAppCompatActivity<Core.ViewModels.Languages.LanguagesViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LanguagesActivity);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguages);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_english, menu);
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.Stats:
                    StartActivity(typeof(StatisticActivity));
                    return true;
                case Resource.Id.Deleteword:
                    StartActivity(typeof(ViewDictionaryActivity));
                    return true;
                case Resource.Id.MenuEnglishSelectDictionary:
                    StartActivity(typeof(SelectDictionaryActivity));
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