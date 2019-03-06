using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.Localization;
using ReLearn.Core.Services;
using System.Collections.Generic;

namespace ReLearn.Core.ViewModels.Languages
{
    public class LearnViewModel : MvxViewModel<List<DBWords>>
    {
        #region Fields
        #endregion

        #region Commands
        private IMvxCommand _nextCommand;
        public IMvxCommand NextCommand => _nextCommand ?? (_nextCommand = new MvxCommand(Next));

        private IMvxCommand _notRepeatCommand;
        public IMvxCommand NotRepeatCommand => _notRepeatCommand ?? (_notRepeatCommand = new MvxCommand(NotRepeat));

        private IMvxCommand _voiceCommand;
        public IMvxCommand VoiceCommand => _voiceCommand ?? (_voiceCommand = new MvxCommand(() => TextToSpeech.Speak(Word)));
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
        }
        #endregion

        #region Private
        private async void Next()
        {
            if (Count < Database.Count)
            {
                Word = Database[Count].Word;
                Text = $"{Word}{(Database[Count].Transcription == null ? "" : $"\n\n{Database[Count].Transcription}")}" +
                       $"\n\n{Database[Count++].TranslationWord}";
                await DBWords.UpdateLearningNext(Word);
                if (VoiceEnable)
                    TextToSpeech.Speak(Word);
            }
            else
                Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.LearnViewModel_DictionaryOver);
        }

        private async void NotRepeat()
        {
            await DBWords.UpdateLearningNotRepeat(Word);
            Next();
        }
        #endregion

        #region Protected
        #endregion

        #region Public
        public override void Prepare(List<DBWords> parameter)
        {
            Database = parameter;
            Next();
        }
        #endregion
    }
}
