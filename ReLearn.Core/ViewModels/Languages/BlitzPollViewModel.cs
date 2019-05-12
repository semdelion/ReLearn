using System.Collections.Generic;
using MvvmCross.Navigation;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Facade;

namespace ReLearn.Core.ViewModels.Languages
{
    public class BlitzPollViewModel : MvxBlitzPollViewModel<List<DatabaseWords>>
    {
        #region Constructors

        public BlitzPollViewModel(IMvxNavigationService navigationService) :
            base(navigationService)
        {
        }

        #endregion

        #region Properties

        public List<DatabaseWords> Database { get; set; }

        #endregion

        #region Public

        public override void Prepare(List<DatabaseWords> parameter)
        {
            Database = parameter;
        }

        #endregion

        #region Fields

        #endregion

        #region Commands

        #endregion

        #region Services

        #endregion

        #region Private

        #endregion

        #region Protected

        #endregion
    }
}