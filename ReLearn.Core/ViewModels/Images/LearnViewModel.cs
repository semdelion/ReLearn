using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using System.Collections.Generic;

namespace ReLearn.Core.ViewModels.Images
{
    public class LearnViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        #endregion

        #region Properties
        public List<DBImages> Database { get; set; }
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public LearnViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
            Database = DBImages.GetDataNotLearned;
        }
        #endregion

        #region Private
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
