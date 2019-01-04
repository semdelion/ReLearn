using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        private IMvxAsyncCommand _toImages;
        public IMvxAsyncCommand ToImages => _toImages ?? (_toImages = new MvxAsyncCommand(NavigateToImages));
        private IMvxAsyncCommand _toLanguages;
        public IMvxAsyncCommand ToLanguages => _toLanguages ?? (_toLanguages = new MvxAsyncCommand(NavigateToLanguages));
        private IMvxAsyncCommand _toAboutUs;
        public IMvxAsyncCommand ToAboutUs => _toAboutUs ?? (_toAboutUs = new MvxAsyncCommand(NavigateToAboutUs));
        private IMvxAsyncCommand _toFeedback;
        public IMvxAsyncCommand ToFeedback => _toFeedback ?? (_toFeedback = new MvxAsyncCommand(NavigateToFeedback));
        private IMvxAsyncCommand _toSettingsMenu;
        public IMvxAsyncCommand ToSettingsMenu => _toSettingsMenu ?? (_toSettingsMenu = new MvxAsyncCommand(NavigateToSettingsMenu));

        #endregion

        #region Properties
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; set; }
        #endregion

        #region Constructors
        public MainViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Private
        private Task<bool> NavigateToImages() => NavigationService.Navigate<Images.ImagesViewModel>();
        private Task<bool> NavigateToLanguages() => NavigationService.Navigate<Languages.LanguagesViewModel>();
        private Task<bool> NavigateToAboutUs() => NavigationService.Navigate<AboutUsViewModel>();
        private Task<bool> NavigateToFeedback() => NavigationService.Navigate<FeedbackViewModel>();
        private Task<bool> NavigateToSettingsMenu() => NavigationService.Navigate<SettingsMenuViewModel>();
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

