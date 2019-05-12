using System.Collections.Generic;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API;
using ReLearn.Core.Localization;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class SettingsViewModel : MvxViewModel
    {
        #region Constructors

        public SettingsViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
            WordsNumber = (Settings.NumberOfRepeatsLanguage - 5) / 5;
            ImagesNumber = (Settings.NumberOfRepeatsImage - 5) / 5;
            TimeToBlitz = (Settings.TimeToBlitz - 15) / 5;
            IsActiveBlitz = Settings.BlitzEnable;
            IsActiveQuiz = Settings.QuizEnable;
        }

        #endregion

        #region Services

        protected IMvxNavigationService NavigationService { get; }

        #endregion

        #region Fields

        #endregion

        #region Commands

        public IMvxCommand SelectedItemLanguageCommand => new MvxCommand(SelectedItemLanguage);
        public IMvxCommand SelectedItemPronunciationCommand => new MvxCommand(SelectedItemPronunciation);

        #endregion

        #region Properties

        private int _wordsNumber;

        public int WordsNumber
        {
            get => _wordsNumber;
            set
            {
                SetProperty(ref _wordsNumber, value);
                WordsNumberText = $"{5 + _wordsNumber * 5}";
                Settings.NumberOfRepeatsLanguage = _wordsNumber * 5 + 5;
            }
        }

        private string _wordsNumberText;

        public string WordsNumberText
        {
            get => _wordsNumberText;
            set => SetProperty(ref _wordsNumberText, value);
        }

        private int _imagesNumber;

        public int ImagesNumber
        {
            get => _imagesNumber;
            set
            {
                SetProperty(ref _imagesNumber, value);
                ImagesNumberText = $"{5 + _imagesNumber * 5}";
                Settings.NumberOfRepeatsImage = _imagesNumber * 5 + 5;
            }
        }

        private string _imagesNumberText;

        public string ImagesNumberText
        {
            get => _imagesNumberText;
            set => SetProperty(ref _imagesNumberText, value);
        }

        private string _timeToBlitzText;

        public string TimeToBlitzText
        {
            get => _timeToBlitzText;
            set => SetProperty(ref _timeToBlitzText, value);
        }

        private int _timeToBlitz;

        public int TimeToBlitz
        {
            get => _timeToBlitz;
            set
            {
                SetProperty(ref _timeToBlitz, value);
                TimeToBlitzText = $"{15 + _timeToBlitz * 5}";
                Settings.TimeToBlitz = _timeToBlitz * 5 + 15;
            }
        }

        private bool _isActiveBlitz;

        public bool IsActiveBlitz
        {
            get => _isActiveBlitz;
            set
            {
                SetProperty(ref _isActiveBlitz, value);
                Settings.BlitzEnable = _isActiveBlitz;
                if (!value)
                    IsActiveQuiz = true;
            }
        }

        private bool _isActiveQuiz;

        public bool IsActiveQuiz
        {
            get => _isActiveQuiz;
            set
            {
                SetProperty(ref _isActiveQuiz, value);

                Settings.QuizEnable = _isActiveQuiz;
                if (!value)
                    IsActiveBlitz = true;
            }
        }

        private string _languages = Settings.Currentlanguage == $"{Language.en}" ? "English" : "Русский";

        public string Languages
        {
            get => _languages;
            set => SetProperty(ref _languages, value);
        }

        private string _pronunciations = Settings.CurrentPronunciation == $"{Pronunciation.en}" ? "English" : "British";

        public string Pronunciations
        {
            get => _pronunciations;
            set => SetProperty(ref _pronunciations, value);
        }

        #endregion

        #region Private

        private void SelectedItemLanguage()
        {
            var actionSheetConfig = new ActionSheetConfig
            {
                Title = AppResources.SettingsMenuViewModel_ChooseLanguage,
                UseBottomSheet = true,
                Options = new List<ActionSheetOption>
                {
                    new ActionSheetOption("English", () =>
                    {
                        Languages = "English";
                        Settings.Currentlanguage = $"{Language.en}";
                    }),
                    new ActionSheetOption("Русский", () =>
                    {
                        Languages = "Русский";
                        Settings.Currentlanguage = $"{Language.ru}";
                    })
                },

                Cancel = new ActionSheetOption(AppResources.SettingsMenuViewModel_Cancel, () => { })
            };
            Mvx.IoCProvider.Resolve<IUserDialogs>().ActionSheet(actionSheetConfig);
        }

        private void SelectedItemPronunciation()
        {
            var actionSheetConfig = new ActionSheetConfig
            {
                Title = AppResources.SettingsMenuViewModel_ChoosePronunciation,
                UseBottomSheet = true,
                Options = new List<ActionSheetOption>
                {
                    new ActionSheetOption("English", () =>
                    {
                        Pronunciations = "English";
                        Settings.CurrentPronunciation = $"{Pronunciation.en}";
                    }),
                    new ActionSheetOption("British", () =>
                    {
                        Pronunciations = "British";
                        Settings.CurrentPronunciation = $"{Pronunciation.uk}";
                    })
                },
                Cancel = new ActionSheetOption(AppResources.SettingsMenuViewModel_Cancel, () => { })
            };
            Mvx.IoCProvider.Resolve<IUserDialogs>().ActionSheet(actionSheetConfig);
        }

        #endregion

        #region Protected

        #endregion

        #region Public

        #endregion
    }
}