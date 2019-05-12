using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ReLearn.Core.ViewModels.MainMenu.Statistics
{
    public class StatisticViewModel : MvxViewModel
    {
        #region Constructors

        public StatisticViewModel(IMvxNavigationService navigationService)
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