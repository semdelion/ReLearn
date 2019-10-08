using MvvmCross;
using MvvmCross.Commands;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.Localization;
using ReLearn.Core.Services;
using ReLearn.Core.ViewModels.Base;
using ReLearn.Core.ViewModels.Images;
using ReLearn.Core.ViewModels.MainMenu.SelectDictionary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class HomeViewModel : BaseViewModel
    {
        private IMvxAsyncCommand _toLearn;
        private IMvxAsyncCommand _toRepeat;

        private IMvxAsyncCommand _toSelectDictionary;

        public IMvxAsyncCommand ToRepeat => _toRepeat ?? (_toRepeat = new MvxAsyncCommand(NavigateToRepeat));
        public IMvxAsyncCommand ToLearn => _toLearn ?? (_toLearn = new MvxAsyncCommand(NavigateToLearn));

        public IMvxAsyncCommand ToSelectDictionary =>
            _toSelectDictionary ?? (_toSelectDictionary = new MvxAsyncCommand(NavigateToSelectDictionary));

        public string TextRepeat => this["Buttons.Repeat"];

        public string TextLearn => this["Buttons.Learn"];

        private async Task Quiz(bool isImage)
        {
            Settings.TypeOfRepetition = TypeOfRepetitions.Blitz;
            if (isImage)
            {
                var database = await DatabaseImages.GetDataNotLearned();
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllImages);
                else
                    await NavigationService.Navigate<RepeatViewModel, List<DatabaseImages>>(database);
            }
            else
            {
                var database = await DatabaseWords.GetDataNotLearned();
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllWords);
                else
                    await NavigationService.Navigate<Languages.RepeatViewModel, List<DatabaseWords>>(database);
            }
        }

        private async Task Blitz(bool isImage)
        {
            Settings.TypeOfRepetition = TypeOfRepetitions.FourOptions;
            if (isImage)
            {
                var database = await DatabaseImages.GetDataNotLearned();
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllImages);
                else
                    await NavigationService.Navigate<BlitzPollViewModel, List<DatabaseImages>>(database);
            }
            else
            {
                var database = await DatabaseWords.GetDataNotLearned();
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllWords);
                else
                    await NavigationService.Navigate<Languages.BlitzPollViewModel, List<DatabaseWords>>(database);
            }
        }

        private async Task NavigateToRepeat()
        {
            var isImage = DatabaseImages.DatabaseIsContain($"{DataBase.TableName}");

            if (Settings.QuizEnable && Settings.BlitzEnable)
            {
                if (API.Statistics.Count != 0)
                {
                    await Quiz(isImage);
                }
                else
                {
                    if (Settings.TypeOfRepetition == TypeOfRepetitions.Blitz)
                        await Blitz(isImage);
                    else
                        await Quiz(isImage);
                }
            }
            else if (Settings.QuizEnable)
            {
                await Quiz(isImage);
            }
            else
            {
                if (API.Statistics.Count != 0)
                    API.Statistics.Delete();
                await Blitz(isImage);
            }
        }

        private async Task NavigateToLearn()
        {
            if (DatabaseImages.DatabaseIsContain($"{DataBase.TableName}"))
            {
                var database = await DatabaseImages.GetDataNotLearned();
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllImages);
                else
                    await NavigationService.Navigate<LearnViewModel, List<DatabaseImages>>(database);
            }
            else
            {
                var database = await DatabaseWords.GetDataNotLearned();
                if (database.Count == 0)
                    Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.HomeViewModel_RepeatedAllWords);
                else
                    await NavigationService.Navigate<Languages.LearnViewModel, List<DatabaseWords>>(database);
            }
        }

        private Task<bool> NavigateToSelectDictionary()
        {
            return NavigationService.Navigate<SelectDictionaryViewModel>();
        }
    }
}