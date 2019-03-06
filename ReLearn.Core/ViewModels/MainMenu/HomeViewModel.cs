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
        private async Task Quiz(bool isImage)
        {
            Settings.TypeOfRepetition = TypeOfRepetitions.Blitz;
            if (isImage)
            {
                var database = await DBImages.GetDataNotLearned();
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllImages);
                else
                  await NavigationService.Navigate<Images.RepeatViewModel, List<DBImages>>(database);
            }
            else
            {
                var database = await DBWords.GetDataNotLearned();
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllWords);
                else
                   await NavigationService.Navigate<Languages.RepeatViewModel, List<DBWords>>(database);
            }
            
        }
        private async Task Blitz(bool isImage)
        {
            Settings.TypeOfRepetition = TypeOfRepetitions.FourOptions;
            if (isImage)
            {
                var database = await DBImages.GetDataNotLearned();
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllImages);
                else
                    await NavigationService.Navigate<Images.BlitzPollViewModel, List<DBImages>>(database);
            }
            else
            {
                var database = await DBWords.GetDataNotLearned();
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllWords);
                else
                    await NavigationService.Navigate<Languages.BlitzPollViewModel, List<DBWords>>(database);
            }
            
        }

        private async Task NavigateToRepeat()
        {
            bool isImage = DBImages.DatabaseIsContain($"{DataBase.TableName}");

            if (Settings.QuizEnable && Settings.BlitzEnable)
                if (API.Statistics.Count != 0)
                    await Quiz(isImage);
                else
                {
                    if (Settings.TypeOfRepetition == TypeOfRepetitions.Blitz)
                        await Blitz(isImage);
                    else
                        await Quiz(isImage);
                }
            else if (Settings.QuizEnable)
                await Quiz(isImage);
            else
            {
                if (API.Statistics.Count != 0)
                    API.Statistics.Delete();
                await Blitz(isImage);
            }
        }
        private async Task NavigateToLearn()
        {
            if (DBImages.DatabaseIsContain($"{DataBase.TableName}"))
            {
                var database = await DBImages.GetDataNotLearned();
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllImages);
                else
                    await NavigationService.Navigate<Images.LearnViewModel, List<DBImages>>(database);
            }
            else
            {
                var database = await DBWords.GetDataNotLearned();
                if(database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllWords);
                else
                    await NavigationService.Navigate<Languages.LearnViewModel,List<DBWords>>(database);
            }
        }

        private Task<bool> NavigateToSelectDictionary() => NavigationService.Navigate<SelectDictionaryViewModel>();
    }
}
