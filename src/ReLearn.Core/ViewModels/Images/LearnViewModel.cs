using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Facade;
using System.Collections.Generic;

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
    }
}