using System;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.ViewModels;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels;

namespace ReLearn.Core
{
    public class App : MvxApplication
    {

        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton(() => UserDialogs.Instance);
            RegisterAppStart<MainViewModel>();
            DataBase.SetupConnection();
            DBImages.UpdateData();
            DBWords.UpdateData();
        }
    }
}