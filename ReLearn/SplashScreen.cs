﻿using Android.App;
using Android.Content;
using Android.OS;

namespace ReLearn
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/SplashTheme", NoHistory = true)]
    public class SplashScreen : Activity
    {     
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Magic_constants.Get_repeat_count("Language_repeat_count");
            Magic_constants.Get_repeat_count("Images_repeat_count");
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



