using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Facade;
using System.Collections.Generic;

namespace ReLearn.Core.ViewModels.Images
{
    public class LearnViewModel : MvxLearnViewModel<List<DatabaseImages>>
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
