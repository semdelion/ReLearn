using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu.Statistics
{
    public class MainStatisticsViewModel : MvxViewModel
    {
        private int _amountOfStatistics = API.Settings.AmountOfStatistics;
        public int AmountOfStatistics
        {
            get => _amountOfStatistics;
            set => _amountOfStatistics = API.Settings.AmountOfStatistics = value;
        }

        public List<DatabaseStatistics> Database { get; private set; }
        public int? True { get; private set; }
        public int? False { get; private set; }

        public string Answers => $"{AppResources.MainStatisticsViewModel_Correct}:" +
            $" { True ?? 0 }, {AppResources.MainStatisticsViewModel_Incorrect}: { False ?? 0}";
        public string LastTest => AppResources.MainStatisticsViewModel_LastTest;

        public override async Task Initialize()
        {
            Database = await DBStatistics.GetData($"{DataBase.TableName}");
            if (Database.Count != 0)
            {
                True = Database[Database.Count - 1].True;
                False = Database[Database.Count - 1].False;
            }
        }
    }
}
