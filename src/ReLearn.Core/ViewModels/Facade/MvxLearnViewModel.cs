using ReLearn.Core.ViewModels.Base;

namespace ReLearn.Core.ViewModels.Facade
{
    public abstract class MvxLearnViewModel<ListDatabase> : BaseViewModel<ListDatabase>
    {
        private string _text;

        public int Count { get; set; }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public string TextNotRepeat => this["Buttons.NotRepeat"];
        public string TextNext => this["Buttons.Next"];
    }
}