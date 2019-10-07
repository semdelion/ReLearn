using MvvmCross.ViewModels;

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
    }
}