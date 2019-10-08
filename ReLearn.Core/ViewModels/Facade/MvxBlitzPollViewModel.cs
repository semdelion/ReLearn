using System.Threading.Tasks;
using System.Timers;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.UI;
using MvvmCross.ViewModels;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Base;
using ReLearn.Core.ViewModels.MainMenu.Statistics;

namespace ReLearn.Core.ViewModels.Facade
{
    public abstract class MvxBlitzPollViewModel<ListDatabase> : BaseViewModel<ListDatabase>
    {
        #region Constructors

        public MvxBlitzPollViewModel()
        {
            Time = Settings.TimeToBlitz * 10;
        }

        #endregion

        #region Services
        #endregion

        #region Private

        protected virtual async Task<bool> NavigateToStatistic()
        {
            return await NavigationService.Navigate<StatisticViewModel>();
        }

        #endregion

        public void TimerStart()
        {
            Timer = new Timer {Interval = 100, Enabled = true};
            Timer.Elapsed += TimerElapsed;
            Timer.Start();
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (Time > 0)
            {
                Time--;
                TimerText = $"{Time / 10}:{Time % 10}0";
                if (Time == 50)
                    CurrentColor = MvxColor.ParseHexString("#ef4049");
            }
            else
            {
                if (Timer != null)
                {
                    Timer.Dispose();
                    await DBStatistics.Insert(True, False, $"{DataBase.TableName}");
                    await NavigateToStatistic();
                    await NavigationService.Close(this);
                }
            }
        }

        #region Fields

        #endregion

        #region Commands

        private IMvxAsyncCommand _toStatistic;

        public IMvxAsyncCommand ToStatistic =>
            _toStatistic ?? (_toStatistic = new MvxAsyncCommand(NavigateToStatistic));

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

        protected MvxColor _currentColor = MvxColor.ParseHexString("#d7f8fe");

        public MvxColor CurrentColor
        {
            get => _currentColor;
            set => SetProperty(ref _currentColor, value);
        }

        #endregion

        #region Protected

        #endregion

        #region Public

        #endregion
    }
}