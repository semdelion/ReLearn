using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Facade;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu.SelectDictionary
{
    public class DictionaryImageViewModel : MvxDictionaryViewModel
    {
        #region Properties
        public string TextImages => this["Texts.Images"];
        public string TextStudiedAt => this["Texts.StudiedAt"];

        public List<DBStatistics> DatabaseFlag { get; private set; }
        public List<DBStatistics> DatabaseFilms { get; private set; }
        #endregion

        #region Public
        public DictionaryImageViewModel()
        {
            Task.Run(async () =>
            {
                DatabaseFlag = await DBStatistics.GetImages($"{TableNamesImage.Flags}");
                DatabaseFilms = await DBStatistics.GetImages($"{TableNamesImage.Films}");
            }).Wait();
        }
        #endregion
    }
}