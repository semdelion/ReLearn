using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;
using ReLearn.API;

namespace ReLearn.Core.ViewModels.Images
{
    public class ImagesViewModel : MvxViewModel
    {

        #region Fields
        #endregion

        #region Commands
       
       
        private IMvxAsyncCommand _toViewDictionary;
        public IMvxAsyncCommand ToViewDictionary => _toViewDictionary ?? (_toViewDictionary = new MvxAsyncCommand(NavigateToViewDictionary));
        private IMvxAsyncCommand _toSelectDictionary;
        public IMvxAsyncCommand ToSelectDictionary => _toSelectDictionary ?? (_toSelectDictionary = new MvxAsyncCommand(NavigateToSelectDictionary));
        #endregion

        #region Properties
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public ImagesViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Private
       

        
        private Task<bool> NavigateToViewDictionary() => NavigationService.Navigate<ViewDictionaryViewModel>();
        private Task<bool> NavigateToSelectDictionary() => NavigationService.Navigate<SelectDictionaryViewModel>();
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
