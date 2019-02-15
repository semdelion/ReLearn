using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API;
using ReLearn.API.Database;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class HomeViewModel : MvxViewModel
    {
        private IMvxAsyncCommand _toRepeat;
        public IMvxAsyncCommand ToRepeat => _toRepeat ?? (_toRepeat = new MvxAsyncCommand(NavigateToRepeat));
        private IMvxAsyncCommand _toLearn;
        public IMvxAsyncCommand ToLearn => _toLearn ?? (_toLearn = new MvxAsyncCommand(NavigateToLearn));
        private IMvxAsyncCommand _toSelectDictionary;
        public IMvxAsyncCommand ToSelectDictionary => _toSelectDictionary ?? (_toSelectDictionary = new MvxAsyncCommand(NavigateToSelectDictionary));

        protected IMvxNavigationService NavigationService { get; set; }

        public HomeViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        private Task<bool> Quiz(bool isImage)
        {
            Settings.TypeOfRepetition = TypeOfRepetitions.Blitz;
            return isImage ?
                NavigationService.Navigate<Images.RepeatViewModel>() :
                NavigationService.Navigate<Languages.RepeatViewModel>();
        }
        private Task<bool> Blitz(bool isImage)
        {
            Settings.TypeOfRepetition = TypeOfRepetitions.FourOptions;
            return isImage ?
                NavigationService.Navigate<Images.BlitzPollViewModel>() :
                NavigationService.Navigate<Languages.BlitzPollViewModel>();
        }

        private Task<bool> NavigateToRepeat()
        {
            bool isImage = DBImages.DatabaseIsContain(DataBase.TableName.ToString());
            if (Settings.QuizEnable && Settings.BlitzEnable)
                if (API.Statistics.Count != 0)
                    return Quiz(isImage);
                else
                    return Settings.TypeOfRepetition == TypeOfRepetitions.Blitz ? Blitz(isImage) : Quiz(isImage);
            else if (Settings.QuizEnable)
                return Quiz(isImage);
            else
            {
                if (API.Statistics.Count != 0)
                    API.Statistics.Delete();
                return Blitz(isImage);
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

    }
}
