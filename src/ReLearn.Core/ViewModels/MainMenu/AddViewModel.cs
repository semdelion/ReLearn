using MvvmCross.Commands;
using ReLearn.API.Database;
using ReLearn.Core.Services;
using ReLearn.Core.ViewModels.Base;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class AddViewModel : BaseViewModel
    {
        #region Fields
        private string _word;
        private string _translationWord;
        #endregion

        #region Commands
        private IMvxAsyncCommand _toDictionaryReplenishment;
        public IMvxAsyncCommand ToDictionaryReplenishment =>
          _toDictionaryReplenishment ?? (_toDictionaryReplenishment = new MvxAsyncCommand(NavigateToDictionaryReplenishment));

        public IMvxAsyncCommand AddWordCommand => new MvxAsyncCommand(AddWord);
        #endregion

        #region Properties
        public string ButtonAdd => this["Buttons.Add"];
        public string HintWordEnglish => this["Hints.WordEnglish"];
        public string HintWordRus => this["Hints.WordRus"];

        public string Word
        {
            get => _word;
            set => SetProperty(ref _word, value);
        }

        public string TranslationWord
        {
            get => _translationWord;
            set => SetProperty(ref _translationWord, value);
        }
        #endregion

        #region Services
        protected IMessageCore Message { get; }
        #endregion

        #region Constructors
        public AddViewModel(IMessageCore imessage)
        {
            Message = imessage;
        }
        #endregion

        #region Private
        private Task<bool> NavigateToDictionaryReplenishment()
        {
            return NavigationService.Navigate<AddDictionaryViewModel>();
        }

        private async Task AddWord()
        {
            if (Word == "" || Word == null || TranslationWord == null || TranslationWord == "")
            {
                Message.Toast(this["EnterWord"]);
            }
            else if (await Task.Run(() => DatabaseWords.WordIsContained(Word.ToLower())))
            {
                Message.Toast(this["WordExists"]);
            }
            else
            {
                await Task.Run(() => DatabaseWords.Insert(Word.ToLower(), TranslationWord.ToLower()));
                Word = TranslationWord = "";
                Message.Toast(this["WordAdded"]);
            }
        }
        #endregion
    }
}