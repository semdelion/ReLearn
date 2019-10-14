using MvvmCross.Commands;
using ReLearn.Core.ViewModels.Base;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Facade
{
    public abstract class MvxRepeatViewModel<ListDatabase> : BaseViewModel<ListDatabase>
    {
        #region Fields
        private string _titleCount = "1";
        private string _textNext;
        #endregion

        #region Commands
        private IMvxAsyncCommand _toStatistic;

        public IMvxAsyncCommand ToStatistic =>
            _toStatistic ?? (_toStatistic = new MvxAsyncCommand(NavigateToStatistic));
        #endregion

        #region Properties
        public string ButtonEnableText(bool buttonEnable) => buttonEnable ? this["Buttons.Next"] : this["Buttons.Unknown"];

        public string TitleCount
        {
            get => $"{this["Title"]} {_titleCount}";
            set => SetProperty(ref _titleCount, value);
        }

        public string TextNext
        {
            get => _textNext ?? ButtonEnableText(false);
            set => SetProperty(ref _textNext, value);
        }

        public int CurrentNumber { get; set; }
        #endregion

        #region Constructors
        public MvxRepeatViewModel()
        {
        }
        #endregion

        #region Protected
        protected virtual async Task<bool> NavigateToStatistic()
        {
            return await NavigationService.Navigate<StatisticViewModel>();
        }
        #endregion
    }
}