using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using ReLearn.API.Database;

namespace ReLearn.Core.ViewModels.MainMenu.SelectDictionary
{
    public class DictionaryLanguageViewModel : MvxViewModel
    {
        public DictionaryLanguageViewModel()
        {
            Task.Run(async () =>
            {
                Home = await DBStatistics.GetWords($"{TableNamesLanguage.Home}");
                Education = await DBStatistics.GetWords($"{TableNamesLanguage.Education}");
                PopularWords = await DBStatistics.GetWords($"{TableNamesLanguage.Popular_Words}");
                ThreeFormsOfVerb = await DBStatistics.GetWords($"{TableNamesLanguage.ThreeFormsOfVerb}");
                ComputerScience = await DBStatistics.GetWords($"{TableNamesLanguage.ComputerScience}");
                Nature = await DBStatistics.GetWords($"{TableNamesLanguage.Nature}");
                MyDirectly = await DBStatistics.GetWords($"{TableNamesLanguage.My_Directly}");
            }).Wait();
        }

        public List<DBStatistics> Home { get; private set; }
        public List<DBStatistics> Education { get; private set; }
        public List<DBStatistics> PopularWords { get; private set; }
        public List<DBStatistics> ThreeFormsOfVerb { get; private set; }
        public List<DBStatistics> ComputerScience { get; private set; }
        public List<DBStatistics> Nature { get; private set; }
        public List<DBStatistics> MyDirectly { get; private set; }
    }
}