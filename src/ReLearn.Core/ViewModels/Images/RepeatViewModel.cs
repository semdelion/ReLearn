using System.Collections.Generic;
using MvvmCross.Navigation;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Facade;

namespace ReLearn.Core.ViewModels.Images
{
    public class RepeatViewModel : MvxRepeatViewModel<List<DatabaseImages>>
    {
        public List<DatabaseImages> Database { get; set; }

        public override void Prepare(List<DatabaseImages> parameter)
        {
            Database = parameter;
        }
    }
}