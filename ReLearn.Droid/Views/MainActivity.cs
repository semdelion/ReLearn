using Android.App;
using Android.OS;
using Android.Views;
using Android.Content.PM;
using Android.Content;
using Calligraphy;
using Android.Support.V7.App;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace ReLearn.Droid
{
    [MvxActivityPresentation]
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Locale)]
    public class MainActivity : MvxAppCompatActivity <MainViewModel>
    {
        [Java.Interop.Export("Button_Languages_Click")]
        public void Button_Languages_Click(View v) => StartActivity(typeof(Languages.Languages));

        [Java.Interop.Export("Button_Images_Click")]
        public void Button_Images_Click(View v) => StartActivity(typeof(Images.Images));

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            SetContentView(Resource.Layout.ActivityMain);
            SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarMain));
            base.OnCreate(savedInstanceState);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.settings, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.about_us:
                    StartActivity(typeof(AboutUs));
                    return true;
                case Resource.Id.Feedback:
                    StartActivity(typeof(Feedback));
                    return true;
                case Resource.Id.Settings_Menu:
                    StartActivity(typeof(SettingsMenu));
                    Finish();
                    return true;
                //case Resource.Id.achievements:
                //    StartActivity(typeof(Achievements));
                //    return true;
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

