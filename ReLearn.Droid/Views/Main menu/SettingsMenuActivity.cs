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

namespace ReLearn.Droid
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SettingsMenuActivity : MvxAppCompatActivity<SettingsMenuViewModel>
    {
        //private bool CreateAdapter(MvxSpinner spinner)
        //{
        //    //try
        //    //{
        //    //    var adapter = ArrayAdapter.CreateFromResource(this, Resource.Drawable.spinner_item);
        //    //    adapter.SetDropDownViewResource(Resource.Drawable.spinner_item);
        //    //    spinner.Adapter = adapter;
        //    //    return true;
        //    //}
        //    //catch(Exception ex)
        //    //{
        //    //    throw new Exception(ex.Message);
        //    //}
        ////android: textColor = "@color/TextWhite"
        //}

        private void InitializationAdapters()
        {
            //FindViewById<MvxSpinner>(Resource.Id.LanguageSpinner).= Colors.DarkOrange;
            //CreateAdapter(FindViewById<MvxSpinner>(Resource.Id.PronunciationSpinner));
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MenuSettingsActivity);
            var toolbarSettings = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarSetting);
            SetSupportActionBar(toolbarSettings);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            InitializationAdapters();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            StartActivity(typeof(MainActivity));
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}