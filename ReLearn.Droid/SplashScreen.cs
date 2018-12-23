using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core;
using Android.Content.PM;

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
        public SplashScreen() : base(Resource.Layout.SplashScreen)
        {
            // AdditionalFunctions.Update_Configuration_Locale(this.Resources);
            //FrameStatistics.Plain = Typeface.CreateFromAsset(Assets, Settings.font);
            DataBase.InstallDatabaseFromAssets();
            DataBase.SetupConnection();
            DBWords.СreateTable();
            DBImages.СreateTable();
            DBWords.ADDCOLUMN();
            DBImages.UpdateData();
            DBWords.UpdateData();
        }
    }
}
