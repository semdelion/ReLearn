using Android.App;
using Android.OS;

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
            DataBase.InstallDatabaseFromAssets();
            DataBase.SetupConnection();
            DBWords.СreateTable();
            DBImages.UpdateDate();
            DBWords.UpdateWordsToRepeat();
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }
}



