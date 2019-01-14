using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class SettingsMenuViewModel : MvxViewModel
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
                //{ GetString(Resource.String.Number_of_word_repeats)}
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
                //{GetString(Resource.String.Number_of_image_repeats)}
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
                //{GetString(Resource.String.Time_blitz)}
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

        //public static List<string> _languages = new List<string>() { "English", "Русский" };
        //public List<string> ItemsLanguages
        //{
        //    get => _languages;
        //    set
        //    {
        //        _languages = value;
        //        RaisePropertyChanged(() => ItemsLanguages);
        //    }
        //}

        //public static string _selectedItemLanguage = Settings.Currentlanguage == Language.en.ToString() ? "English" : "Русский";
        //public string SelectedItemLanguage
        //{
        //    get => _selectedItemLanguage;
        //    set
        //    {
        //        _selectedItemLanguage = value;
        //        Settings.Currentlanguage = value == "English" ?
        //            Language.en.ToString() :
        //            Language.ru.ToString();
        //        RaisePropertyChanged(() => SelectedItemLanguage);
        //    }
        //}
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public SettingsMenuViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
            WordsNumber = (Settings.NumberOfRepeatsLanguage - 5) / 5;
            ImagesNumber = (Settings.NumberOfRepeatsImage - 5) / 5;
            TimeToBlitz = (Settings.TimeToBlitz - 15) / 15;
            IsActiveBlitz = Settings.BlitzEnable;
        }
        #endregion

        #region Private
        private void SelectedItemLanguage()
        {
            var actionSheetConfig = new ActionSheetConfig
            {
                Title = "Choose language",
                UseBottomSheet = true,
                Options = new List<ActionSheetOption>
                {
                    new ActionSheetOption("English", () => {Languages = "English";  Settings.Currentlanguage = Language.en.ToString();}),
                    new ActionSheetOption("Русский", () => {Languages = "Русский";  Settings.Currentlanguage = Language.ru.ToString(); }),
                },

                Cancel = new ActionSheetOption("Cancel", () => { }),
            };

            Mvx.IoCProvider.Resolve<IUserDialogs>().ActionSheet(actionSheetConfig);
        }
        private void SelectedItemPronunciation()
        {
            var actionSheetConfig = new ActionSheetConfig
            {
                Title = "Choose pronunciation",
                UseBottomSheet = true,
                Options = new List<ActionSheetOption>
                {
                    new ActionSheetOption("English", () => {Pronunciations = "English";  Settings.CurrentPronunciation =  Pronunciation.en.ToString();}),
                    new ActionSheetOption("British", () => {Pronunciations = "British";  Settings.CurrentPronunciation =  Pronunciation.uk.ToString(); }),
                },
                Cancel = new ActionSheetOption("Cancel", () => { }),
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
//string PronunciationText
//{
//    get => FindViewById<TextView>(Resource.Id.pronunciation).Text; 
//    set => FindViewById<TextView>(Resource.Id.pronunciation).Text = value; 
//}

//string LanguageText
//{
//    get => FindViewById<TextView>(Resource.Id.language).Text; 
//    set => FindViewById<TextView>(Resource.Id.language).Text = value; 
//}

//int CheckedItemLanguage()
//{
//    if (Settings.Currentlanguage == Language.en.ToString())
//    {
//        LanguageText = $"{ GetString(Resource.String.Language) }:   English";
//        return 0;
//    }
//    else
//    {
//        LanguageText = $"{ GetString(Resource.String.Language) }:   Русский";
//        return 1;
//    }
//}

//[Java.Interop.Export("TextView_Language_Click")]
//public void TextView_Language_Click(View v)
//{
//    string[] listLanguage = { "English", "Русский" };
//    int checkedItem = CheckedItemLanguage();

//    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
//    alert.SetTitle(GetString(Resource.String.Language));
//    alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
//    alert.SetSingleChoiceItems(listLanguage, checkedItem, new EventHandler<DialogClickEventArgs>(delegate (object sender, DialogClickEventArgs e)
//    {
//        var dialog = (sender as Android.App.AlertDialog);
//        checkedItem = e.Which;
//        if (listLanguage[e.Which] == "English")
//            Settings.Currentlanguage = Language.en.ToString();
//        else
//            Settings.Currentlanguage = Language.ru.ToString();

//        LanguageText = $"{ GetString(Resource.String.Language) }:   {listLanguage[e.Which]}";
//        StartActivity(typeof(SettingsMenuActivity));
//        Finish();
//        dialog.Dismiss();
//    }));
//    alert.Show();
//}

//int CheckedItemPronunciation()
//{
//    if (Settings.CurrentPronunciation == Pronunciation.en.ToString())
//    {
//        PronunciationText = $"{ GetString(Resource.String.Pronunciation) }:  American";
//        return 0;
//    }
//    else
//    {
//        PronunciationText = $"{ GetString(Resource.String.Pronunciation) }:  British";
//        return 1;
//    }
//}

//[Java.Interop.Export("TextView_Pronunciation_Click")]
//public void TextView_Pronunciation_Click(View v)
//{
//    string[] listPronunciation = { "American", "British" };
//    int checkedItem = CheckedItemPronunciation();
//    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
//    alert.SetTitle(GetString(Resource.String.Pronunciation));
//    alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
//    alert.SetSingleChoiceItems(listPronunciation, checkedItem, new EventHandler<DialogClickEventArgs>(delegate (object sender, DialogClickEventArgs e)
//    {
//        var dialog = (sender as Android.App.AlertDialog);
//        checkedItem = e.Which;
//        if (listPronunciation[e.Which] == "American")
//            Settings.CurrentPronunciation = Pronunciation.en.ToString();
//        else
//            Settings.CurrentPronunciation = Pronunciation.uk.ToString();

//        PronunciationText = $"{ GetString(Resource.String.Pronunciation) }:   {listPronunciation[e.Which]}";
//        StartActivity(typeof(SettingsMenuActivity));
//        Finish();
//        dialog.Dismiss();
//    }));
//    alert.Show();
//}