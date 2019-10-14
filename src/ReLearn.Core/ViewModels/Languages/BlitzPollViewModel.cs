using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Facade;
using System.Collections.Generic;

namespace ReLearn.Core.ViewModels.Languages
{
    public class BlitzPollViewModel : MvxBlitzPollViewModel<List<DatabaseWords>>
    {
        #region Properties
        public string TextNegative => this["Buttons.No"];
        public string TextPositive => this["Buttons.Yes"];

        public List<DatabaseWords> Database { get; set; }
        #endregion

        #region Public
        public override void Prepare(List<DatabaseWords> parameter)
        {
            Database = parameter;
        }
        #endregion
    }
}