using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels.Images;

namespace ReLearn.Droid.Images
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ImagesActivity : MvxAppCompatActivity<ImagesViewModel>
    {    
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ImagesActivity);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarImages);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_flags, menu);
            return base.OnPrepareOptionsMenu(menu);
        }    

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.Stats_Flags:
                    StartActivity(typeof(StatisticActivity));
                    return true;
                case Resource.Id.MenuFlagsSelectDictionary:
                    StartActivity(typeof(SelectDictionaryActivity));
                    return true;
                case Resource.Id.View_dictionary_image:
                    StartActivity(typeof(ViewDictionaryActivity));
                    return true;
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
    
}