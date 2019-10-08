using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu.SelectDictionary
{
    public class DictionaryImageViewModel : BaseViewModel
    {
        public DictionaryImageViewModel()
        {
            Task.Run(async () =>
            {
                DatabaseFlag = await DBStatistics.GetImages($"{TableNamesImage.Flags}");
                DatabaseFilms = await DBStatistics.GetImages($"{TableNamesImage.Films}");
            }).Wait();
        }

        public List<DBStatistics> DatabaseFlag { get; private set; }
        public List<DBStatistics> DatabaseFilms { get; private set; }
    }
}