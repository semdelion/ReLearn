using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
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

        private IMvxAsyncCommand _toStatistic;
        public IMvxAsyncCommand ToStatisticViewModel => _toStatistic ?? (_toStatistic = new MvxAsyncCommand(NavigateToStatistic));

        private IMvxAsyncCommand _toViewDictionary;
        public IMvxAsyncCommand ToViewDictionary => _toViewDictionary ?? (_toViewDictionary = new MvxAsyncCommand(NavigateToViewDictionary));

        private IMvxAsyncCommand _toSettingsMenu;
        public IMvxAsyncCommand ToSettingsMenu => _toSettingsMenu ?? (_toSettingsMenu = new MvxAsyncCommand(NavigateToSettingsMenu));

        private IMvxAsyncCommand _toAddition;
        public IMvxAsyncCommand ToAdditionViewModel => _toAddition ?? (_toAddition = new MvxAsyncCommand(NavigateToAddition));

        private IMvxAsyncCommand _toFeedback;
        public IMvxAsyncCommand ToFeedbackViewModel => _toFeedback ?? (_toFeedback = new MvxAsyncCommand(NavigateToFeedback));

        private IMvxAsyncCommand _toAboutUsViewModel;
        public IMvxAsyncCommand ToAboutUsViewModel => _toAboutUsViewModel ?? (_toAboutUsViewModel = new MvxAsyncCommand(NavigateToAboutUsViewModel));

        protected IMvxNavigationService NavigationService { get; set; }

        public MenuViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        private Task<bool> NavigateToHomeViewModel() => NavigationService.Navigate<HomeViewModel>();
        private Task<bool> NavigateToStatistic() => NavigationService.Navigate<StatisticViewModel>();
        private Task<bool> NavigateToViewDictionary()
        {
            if (DBImages.DatabaseIsContain(DataBase.TableName.ToString()))
                return NavigationService.Navigate<Images.ViewDictionaryViewModel>();
            else
                return NavigationService.Navigate<Languages.ViewDictionaryViewModel>();
        }
        private Task<bool> NavigateToAboutUsViewModel() => NavigationService.Navigate<AboutUsViewModel>();
      
        private Task<bool> NavigateToAddition() => NavigationService.Navigate<Languages.AddViewModel>();
        private Task<bool> NavigateToFeedback() => NavigationService.Navigate<FeedbackViewModel>();
        private Task<bool> NavigateToSettingsMenu() => NavigationService.Navigate<SettingsMenuViewModel>();
    }
}
