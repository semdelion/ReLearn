using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Languages
{
    public class RepeatViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        private IMvxAsyncCommand _toStatistic;
        public IMvxAsyncCommand ToStatistic => _toStatistic ?? (_toStatistic = new MvxAsyncCommand(NavigateToStatistic));
        #endregion

        #region Properties
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public RepeatViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Private
        private Task<bool> NavigateToStatistic() => NavigationService.Navigate<StatisticViewModel>();
        #endregion

        #region Protected
        #endregion

        #region Public
        public override void ViewCreated()
        {
            base.ViewCreated();
        }
        #endregion
    }
}
