using MvvmCross.Navigation;
using ReLearn.Core.ViewModels.Base;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Constructors
        #endregion

        #region Services
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