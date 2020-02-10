using MvvmCross.Commands;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Base;
using ReLearn.Core.ViewModels.Images;
using ReLearn.Core.ViewModels.MainMenu.SelectDictionary;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class MenuViewModel : BaseViewModel
    {
        #region Commands
        private IMvxAsyncCommand _toAboutUs;
        public IMvxAsyncCommand ToAboutUsViewModel =>
            _toAboutUs ?? (_toAboutUs = new MvxAsyncCommand(NavigateToAboutUsViewModel));

        private IMvxAsyncCommand _toAddition;
        public IMvxAsyncCommand ToAdditionViewModel =>
            _toAddition ?? (_toAddition = new MvxAsyncCommand(NavigateToAdditionViewModel));

        private IMvxAsyncCommand _toFeedback;
        public IMvxAsyncCommand ToFeedbackViewModel =>
            _toFeedback ?? (_toFeedback = new MvxAsyncCommand(NavigateToFeedbackViewModel));

        private IMvxAsyncCommand _toHome;
        public IMvxAsyncCommand ToHomeViewModel => 
            _toHome ?? (_toHome = new MvxAsyncCommand(NavigateToHomeViewModel));

        private IMvxAsyncCommand _toSelectDictionary;
        public IMvxAsyncCommand ToSelectDictionary => 
            _toSelectDictionary ?? (_toSelectDictionary = new MvxAsyncCommand(NavigateToSelectDictionary));

        private IMvxAsyncCommand _toSettings;
        public IMvxAsyncCommand ToSettingsViewModel => 
            _toSettings ?? (_toSettings = new MvxAsyncCommand(NavigateToSettingsViewModel));

        private IMvxAsyncCommand _toStatistic;
        public IMvxAsyncCommand ToStatisticViewModel => 
            _toStatistic ?? (_toStatistic = new MvxAsyncCommand(NavigateToStatisticViewModel));

        private IMvxAsyncCommand _toViewDictionary;
        public IMvxAsyncCommand ToViewDictionaryViewModel => 
            _toViewDictionary ?? (_toViewDictionary = new MvxAsyncCommand(NavigateToViewDictionaryViewModel));
        #endregion

        #region Constructors
        public MenuViewModel()
        {
            NavigateToHomeViewModel().ConfigureAwait(false);
        }
        #endregion

        #region Private
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
            {
                await NavigationService.Navigate<ViewDictionaryViewModel>();
            }
            else
            {
                await NavigationService.Navigate<Languages.ViewDictionaryViewModel>();
            }
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
        #endregion
    }
}