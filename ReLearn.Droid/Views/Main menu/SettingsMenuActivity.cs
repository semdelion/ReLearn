using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Calligraphy;
using Java.Util;
using Plugin.Settings;
using ReLearn.Droid.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels.MainMenu;
using MvvmCross.Platforms.Android.Binding.Views;
using Acr.UserDialogs;

namespace ReLearn.Droid
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SettingsMenuActivity : MvxAppCompatActivity<SettingsMenuViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MenuSettingsActivity);
            UserDialogs.Init(this);
            var toolbarSettings = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarSetting);
            SetSupportActionBar(toolbarSettings);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            StartActivity(typeof(MainActivity));
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}