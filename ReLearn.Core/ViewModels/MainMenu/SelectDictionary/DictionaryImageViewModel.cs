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
        public List<DBStatistics> DatabaseFlag { get; private set; }
        public List<DBStatistics> DatabaseFilms { get; private set; }

        public DictionaryImageViewModel()
        {
            Task.Run(async () => 
            {
                DatabaseFlag = await DBStatistics.GetImages($"{TableNamesImage.Flags}");
                DatabaseFilms = await DBStatistics.GetImages($"{TableNamesImage.Films}");
            }).Wait();
        }
    }
}
