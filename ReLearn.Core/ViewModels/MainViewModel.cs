using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        #region Constructors

        public MainViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #endregion

        #region Services

        protected IMvxNavigationService NavigationService { get; set; }

        #endregion

        #region Public

        public void ShowMenu()
        {
            NavigationService.Navigate<MenuViewModel>();
        }

        #endregion

        #region Fields

        #endregion

        #region Commands

        #endregion

        #region Properties

        #endregion

        #region Private

        #endregion

        #region Protected

        #endregion
    }
}