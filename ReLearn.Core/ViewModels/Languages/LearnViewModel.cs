using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.Localization;
using ReLearn.Core.Services;
using ReLearn.Core.ViewModels.Facade;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Languages
{
    public class LearnViewModel : MvxLearnViewModel<List<DatabaseWords>>
    {
        #region Fields
        #endregion

        #region Commands
        private IMvxAsyncCommand _nextCommand;
        public IMvxAsyncCommand NextCommand => _nextCommand ?? (_nextCommand = new MvxAsyncCommand(Next));

        private IMvxAsyncCommand _notRepeatCommand;
        public IMvxAsyncCommand NotRepeatCommand => _notRepeatCommand ?? (_notRepeatCommand = new MvxAsyncCommand(NotRepeat));

        private IMvxCommand _voiceCommand;
        public IMvxCommand VoiceCommand => _voiceCommand ?? (_voiceCommand = new MvxCommand(() => TextToSpeech.Speak(Word)));
        #endregion

        #region Properties
        public List<DatabaseWords> Database { get; set; }
        public bool VoiceEnable { get; set; } = true;

        public string Word { get; set; }
        #endregion

        #region Services
        public ITextToSpeech TextToSpeech { get; }
        #endregion

        #region Constructors
        public LearnViewModel(ITextToSpeech textToSpeech) => TextToSpeech = textToSpeech;
        #endregion

        #region Private
        private async Task Next()
        {
            if (Count < Database.Count)
            {
                Word = Database[Count].Word;
                Text = $"{Word}{(Database[Count].Transcription == null ? "" : $"\n\n{Database[Count].Transcription}")}" +
                       $"\n\n{Database[Count++].TranslationWord}";
                await DatabaseWords.UpdateLearningNext(Word);
                if (VoiceEnable)
                    TextToSpeech.Speak(Word);
            }
            else
                Mvx.IoCProvider.Resolve<IMessageCore>().Toast(AppResources.LearnViewModel_DictionaryOver);
        }

        private async Task NotRepeat()
        {
            await DatabaseWords.UpdateLearningNotRepeat(Word);
            await Next();
        }
        #endregion

        #region Protected
        #endregion

        #region Public
        public override void Prepare(List<DatabaseWords> parameter) => Database = parameter;
        
        public override async Task Initialize()
        {
            await base.Initialize();
            await Next();
        }
        #endregion
    }
}
