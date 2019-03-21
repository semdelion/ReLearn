using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Facade
{
    public abstract class MvxRepeatViewModel<ListDatabase> : MvxViewModel<ListDatabase>
    {
        #region Fields
        #endregion

        #region Commands
        private IMvxAsyncCommand _toStatistic;
        public IMvxAsyncCommand ToStatistic => _toStatistic ?? (_toStatistic = new MvxAsyncCommand(NavigateToStatistic));
        #endregion

        #region Properties
        private string _titleCount;
        public string TitleCount
        {
            get => _titleCount;
            set => SetProperty(ref _titleCount, value);
        }

        public int CurrentNumber { get; set; }
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public MvxRepeatViewModel(IMvxNavigationService navigationService) => NavigationService = navigationService;

        #endregion

        #region Private
        #endregion

        #region Protected
        protected virtual async Task<bool> NavigateToStatistic() => await NavigationService.Navigate<StatisticViewModel>();
        #endregion

        #region Public
        #endregion
    }
}
