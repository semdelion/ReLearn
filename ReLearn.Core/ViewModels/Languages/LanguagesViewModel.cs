using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Languages
{
    public class LanguagesViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        private IMvxAsyncCommand _toRepeat;
        public IMvxAsyncCommand ToRepeat => _toRepeat ?? (_toRepeat = new MvxAsyncCommand(NavigateToRepeat));
        private IMvxAsyncCommand _toLearn;
        public IMvxAsyncCommand ToLearn => _toLearn ?? (_toLearn = new MvxAsyncCommand(NavigateToLearn));
        private IMvxAsyncCommand _toAddition;
        public IMvxAsyncCommand ToAddition => _toAddition ?? (_toAddition = new MvxAsyncCommand(NavigateToAddition));
        #endregion

        #region Properties
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public LanguagesViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Private
        private Task<bool> NavigateToRepeat()
        {
            if (Settings.TypeOfRepetition == TypeOfRepetitions.Blitz && Statistics.Count == 0 && Settings.BlitzEnable)
            {
                Settings.TypeOfRepetition = TypeOfRepetitions.FourOptions;
                return NavigationService.Navigate<BlitzPollViewModel>();
            }
            else
            {
                Settings.TypeOfRepetition = TypeOfRepetitions.Blitz;
                return NavigationService.Navigate<RepeatViewModel>();
            }
        }
        private Task<bool> NavigateToLearn() => NavigationService.Navigate<LearnViewModel>();
        private Task<bool> NavigateToAddition() => NavigationService.Navigate<AddViewModel>();
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
