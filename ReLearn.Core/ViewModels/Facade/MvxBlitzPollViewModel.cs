using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API;
using ReLearn.API.Database.Interface;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ReLearn.Core.ViewModels.Facade
{

    public abstract class MvxBlitzPollViewModel<ListDatabase> : MvxViewModel<ListDatabase>
    {
        #region Fields
        #endregion

        #region Commands
        private IMvxAsyncCommand _toStatistic;
        public IMvxAsyncCommand ToStatistic => _toStatistic ?? (_toStatistic = new MvxAsyncCommand(NavigateToStatistic));
        #endregion

        #region Properties
        protected string _titleCount;
        public string TitleCount
        {
            get => _titleCount;
            set => SetProperty(ref _titleCount, value);
        }
        public Timer Timer { get; set; }
        public bool Answer { get; set; }
        public int CurrentNumber { get; set; }
        public int Time { get; set; }
        public int True { get; set; } = 0;
        public int False { get; set; } = 0;
        protected string _timerText;
        public string TimerText
        {
            get => _timerText;
            set => SetProperty(ref _timerText, value);
        }
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public MvxBlitzPollViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
            Time = Settings.TimeToBlitz * 10;
        }
        #endregion

        #region Private
        protected virtual async Task<bool> NavigateToStatistic() => await NavigationService.Navigate<StatisticViewModel>();
        #endregion

        #region Protected
        #endregion

        #region Public
        public void Cancel() => Timer.Dispose();
        #endregion














       
    }
}
