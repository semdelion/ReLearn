using ReLearn.Core.ViewModels.Base;

namespace ReLearn.Core.ViewModels.Facade
{
    public abstract class MvxLearnViewModel<ListDatabase> : BaseViewModel<ListDatabase>
    {
        #region Fields
        private string _text;
        #endregion

        #region Properties
        public string TextNotRepeat => this["Buttons.NotRepeat"];

        public string TextNext => this["Buttons.Next"];

        public int Count { get; set; }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
        #endregion
    }
}