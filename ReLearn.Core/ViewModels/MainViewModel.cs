using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReLearn.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        #endregion

        #region Properties
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        protected IMessage Message { get; }
        #endregion

        #region Constructors
        public MainViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
          //Message = message;
        }
        #endregion

        #region Private
        #endregion

        #region Protected
        #endregion

        #region Public
        public override void ViewCreated()
        {
            //Task.Run(PullToRefresh);
            base.ViewCreated();
        }
        #endregion
    }
}
