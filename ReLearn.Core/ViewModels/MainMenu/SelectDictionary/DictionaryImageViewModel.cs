using MvvmCross.ViewModels;
using ReLearn.API.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu.SelectDictionary
{
    public class DictionaryImageViewModel : MvxViewModel
    {
        public Task<List<DBStatistics>> DatabaseFlag { get; private set; }
        public Task<List<DBStatistics>> DatabaseFilms { get; private set; }

        public override Task Initialize()
        {
            DatabaseFlag = DBStatistics.GetImages($"{TableNamesImage.Flags}");
            DatabaseFilms = DBStatistics.GetImages($"{TableNamesImage.Films}");
            return Task.FromResult(true);
        }
    }
}
