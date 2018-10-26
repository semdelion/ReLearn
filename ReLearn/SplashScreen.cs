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
            Additional_functions.Update_Configuration_Locale(this.Resources);
            base.OnCreate(savedInstanceState);
            FrameStatistics.plain = Android.Graphics.Typeface.CreateFromAsset(Assets, Magic_constants.font);
            DataBase.Install_database_from_assets(Database_Name.English_DB);
            DataBase.Install_database_from_assets(Database_Name.Flags_DB);
            DataBase.Install_database_from_assets(Database_Name.Statistics);
            DataBase.СreateNewTableToLanguagesDataBase();
            Intent intent = new Intent(this, typeof(MainActivity));        
            StartActivity(typeof(MainActivity));
            this.Finish();
        }
    }
}



