using MvvmCross.Commands;
using ReLearn.API.Database;
using ReLearn.Core.Localization;
using ReLearn.Core.Services;
using ReLearn.Core.ViewModels.Base;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class AddViewModel : BaseViewModel
    {
        #region Constructors

        public AddViewModel(IMessageCore imessage)
        {
            Message = imessage;
        }

        #endregion

        #region Fields

        #endregion

        #region Commands

        private IMvxAsyncCommand _toDictionaryReplenishment;

        public IMvxAsyncCommand ToDictionaryReplenishment => _toDictionaryReplenishment ??
                                                             (_toDictionaryReplenishment =
                                                                 new MvxAsyncCommand(NavigateToDictionaryReplenishment)
                                                             );

        public IMvxAsyncCommand AddWordCommand => new MvxAsyncCommand(AddWord);

        #endregion

        #region Properties       

        private string _word;

        public string Word
        {
            get => _word;
            set => SetProperty(ref _word, value);
        }

        private string _translationWord;

        public string TranslationWord
        {
            get => _translationWord;
            set => SetProperty(ref _translationWord, value);
        }

        public string Title => this["Title"];
        #endregion

        #region Services
        protected IMessageCore Message { get; }

        #endregion

        #region Private

        private Task<bool> NavigateToDictionaryReplenishment()
        {
            return NavigationService.Navigate<AddDictionaryViewModel>();
        }

        private async Task AddWord() //TODO - перевод, async db
        {
            if (Word == "" || Word == null || TranslationWord == null || TranslationWord == "")
            {
                Message.Toast(AppResources.AddViewModel_EnterWord);
            }
            else if (await Task.Run(() => DatabaseWords.WordIsContained(Word.ToLower())))
            {
                Message.Toast(AppResources.AddViewModel_WordExists);
            }
            else
            {
                await Task.Run(() => DatabaseWords.Insert(Word.ToLower(), TranslationWord.ToLower()));
                Word = TranslationWord = "";
                Message.Toast(AppResources.AddViewModel_WordAdded);
            }
        }

        #endregion

        #region Protected

        #endregion

        #region Public

        #endregion
    }
}