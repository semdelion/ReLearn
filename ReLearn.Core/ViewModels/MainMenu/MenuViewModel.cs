using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class MenuViewModel : MvxViewModel
    {
        private IMvxAsyncCommand _toHomeViewModel;
        public IMvxAsyncCommand ToHomeViewModel => _toHomeViewModel ?? (_toHomeViewModel = new MvxAsyncCommand(NavigateToHomeViewModel));

        private IMvxAsyncCommand _toAboutUsViewModel;
        public IMvxAsyncCommand ToAboutUsViewModel => _toAboutUsViewModel ?? (_toAboutUsViewModel = new MvxAsyncCommand(NavigateToAboutUsViewModel));

        protected IMvxNavigationService NavigationService { get; set; }

        public MenuViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        private Task<bool> NavigateToHomeViewModel() => NavigationService.Navigate<HomeViewModel>();

        private Task<bool> NavigateToAboutUsViewModel() => NavigationService.Navigate<AboutUsViewModel>();
    }
}
