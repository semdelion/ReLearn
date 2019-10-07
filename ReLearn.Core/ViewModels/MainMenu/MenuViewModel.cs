using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Images;
using ReLearn.Core.ViewModels.MainMenu.SelectDictionary;
using ReLearn.Core.ViewModels.MainMenu.Statistics;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class MenuViewModel : BaseViewModel
    {
        private IMvxAsyncCommand _toAboutUs;

        private IMvxAsyncCommand _toAddition;

        private IMvxAsyncCommand _toFeedback;
        private IMvxAsyncCommand _toHome;

        private IMvxAsyncCommand _toSelectDictionary;

        private IMvxAsyncCommand _toSettings;

        private IMvxAsyncCommand _toStatistic;

        private IMvxAsyncCommand _toViewDictionary;

        public MenuViewModel()
        {
            NavigateToHomeViewModel();
        }

        public IMvxAsyncCommand ToHomeViewModel => _toHome ?? (_toHome = new MvxAsyncCommand(NavigateToHomeViewModel));

        public IMvxAsyncCommand ToSelectDictionary =>
            _toSelectDictionary ?? (_toSelectDictionary = new MvxAsyncCommand(NavigateToSelectDictionary));

        public IMvxAsyncCommand ToStatisticViewModel =>
            _toStatistic ?? (_toStatistic = new MvxAsyncCommand(NavigateToStatisticViewModel));

        public IMvxAsyncCommand ToViewDictionaryViewModel =>
            _toViewDictionary ?? (_toViewDictionary = new MvxAsyncCommand(NavigateToViewDictionaryViewModel));

        public IMvxAsyncCommand ToSettingsViewModel =>
            _toSettings ?? (_toSettings = new MvxAsyncCommand(NavigateToSettingsViewModel));

        public IMvxAsyncCommand ToAdditionViewModel =>
            _toAddition ?? (_toAddition = new MvxAsyncCommand(NavigateToAdditionViewModel));

        public IMvxAsyncCommand ToFeedbackViewModel =>
            _toFeedback ?? (_toFeedback = new MvxAsyncCommand(NavigateToFeedbackViewModel));

        public IMvxAsyncCommand ToAboutUsViewModel =>
            _toAboutUs ?? (_toAboutUs = new MvxAsyncCommand(NavigateToAboutUsViewModel));

        private async Task NavigateToHomeViewModel()
        {
            await NavigationService.Navigate<HomeViewModel>();
        }

        private async Task NavigateToSelectDictionary()
        {
            await NavigationService.Navigate<SelectDictionaryViewModel>();
        }

        private async Task NavigateToStatisticViewModel()
        {
            await NavigationService.Navigate<StatisticViewModel>();
        }

        private async Task NavigateToViewDictionaryViewModel()
        {
            if (DatabaseImages.DatabaseIsContain($"{DataBase.TableName}"))
                await NavigationService.Navigate<ViewDictionaryViewModel>();
            else
                await NavigationService.Navigate<Languages.ViewDictionaryViewModel>();
        }

        private async Task NavigateToAboutUsViewModel()
        {
            await NavigationService.Navigate<AboutUsViewModel>();
        }

        private async Task NavigateToAdditionViewModel()
        {
            await NavigationService.Navigate<AddViewModel>();
        }

        private async Task NavigateToFeedbackViewModel()
        {
            await NavigationService.Navigate<FeedbackViewModel>();
        }

        private async Task NavigateToSettingsViewModel()
        {
            await NavigationService.Navigate<SettingsViewModel>();
        }
    }
}