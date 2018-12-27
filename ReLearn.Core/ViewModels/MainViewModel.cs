using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        private IMvxAsyncCommand _toImages;
        public IMvxAsyncCommand ToImages => _toImages ?? (_toImages = new MvxAsyncCommand(NavigateToImages));
        private IMvxAsyncCommand _toLanguages;
        public IMvxAsyncCommand ToLanguages => _toLanguages ?? (_toLanguages = new MvxAsyncCommand(NavigateToLanguages));
        #endregion

        #region Properties
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; set; }
        #endregion

        #region Constructors
        public MainViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Private
        private Task<bool> NavigateToImages() => NavigationService.Navigate<Images.ImagesViewModel>();
        private Task<bool> NavigateToLanguages() => NavigationService.Navigate<Languages.LanguagesViewModel>();
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

