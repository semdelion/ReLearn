using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Localization;
using MvvmCross.Plugin.ResxLocalization;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.Localization;
using ReLearn.Core.ViewModels;

namespace ReLearn.Core
{
    public class App : MvxApplication
    {

        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxTextProvider>(new MvxResxTextProvider(AppResources.ResourceManager));
            Mvx.IoCProvider.RegisterSingleton(() => UserDialogs.Instance);
            RegisterAppStart<MainViewModel>();
            DataBase.SetupConnection();
            DBImages.UpdateData();
            DBWords.UpdateData();
        }
    }
}