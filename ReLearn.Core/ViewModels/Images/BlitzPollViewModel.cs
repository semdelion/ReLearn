using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Facade;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace ReLearn.Core.ViewModels.Images
{
    public class BlitzPollViewModel : MvxBlitzPollViewModel<List<DatabaseImages>>
    {
        #region Fields
        #endregion

        #region Commands
        #endregion

        #region Properties
        public List<DatabaseImages> Database { get; set; }
        #endregion

        #region Services
        #endregion

        #region Constructors
        public BlitzPollViewModel(IMvxNavigationService navigationService) :
            base(navigationService) { }
        #endregion

        #region Private
        #endregion

        #region Protected
        #endregion

        #region Public
        public override void Prepare(List<DatabaseImages> parameter) => Database = parameter;
        #endregion
    }
}
