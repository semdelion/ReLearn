using MvvmCross.ViewModels;
using ReLearn.API.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu.SelectDictionary
{
    public class DictionaryLanguageViewModel : MvxViewModel
    {
        public Task<List<DBStatistics>> Home { get; private set; }
        public Task<List<DBStatistics>> Education { get; private set; }
        public Task<List<DBStatistics>> PopularWords { get; private set; }
        public Task<List<DBStatistics>> ThreeFormsOfVerb { get; private set; }
        public Task<List<DBStatistics>> ComputerScience { get; private set; }
        public Task<List<DBStatistics>> Nature { get; private set; }
        public Task<List<DBStatistics>> MyDirectly { get; private set; }

        public override Task Initialize()
        {
            Home = DBStatistics.GetWords($"{TableNamesLanguage.Home}");
            Education = DBStatistics.GetWords($"{TableNamesLanguage.Education}");
            PopularWords = DBStatistics.GetWords($"{TableNamesLanguage.Popular_Words}");
            ThreeFormsOfVerb = DBStatistics.GetWords($"{TableNamesLanguage.ThreeFormsOfVerb}");
            ComputerScience = DBStatistics.GetWords($"{TableNamesLanguage.ComputerScience}");
            Nature = DBStatistics.GetWords($"{TableNamesLanguage.Nature}");
            MyDirectly = DBStatistics.GetWords($"{TableNamesLanguage.My_Directly}");
            return Task.FromResult(true);
        }
    }
}
