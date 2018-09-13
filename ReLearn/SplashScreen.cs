using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content.PM;
using Java.Util;
using Plugin.Settings;
using Android.Content;
using Calligraphy;
using Android.Content.Res;

namespace ReLearn
{
    [Activity(Label = "", MainLauncher = true, Theme = "@style/SplashTheme", NoHistory = true)]
    public class SplashScreen : Activity
    {     
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            FrameStatistics.plain = Android.Graphics.Typeface.CreateFromAsset(Assets, Magic_constants.font);
            DataBase.Install_database_from_assets(Database_Name.English_DB);
            DataBase.Install_database_from_assets(Database_Name.Flags_DB);
            DataBase.Install_database_from_assets(Database_Name.Statistics);
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(typeof(MainActivity));
            this.Finish();
        }
    }
}



