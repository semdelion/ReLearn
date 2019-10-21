using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using ReLearn.API;
using ReLearn.Core.Services;
using ReLearn.Core.ViewModels.Base;
using System.Collections.Generic;
using System.Globalization;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Fields
        private readonly App _app = new App();
        private int _wordsNumber;
        private string _wordsNumberText;
        private int _imagesNumber;
        private string _imagesNumberText;
        private int _timeToBlitz;
        private string _timeToBlitzText;
        private bool _isActiveBlitz;
        private bool _isActiveQuiz;
        private string _languages = Settings.Currentlanguage == Language.en ? "English" : "Русский";
        private string _pronunciations = Settings.CurrentPronunciation == $"{Pronunciation.en}" ? "English" : "British";
        private INavigatiomViewUpdater _navigatiomViewUpdater;
        #endregion

        #region Commands
        public IMvxCommand SelectedItemLanguageCommand => new MvxCommand(SelectedItemLanguage);
        public IMvxCommand SelectedItemPronunciationCommand => new MvxCommand(SelectedItemPronunciation);
        #endregion

        #region Properties
        public string TextQuiz => this["Texts.Quiz"];
        public string TextBlitz => this["Texts.Blitz"];
        public int WordsNumber
        {
            get => _wordsNumber;
            set
            {
                SetProperty(ref _wordsNumber, value);
                WordsNumberText = $" {5 + _wordsNumber * 5}";
                Settings.NumberOfRepeatsLanguage = _wordsNumber * 5 + 5;
            }
        }

        public string WordsNumberText
        {
            get => this["Texts.WordRepeats"] + _wordsNumberText;
            set => SetProperty(ref _wordsNumberText, value);
        }

        public int ImagesNumber
        {
            get => _imagesNumber;
            set
            {
                SetProperty(ref _imagesNumber, value);
                ImagesNumberText = $" {5 + _imagesNumber * 5}";
                Settings.NumberOfRepeatsImage = _imagesNumber * 5 + 5;
            }
        }

        public string ImagesNumberText
        {
            get => this["Texts.ImageRepeats"] + _imagesNumberText;
            set => SetProperty(ref _imagesNumberText, value);
        }

        public int TimeToBlitz
        {
            get => _timeToBlitz;
            set
            {
                SetProperty(ref _timeToBlitz, value);
                TimeToBlitzText = $" {15 + _timeToBlitz * 5} ";
                Settings.TimeToBlitz = _timeToBlitz * 5 + 15;
            }
        }

        public string TimeToBlitzText
        {
            get => this["Texts.TimeBlitz"] + _timeToBlitzText + this["Texts.Seconds"];
            set => SetProperty(ref _timeToBlitzText, value);
        }

        public bool IsActiveBlitz
        {
            get => _isActiveBlitz;
            set
            {
                SetProperty(ref _isActiveBlitz, value);
                Settings.BlitzEnable = _isActiveBlitz;
                if (!value)
                {
                    IsActiveQuiz = true;
                }
            }
        }

        public bool IsActiveQuiz
        {
            get => _isActiveQuiz;
            set
            {
                SetProperty(ref _isActiveQuiz, value);

                Settings.QuizEnable = _isActiveQuiz;
                if (!value)
                {
                    IsActiveBlitz = true;
                }
            }
        }

        public string Languages
        {
            get => $"{this["Texts.Language"]}:  {_languages}";
            set => SetProperty(ref _languages, value);
        }

        public string Pronunciations
        {
            get => $"{this["Texts.Pronunciation"]}:  {_pronunciations}";
            set => SetProperty(ref _pronunciations, value);
        }
        #endregion

        #region Constructors
        public SettingsViewModel(INavigatiomViewUpdater navigatiomViewUpdater)
        {
            WordsNumber = (Settings.NumberOfRepeatsLanguage - 5) / 5;
            ImagesNumber = (Settings.NumberOfRepeatsImage - 5) / 5;
            TimeToBlitz = (Settings.TimeToBlitz - 15) / 5;
            IsActiveBlitz = Settings.BlitzEnable;
            IsActiveQuiz = Settings.QuizEnable;
            _navigatiomViewUpdater = navigatiomViewUpdater;
        }
        #endregion

        #region Private
        private void SelectedItemLanguage()
        {
            var actionSheetConfig = new ActionSheetConfig
            {
                Title = this["ChooseLanguage"],
                UseBottomSheet = true,
                Options = new List<ActionSheetOption>
                {
                    new ActionSheetOption("English", () =>
                    {
                        Languages = "English";
                        Settings.Currentlanguage = Language.en;
                        _app.InitializeCultureInfo(new CultureInfo("en-US"));
                        _navigatiomViewUpdater.UpdateNavigatiomView();
                        RaiseAllPropertiesChanged();
                    }),
                    new ActionSheetOption("Русский", () =>
                    {
                        Languages = "Русский";
                        Settings.Currentlanguage = Language.ru;
                        _app.InitializeCultureInfo(new CultureInfo("ru-RU"));
                        _navigatiomViewUpdater.UpdateNavigatiomView();
                        RaiseAllPropertiesChanged();
                    })
                },

                Cancel = new ActionSheetOption(this["Cancel"], () => { })
            };
            Mvx.IoCProvider.Resolve<IUserDialogs>().ActionSheet(actionSheetConfig);
        }

        private void SelectedItemPronunciation()
        {
            var actionSheetConfig = new ActionSheetConfig
            {
                Title = this["ChoosePronunciation"],
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
                Cancel = new ActionSheetOption(this["Cancel"], () => { })
            };
            Mvx.IoCProvider.Resolve<IUserDialogs>().ActionSheet(actionSheetConfig);
        }
        #endregion
    }
}