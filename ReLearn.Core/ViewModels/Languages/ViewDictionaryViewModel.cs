using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ReLearn.Core.ViewModels.Languages
{
    public class ViewDictionaryViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        #endregion

        #region Properties
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public ViewDictionaryViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Private
        #endregion

        #region Protected
        #endregion

        #region Public
        public override void ViewCreated()
        {
            base.ViewCreated();
        }
        #endregion
    }
}