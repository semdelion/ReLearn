using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.Services;
using System.Collections.Generic;

namespace ReLearn.Core.ViewModels.Languages
{
    public  class LearnViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        #endregion

        #region Properties
        public List<DBWords> Database { get; set; }
        public int Count { get; set; }
        public bool VoiceEnable { get; set; } = true;

        public string Word { get; set; }
        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }


        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        public ITextToSpeech TextToSpeech { get; }
        #endregion

        #region Constructors
        public LearnViewModel(IMvxNavigationService navigationService, ITextToSpeech textToSpeech)
        {
            NavigationService = navigationService;
            TextToSpeech = textToSpeech;
            Database = DBWords.GetDataNotLearned;
        }
        #endregion

        #region Private
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
