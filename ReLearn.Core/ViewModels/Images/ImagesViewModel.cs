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
        private IMvxAsyncCommand _toRepeat;
        public IMvxAsyncCommand ToRepeat => _toRepeat ?? (_toRepeat = new MvxAsyncCommand(NavigateToRepeat));
        private IMvxAsyncCommand _toLearn;
        public IMvxAsyncCommand ToLearn => _toLearn ?? (_toLearn = new MvxAsyncCommand(NavigateToLearn));
        private IMvxAsyncCommand _toStatistic;
        public IMvxAsyncCommand ToStatistic => _toStatistic ?? (_toStatistic = new MvxAsyncCommand(NavigateToStatistic));
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
        private Task<bool> NavigateToRepeat()
        {
            if (Settings.TypeOfRepetition == TypeOfRepetitions.Blitz && Statistics.Count == 0 && Settings.BlitzEnable)
            {
                Settings.TypeOfRepetition = TypeOfRepetitions.FourOptions;
                return NavigationService.Navigate<BlitzPollViewModel>();
            }
            else
            {
                Settings.TypeOfRepetition = TypeOfRepetitions.Blitz;
                return NavigationService.Navigate<RepeatViewModel>();
            }
        }
        private Task<bool> NavigateToLearn() => NavigationService.Navigate<LearnViewModel>();

        private Task<bool> NavigateToStatistic() => NavigationService.Navigate<StatisticViewModel>();
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
