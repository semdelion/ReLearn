using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReLearn.Core.ViewModels.Facade
{
    public abstract class MvxLearnViewModel<ListDatabase> : MvxViewModel<ListDatabase>
    {
        public int Count { get; set; }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
    }
}
