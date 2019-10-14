using System.Collections.Generic;
using MvvmCross.Navigation;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Facade;

namespace ReLearn.Core.ViewModels.Images
{
    public class BlitzPollViewModel : MvxBlitzPollViewModel<List<DatabaseImages>>
    {
        public List<DatabaseImages> Database { get; set; }

        public string TextNegative => this["Buttons.No"];
        public string TextPositive => this["Buttons.Yes"];

        public override void Prepare(List<DatabaseImages> parameter)
        {
            Database = parameter;
        }
    }
}