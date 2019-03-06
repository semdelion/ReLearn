using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.Services;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Languages
{
    public class RepeatViewModel : MvxViewModel<List<DBWords>>
    {
        #region Fields
        #endregion

        #region Commands
        private IMvxAsyncCommand _toStatistic;
        public IMvxAsyncCommand ToStatistic => _toStatistic ?? (_toStatistic = new MvxAsyncCommand(NavigateToStatistic));
        #endregion

        #region Properties
        public List<DBWords> Database { get; set; }
        private string _text;
        public string Text
        {
            get => _text; 
            set => SetProperty(ref _text, value); 
        }
        private string _titleCount;
        public string TitleCount
        {
            get => _titleCount;
            set => SetProperty(ref _titleCount, value);
        }

        public int CurrentNumber { get; set; }
        public string Word { get; set; }
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        public ITextToSpeech TextToSpeech { get; }
        #endregion

        #region Constructors
        public RepeatViewModel(IMvxNavigationService navigationService, ITextToSpeech textToSpeech)
        {
            NavigationService = navigationService;
            TextToSpeech = textToSpeech;
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
