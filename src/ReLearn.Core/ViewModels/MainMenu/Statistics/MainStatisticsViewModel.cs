using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu.Statistics
{
    public class MainStatisticsViewModel : BaseViewModel
    {
        #region Fields
        private int _amountOfStatistics = Settings.AmountOfStatistics;
        #endregion

        #region Properties
        public string Answers => $"{this["Correct"]}:" +
                         $" {True ?? 0}, {this["Incorrect"]}: {False ?? 0}";

        public string LastTest => this["LastTest"];

        public int? True { get; private set; }
        public int? False { get; private set; }

        public List<DatabaseStatistics> Database { get; private set; }

        public int AmountOfStatistics
        {
            get => _amountOfStatistics;
            set => _amountOfStatistics = Settings.AmountOfStatistics = value;
        }
        #endregion

        #region Public
        public MainStatisticsViewModel()
        {
            Task.Run(async () =>
            {
                Database = await DBStatistics.GetData($"{DataBase.TableName}");
                if (Database.Count != 0)
                {
                    True = Database[Database.Count - 1].True;
                    False = Database[Database.Count - 1].False;
                }
            }).Wait();
        }
        #endregion
    }
}