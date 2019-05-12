using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ReLearn.Core.ViewModels.MainMenu.SelectDictionary
{
    public class SelectDictionaryViewModel : MvxViewModel
    {
        #region Constructors

        public SelectDictionaryViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #endregion

        #region Services

        protected IMvxNavigationService NavigationService { get; }

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

        #region Public

        #endregion
    }
}