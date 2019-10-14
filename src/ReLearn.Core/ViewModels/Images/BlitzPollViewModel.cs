using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Facade;
using System.Collections.Generic;

namespace ReLearn.Core.ViewModels.Images
{
    public class BlitzPollViewModel : MvxBlitzPollViewModel<List<DatabaseImages>>
    {
        #region Properties
        public string TextNegative => this["Buttons.No"];
        public string TextPositive => this["Buttons.Yes"];

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