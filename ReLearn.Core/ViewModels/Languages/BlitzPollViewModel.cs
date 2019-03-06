using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace ReLearn.Core.ViewModels.Languages
{
    public class BlitzPollViewModel : MvxViewModel<List<DBWords>>
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
        public List<DBWords> Database { get; set; }
        public Timer Timer { get; set; }
        public bool Answer { get; set; }
        public int CurrentNumber { get; set; }
        public int Time { get; set; } 
        public int True { get; set; } = 0;
        public int False { get; set; }= 0;
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public BlitzPollViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
            Time = Settings.TimeToBlitz * 10;
        }
        #endregion

        #region Private
        private Task<bool> NavigateToStatistic() => NavigationService.Navigate<StatisticViewModel>();
        #endregion

        #region Protected
        #endregion

        #region Public
        public override void Prepare(List<DBWords> parameter) => Database = parameter;
        #endregion
    }
}
