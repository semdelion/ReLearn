using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Calligraphy;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using ReLearn.Core.ViewModels;

namespace ReLearn.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Label = ""/*, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Locale*/)]
    public class MainActivity : MvxAppCompatActivity<MainViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AdditionalFunctions.Update_Configuration_Locale(this.Resources);
            FrameStatistics.Plain = Android.Graphics.Typeface.CreateFromAsset(Assets, Settings.font);
            AdditionalFunctions.Font();
            SetContentView(Resource.Layout.MainActivity);
            SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarMain));
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
                    StartActivity(typeof(AboutUsActivity));
                    return true;
                case Resource.Id.Feedback:
                    StartActivity(typeof(FeedbackActivity));
                    return true;
                case Resource.Id.Settings_Menu:
                    StartActivity(typeof(SettingsMenuActivity));
                    Finish();
                    return true;
                //        //case Resource.Id.achievements:
                //        //    StartActivity(typeof(Achievements));
                //        //    return true;
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
