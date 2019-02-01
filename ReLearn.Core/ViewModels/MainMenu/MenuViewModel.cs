using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class MenuViewModel : MvxViewModel
    {
        private IMvxAsyncCommand _toHome;
        public IMvxAsyncCommand ToHomeViewModel => _toHome ?? (_toHome = new MvxAsyncCommand(NavigateToHomeViewModel));

        private IMvxAsyncCommand _toStatistic;
        public IMvxAsyncCommand ToStatisticViewModel => _toStatistic ?? (_toStatistic = new MvxAsyncCommand(NavigateToStatisticViewModel));

        private IMvxAsyncCommand _toViewDictionary;
        public IMvxAsyncCommand ToViewDictionaryViewModel => _toViewDictionary ?? (_toViewDictionary = new MvxAsyncCommand(NavigateToViewDictionaryViewModel));

        private IMvxAsyncCommand _toSettings;
        public IMvxAsyncCommand ToSettingsViewModel => _toSettings ?? (_toSettings = new MvxAsyncCommand(NavigateToSettingsViewModel));

        private IMvxAsyncCommand _toAddition;
        public IMvxAsyncCommand ToAdditionViewModel => _toAddition ?? (_toAddition = new MvxAsyncCommand(NavigateToAdditionViewModel));

        private IMvxAsyncCommand _toFeedback;
        public IMvxAsyncCommand ToFeedbackViewModel => _toFeedback ?? (_toFeedback = new MvxAsyncCommand(NavigateToFeedbackViewModel));

        private IMvxAsyncCommand _toAboutUs;
        public IMvxAsyncCommand ToAboutUsViewModel => _toAboutUs ?? (_toAboutUs = new MvxAsyncCommand(NavigateToAboutUsViewModel));

        protected IMvxNavigationService NavigationService { get; set; }

        public MenuViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        private Task<bool> NavigateToHomeViewModel() => NavigationService.Navigate<HomeViewModel>();
        private Task<bool> NavigateToStatisticViewModel() => NavigationService.Navigate<StatisticViewModel>();
        private Task<bool> NavigateToViewDictionaryViewModel()
        {
            if (DBImages.DatabaseIsContain(DataBase.TableName.ToString()))
                return NavigationService.Navigate<Images.ViewDictionaryViewModel>();
            else
                return NavigationService.Navigate<Languages.ViewDictionaryViewModel>();
        }
        private Task<bool> NavigateToAboutUsViewModel() => NavigationService.Navigate<AboutUsViewModel>();
      
        private Task<bool> NavigateToAdditionViewModel() => NavigationService.Navigate<AddViewModel>();
        private Task<bool> NavigateToFeedbackViewModel() => NavigationService.Navigate<FeedbackViewModel>();
        private Task<bool> NavigateToSettingsViewModel() => NavigationService.Navigate<SettingsViewModel>();
    }
}
