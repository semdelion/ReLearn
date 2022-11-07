using Android.App;
using Android.Content.PM;
using ReLearn.API.Database;
using ReLearn.Core;
using ReLearn.Droid.Helpers;
using System.Threading.Tasks;
using MvvmCross.Platforms.Android.Views;

namespace ReLearn.Droid
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Theme = "@style/SplashTheme",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity<Setup, App>
    {
        public SplashScreen() : base(Resource.Layout.splash_screen)
        {
            Database.InstallDatabaseFromAssets();
            DataBase.SetupConnection();
            Task.Run(async () =>
            {
                await Database.СreateTableImage();
                await Database.СreateTableLanguage();
                await Database.AddColumn();
            });
        }
    }
}