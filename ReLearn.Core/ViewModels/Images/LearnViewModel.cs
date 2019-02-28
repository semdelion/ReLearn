using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using System.Collections.Generic;

namespace ReLearn.Core.ViewModels.Images
{
    public class LearnViewModel : MvxViewModel<List<DBImages>>
    {
        #region Fields
        #endregion

        #region Commands
        #endregion

        #region Properties
        public List<DBImages> Database { get; set; }
        public int Count { get; set; }
        private string _imageName;
        public string ImageName
        {
            get => _imageName;
            set => SetProperty(ref _imageName, value);
        }
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public LearnViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
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

        public override void Prepare(List<DBImages> parameter)
        {
            Database = parameter;
        }
        #endregion
    }
}
