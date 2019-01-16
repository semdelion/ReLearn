using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.MainMenu;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        #region Fields
        
        #endregion

        #region Commands
        private IMvxAsyncCommand _toRepeat;
        public IMvxAsyncCommand ToRepeat => _toRepeat ?? (_toRepeat = new MvxAsyncCommand(NavigateToRepeat));
        private IMvxAsyncCommand _toLearn;
        public IMvxAsyncCommand ToLearn => _toLearn ?? (_toLearn = new MvxAsyncCommand(NavigateToLearn));

        private IMvxAsyncCommand _toSelectDictionary;
        public IMvxAsyncCommand ToSelectDictionary => _toSelectDictionary ?? (_toSelectDictionary = new MvxAsyncCommand(NavigateToSelectDictionary));

        private IMvxAsyncCommand _toStatistic;
        public IMvxAsyncCommand ToStatistic => _toStatistic ?? (_toStatistic = new MvxAsyncCommand(NavigateToStatistic));
        private IMvxAsyncCommand _toViewDictionary;
        public IMvxAsyncCommand ToViewDictionary => _toViewDictionary ?? (_toViewDictionary = new MvxAsyncCommand(NavigateToViewDictionary));
        private IMvxAsyncCommand _toAddition;
        public IMvxAsyncCommand ToAddition => _toAddition ?? (_toAddition = new MvxAsyncCommand(NavigateToAddition));

        private IMvxAsyncCommand _toAboutUs;
        public IMvxAsyncCommand ToAboutUs => _toAboutUs ?? (_toAboutUs = new MvxAsyncCommand(NavigateToAboutUs));
        private IMvxAsyncCommand _toFeedback;
        public IMvxAsyncCommand ToFeedback => _toFeedback ?? (_toFeedback = new MvxAsyncCommand(NavigateToFeedback));
        private IMvxAsyncCommand _toSettingsMenu;
        public IMvxAsyncCommand ToSettingsMenu => _toSettingsMenu ?? (_toSettingsMenu = new MvxAsyncCommand(NavigateToSettingsMenu));
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);
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
        private Task<bool> NavigateToRepeat()
        {
            bool isImage = DBImages.DatabaseIsContain(DataBase.TableName.ToString());
            if (Settings.TypeOfRepetition == TypeOfRepetitions.Blitz && Statistics.Count == 0 && Settings.BlitzEnable)
            {
                Settings.TypeOfRepetition = TypeOfRepetitions.FourOptions;
                return isImage ? 
                    NavigationService.Navigate<Images.BlitzPollViewModel>() : 
                    NavigationService.Navigate<Languages.BlitzPollViewModel>();
            }
            else
            {
                Settings.TypeOfRepetition = TypeOfRepetitions.Blitz;
                return isImage ?
                    NavigationService.Navigate<Images.RepeatViewModel>() :
                    NavigationService.Navigate<Languages.RepeatViewModel>();
            }
        }
        private Task<bool> NavigateToLearn()
        {
            if (DBImages.DatabaseIsContain(DataBase.TableName.ToString()))
                return NavigationService.Navigate<Images.LearnViewModel>();
            else
                return NavigationService.Navigate<Languages.LearnViewModel>();
        }
        


        private Task<bool> NavigateToSelectDictionary() => NavigationService.Navigate<SelectDictionaryViewModel>();

        private Task<bool> NavigateToStatistic() => NavigationService.Navigate<StatisticViewModel>();
        private Task<bool> NavigateToViewDictionary()
        {
            if (DBImages.DatabaseIsContain(DataBase.TableName.ToString()))
                return NavigationService.Navigate<Images.ViewDictionaryViewModel>();
            else
                return NavigationService.Navigate<Languages.ViewDictionaryViewModel>();
        } 
        private Task<bool> NavigateToAddition() => NavigationService.Navigate<Languages.AddViewModel>();

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

