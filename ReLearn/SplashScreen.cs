using Android.App;
using Android.Content;
using Android.OS;

namespace ReLearn
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/SplashTheme", NoHistory = true)]
    public class SplashScreen : Activity
    {     
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Checklanguage();
            base.OnCreate(savedInstanceState);
            FrameStatistics.plain = Android.Graphics.Typeface.CreateFromAsset(Assets, Magic_constants.font);
            DataBase.Install_database_from_assets(Database_Name.English_DB);
            DataBase.Install_database_from_assets(Database_Name.Flags_DB);
            DataBase.Install_database_from_assets(Database_Name.Statistics);
            Intent intent = new Intent(this, typeof(MainActivity));        
            StartActivity(typeof(MainActivity));
            this.Finish();
        }

        void Checklanguage()
        {
            if (System.String.IsNullOrEmpty(Plugin.Settings.CrossSettings.Current.GetValueOrDefault("Language", null)))
                Plugin.Settings.CrossSettings.Current.AddOrUpdateValue("Language", "en");
            Additional_functions.Update_Configuration_Locale(Plugin.Settings.CrossSettings.Current.GetValueOrDefault("Language", null), this.Resources);
        }
    }
}



