using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.Localization;
using ReLearn.Core.Services;
using ReLearn.Core.ViewModels.MainMenu.SelectDictionary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class HomeViewModel : MvxViewModel
    {
        private IMvxCommand _toRepeat;
        public IMvxCommand ToRepeat => _toRepeat ?? (_toRepeat = new MvxCommand(NavigateToRepeat));
        private IMvxCommand _toLearn;
        public IMvxCommand ToLearn => _toLearn ?? (_toLearn = new MvxCommand(NavigateToLearn));
        private IMvxAsyncCommand _toSelectDictionary;
        public IMvxAsyncCommand ToSelectDictionary => _toSelectDictionary ?? (_toSelectDictionary = new MvxAsyncCommand(NavigateToSelectDictionary));

        protected IMvxNavigationService NavigationService { get; set; }

        public HomeViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        private void Quiz(bool isImage)
        {
            Settings.TypeOfRepetition = TypeOfRepetitions.Blitz;
            if (isImage)
            {
                var database = DBImages.GetDataNotLearned;
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllImages);
                else
                  NavigationService.Navigate<Images.RepeatViewModel, List<DBImages>>(database);
            }
            else
            {
                var database = DBWords.GetDataNotLearned;
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllWords);
                else
                    NavigationService.Navigate<Languages.RepeatViewModel, List<DBWords>>(DBWords.GetDataNotLearned);
            }
            
        }
        private void Blitz(bool isImage)
        {
            Settings.TypeOfRepetition = TypeOfRepetitions.FourOptions;
            if (isImage)
            {
                var database = DBImages.GetDataNotLearned;
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllImages);
                else
                    NavigationService.Navigate<Images.BlitzPollViewModel, List<DBImages>>(database);
            }
            else
            {
                var database = DBWords.GetDataNotLearned;
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllWords);
                else
                    NavigationService.Navigate<Languages.BlitzPollViewModel, List<DBWords>>(DBWords.GetDataNotLearned);
            }
            
        }

        private void NavigateToRepeat()
        {
            bool isImage = DBImages.DatabaseIsContain($"{DataBase.TableName}");

            if (Settings.QuizEnable && Settings.BlitzEnable)
                if (API.Statistics.Count != 0)
                    Quiz(isImage);
                else
                {
                    if (Settings.TypeOfRepetition == TypeOfRepetitions.Blitz)
                        Blitz(isImage);
                    else
                        Quiz(isImage);
                }
            else if (Settings.QuizEnable)
                Quiz(isImage);
            else
            {
                if (API.Statistics.Count != 0)
                    API.Statistics.Delete();
                Blitz(isImage);
            }
        }
        private void NavigateToLearn()
        {
            if (DBImages.DatabaseIsContain($"{DataBase.TableName}"))
            {
                var Database = DBImages.GetDataNotLearned;
                if (Database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllImages);
                else
                    NavigationService.Navigate<Images.LearnViewModel, List<DBImages>>(Database);
            }
            else
            {
                var Database = DBWords.GetDataNotLearned;
                if(Database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllWords);
                else    
                    NavigationService.Navigate<Languages.LearnViewModel,List<DBWords>>(Database);
            }
        }

        private Task<bool> NavigateToSelectDictionary() => NavigationService.Navigate<SelectDictionaryViewModel>();
    }
}
