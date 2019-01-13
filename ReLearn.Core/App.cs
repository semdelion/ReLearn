using System;
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
            RegisterAppStart<MainViewModel>();
            DataBase.SetupConnection();
            DBImages.UpdateData();
            DBWords.UpdateData();
        }
    }
}