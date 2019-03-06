using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        #endregion

        #region Properties

        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; set; }

        #endregion

        #region Constructors
        public MainViewModel(IMvxNavigationService navigationService) => NavigationService = navigationService;
        #endregion

        #region Private

        #endregion

        #region Protected
        #endregion

        #region Public
        public void ShowMenu() => NavigationService.Navigate<MenuViewModel>();
        #endregion
    }
}

