using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.API.Database.Interface;
using ReLearn.Core.ViewModels.Facade;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace ReLearn.Core.ViewModels.Languages
{
    public class BlitzPollViewModel : MvxBlitzPollViewModel<List<DatabaseWords>>
    {
        #region Fields
        #endregion

        #region Commands
        #endregion

        #region Properties
        public List<DatabaseWords> Database { get; set; }
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
        public override void Prepare(List<DatabaseWords> parameter) => Database = parameter;
        #endregion
    }
}
