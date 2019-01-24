using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core;
using Android.Content.PM;
using ReLearn.API.Database;
using System.Globalization;

namespace ReLearn.Droid
{
    [Activity(
        Label = "@string/app_name", 
        MainLauncher = true,
        Theme = "@style/SplashTheme",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenAppCompatActivity<Setup, App>
    {
        public SplashScreen() : base(Resource.Layout.splash_screen)
        {
            Database.InstallDatabaseFromAssets();
            DataBase.SetupConnection();
            Database.СreateTableImage();
            Database.СreateTableLanguage();
            Database.ADDCOLUMN();
        }
    }
}
