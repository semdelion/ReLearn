using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API;
using ReLearn.Core.Localization;
using System;
using System.Collections.Generic;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class SettingsViewModel : MvxViewModel
    {
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
            get { return _wordsNumber; }
            set
            {
                SetProperty(ref _wordsNumber, value);
                WordsNumberText = $"{Convert.ToString(5 + _wordsNumber * 5)}";
                Settings.NumberOfRepeatsLanguage = _wordsNumber * 5 + 5;
            }
        }

        private string _wordsNumberText;
        public string WordsNumberText
        {
            get { return _wordsNumberText; }
            set { SetProperty(ref _wordsNumberText, value);}
        }

        private int _imagesNumber;
        public int ImagesNumber
        {
            get { return _imagesNumber; }
            set
            {
                SetProperty(ref _imagesNumber, value);
                ImagesNumberText = $"{Convert.ToString(5 + _imagesNumber * 5)}";
                Settings.NumberOfRepeatsImage = _imagesNumber * 5 + 5;
            }
        }

        private string _imagesNumberText;
        public string ImagesNumberText
        {
            get { return _imagesNumberText; }
            set { SetProperty(ref _imagesNumberText, value); }
        }

        private string _timeToBlitzText;
        public string TimeToBlitzText
        {
            get { return _timeToBlitzText; }
            set { SetProperty(ref _timeToBlitzText, value); }
        }

        private int _timeToBlitz;
        public int TimeToBlitz
        {
            get { return _timeToBlitz; }
            set
            {
                SetProperty(ref _timeToBlitz, value);
                TimeToBlitzText = $"{Convert.ToString(15 + _timeToBlitz * 15)}";
                Settings.TimeToBlitz = _timeToBlitz * 15 + 15;
            }
        }

        private bool _isActiveBlitz;
        public bool IsActiveBlitz
        {
            get { return _isActiveBlitz; }
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
            get { return _isActiveQuiz; }
            set
            {
                SetProperty(ref _isActiveQuiz, value);
                
                Settings.QuizEnable = _isActiveQuiz;
                if (!value)
                    IsActiveBlitz = true;
            }
        }

        private string _languages = Settings.Currentlanguage == Language.en.ToString() ? "English" : "Русский";
        public string Languages
        {
            get { return _languages; }
            set { SetProperty(ref _languages, value); }
        }

        private string _pronunciations = Settings.CurrentPronunciation == Pronunciation.en.ToString() ? "English" : "British";
        public string Pronunciations
        {
            get { return _pronunciations; }
            set { SetProperty(ref _pronunciations, value); }
        }
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public SettingsViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
            WordsNumber = (Settings.NumberOfRepeatsLanguage - 5) / 5;
            ImagesNumber = (Settings.NumberOfRepeatsImage - 5) / 5;
            TimeToBlitz = (Settings.TimeToBlitz - 15) / 15;
            IsActiveBlitz = Settings.BlitzEnable;
            IsActiveQuiz = Settings.QuizEnable;
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
                    new ActionSheetOption("English", () => {Languages = "English";  Settings.Currentlanguage = Language.en.ToString();}),
                    new ActionSheetOption("Русский", () => {Languages = "Русский";  Settings.Currentlanguage = Language.ru.ToString(); }),
                },

                Cancel = new ActionSheetOption(AppResources.SettingsMenuViewModel_Cancel, () => { }),
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
                    new ActionSheetOption("English", () => {Pronunciations = "English";  Settings.CurrentPronunciation =  Pronunciation.en.ToString();}),
                    new ActionSheetOption("British", () => {Pronunciations = "British";  Settings.CurrentPronunciation =  Pronunciation.uk.ToString(); }),
                },
                Cancel = new ActionSheetOption( AppResources.SettingsMenuViewModel_Cancel, () => { }),
            };
            Mvx.IoCProvider.Resolve<IUserDialogs>().ActionSheet(actionSheetConfig);
        }

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