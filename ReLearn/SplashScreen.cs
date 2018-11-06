using Android.App;
using Android.Content;
using Android.OS;
using Plugin.Settings;

namespace ReLearn
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/SplashTheme", NoHistory = true)]
    public class SplashScreen : Activity
    {     
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Update_Configuration_Locale(this.Resources);
            base.OnCreate(savedInstanceState);
            FrameStatistics.Plain = Android.Graphics.Typeface.CreateFromAsset(Assets, Settings.font);
            DataBase.InstallDatabaseFromAssets(Database_Name.English_DB);
            DataBase.InstallDatabaseFromAssets(Database_Name.Flags_DB);
            DataBase.InstallDatabaseFromAssets(Database_Name.Statistics);
            DataBase.СreateNewTableToLanguagesDataBase();     
            StartActivity(typeof(MainActivity));
            this.Finish();
        }
    }
}



