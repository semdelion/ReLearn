using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.Localization;
using ReLearn.Core.Services;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class AddDictionaryViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        public IMvxAsyncCommand AddWordsCommand => new MvxAsyncCommand(AddWords);
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);
        #endregion

        #region Properties
        private string _words;
        public string Words
        {
            get { return _words; }
            set { SetProperty(ref _words, value); }
        }
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        protected IMessageCore Message { get; }
        #endregion

        #region Constructors
        public AddDictionaryViewModel(IMvxNavigationService navigationService, IMessageCore imessage)
        {
            NavigationService = navigationService;
            Message = imessage;
        }
        #endregion

        #region Private
        private async Task AddWords() //TODO - перевод, async db
        {
            var text = Words.ToLower().Trim('\n').Split('\n');
            if (await Task.Run(() => ValidationOfEnteredData(text)))
            {
                await Task.Run(async () =>
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        var str = text[i].Split('|');
                        if (!await DatabaseWords.WordIsContained(str[0].Trim()))
                            await DatabaseWords.Insert(str[0].Trim(), str[1].Trim());
                    }
                });
                Message.Toast(AppResources.DictionaryReplenishmentViewModel_WordsAdded);
                Words = "";
            }
            else
                Message.Toast(AppResources.DictionaryReplenishmentViewModel_DataCorrectness);
        }

        private bool ValidationOfEnteredData(string[] text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Trim().Split('|').Length != 2)
                    return false;
                //else if (text[i].Split('|')[0].Any(wordByte => wordByte > 191) || text[i].Split('|')[1].Any(wordByte => wordByte > 164 && wordByte < 123 )) // первое - английское, второе - русское
                //    return false;
            }
            return true;
        }

        #endregion

        #region Protected
        #endregion

        #region Public
        #endregion

    }
}
