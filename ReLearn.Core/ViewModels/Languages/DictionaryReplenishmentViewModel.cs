using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Languages
{
    public class DictionaryReplenishmentViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        public IMvxAsyncCommand AddWordsCommand => new MvxAsyncCommand(AddWords);
        //<Button
        //       local:MvxBind				="Click AddWordsCommand"
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
        public DictionaryReplenishmentViewModel(IMvxNavigationService navigationService, IMessageCore imessage)
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
                await Task.Run(() =>
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        var str = text[i].Split('|');
                        if (!DBWords.WordIsContained(str[0].Trim()))
                            DBWords.Insert(str[0].Trim(), str[1].Trim());
                    }
                });
                Message.Toast("Resource.String.WordsAdded");
                Words = "";
            }
            else
                Message.Toast("Resource.String.DataCorrectness");
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
        public override void ViewCreated()
        {
            base.ViewCreated();
        }
        #endregion

    }
}
