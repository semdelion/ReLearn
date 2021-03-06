﻿using MvvmCross.Commands;
using MvvmCross.Localization;
using ReLearn.API.Database;
using ReLearn.Core.Services;
using ReLearn.Core.ViewModels.Base;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class AddDictionaryViewModel : BaseViewModel
    {
        #region Fields
        private string _words;
        #endregion

        #region Commands
        public IMvxAsyncCommand AddWordsCommand => new MvxAsyncCommand(AddWords);
        #endregion

        #region Properties
        public string ButtonAdd => this["Buttons.Add"];

        public string HintInsertData => this["Hints.InsertData"];

        public string Words
        {
            get => _words;
            set => SetProperty(ref _words, value);
        }
        #endregion

        #region Services
        protected IMessageCore Message { get; }
        #endregion

        #region Constructors
        public AddDictionaryViewModel(IMessageCore imessage)
        {
            Message = imessage;
        }
        #endregion

        #region Private
        private async Task AddWords()
        {
            var text = Words.ToLower().Trim('\n').Split('\n');
            if (await Task.Run(() => ValidationOfEnteredData(text)))
            {
                await Task.Run(async () =>
                {
                    for (var i = 0; i < text.Length; i++)
                    {
                        var str = text[i].Split('|');
                        if (!await DatabaseWords.WordIsContained(str[0].Trim()))
                        {
                            await DatabaseWords.Insert(str[0].Trim(), str[1].Trim());
                        }
                    }
                });
                Message.Toast(this["WordsAdded"]);
                Words = "";
            }
            else
            {
                Message.Toast(this["DataCorrectness"]);
            }
        }

        private bool ValidationOfEnteredData(string[] text)
        {
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i].Trim().Split('|').Length != 2)
                {
                    return false;
                }
            }
            //else if (text[i].Split('|')[0].Any(wordByte => wordByte > 191) || text[i].Split('|')[1].Any(wordByte => wordByte > 164 && wordByte < 123 )) // первое - английское, второе - русское
            //    return false;
            return true;
        }
        #endregion
    }
}