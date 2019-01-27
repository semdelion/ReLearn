using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class HomeViewModel : MvxViewModel
    {
        protected IMvxNavigationService NavigationService { get; set; }

        public HomeViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
