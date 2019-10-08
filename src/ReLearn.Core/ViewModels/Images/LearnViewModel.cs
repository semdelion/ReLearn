using System.Collections.Generic;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Facade;

namespace ReLearn.Core.ViewModels.Images
{
    public class LearnViewModel : MvxLearnViewModel<List<DatabaseImages>>
    {
        #region Properties

        public List<DatabaseImages> Database { get; set; }

        #endregion

        #region Public

        public override void Prepare(List<DatabaseImages> parameter)
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

        #region Constructors

        #endregion

        #region Private

        #endregion

        #region Protected

        #endregion
    }
}