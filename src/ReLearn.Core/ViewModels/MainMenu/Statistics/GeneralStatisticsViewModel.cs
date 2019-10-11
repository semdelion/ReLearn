using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu.Statistics
{
    public class GeneralStatisticsViewModel : BaseViewModel
    {
        private float _avgTrue;
        private float _avgTrueMonth;
        private float _avgTrueToday;

        public GeneralStatisticsViewModel()
        {
            Task.Run(async () =>
            {
                Database = await DBStatistics.GetData($"{DataBase.TableName}");
                var isContain = DatabaseImages.DatabaseIsContain($"{DataBase.TableName}");
                DatabaseStats = isContain
                    ? await DBStatistics.GetImages($"{DataBase.TableName}")
                    : await DBStatistics.GetWords($"{DataBase.TableName}");
                _avgTrueToday = await DBStatistics.AverageTrueToday($"{DataBase.TableName}");
                _avgTrueMonth = await DBStatistics.AverageTrueMonth($"{DataBase.TableName}");
                _avgTrue = await DBStatistics.AveragePercentTrue(Database);
            }).Wait();
        }

        public List<DatabaseStatistics> Database { get; private set; }
        public List<DBStatistics> DatabaseStats { get; private set; }

        public string DegreeOfStudy => this["DegreeOfStudy"];

        public string NumberLearnedCount => $"{DatabaseStats.Count(r => r.NumberLearn == 0)} " +
                                            $"{this["Of"]} {DatabaseStats.Count}";

        public string NumberLearned
        {
            get
            {
                if ($"{DataBase.TableName}" == $"{TableNamesImage.Flags}")
                    return this["Numbers.Learned.Flags"];
                if ($"{DataBase.TableName}" == $"{TableNamesImage.Films}")
                    return this["Numbers.Learned.Films"];
                return this["Numbers.Learned.Words"];
            }
        }

        public string NumberInconvenientCount =>
            $"{DatabaseStats.Count(r => r.NumberLearn == Settings.MaxNumberOfRepeats)} " +
            $"{this["Of"]} {DatabaseStats.Count}";

        public string NumberInconvenient
        {
            get
            {
                if ($"{DataBase.TableName}" == $"{TableNamesImage.Flags}")
                    return this["Numbers.Inconvenient.Flags"];
                if ($"{DataBase.TableName}" == $"{TableNamesImage.Films}")
                    return this["Numbers.Inconvenient.Films"];
                return this["Numbers.Inconvenient.Words"];
            }
        }

        public string NumberCorrectAnswers =>
            this["Numbers.CorrectAnswers"];

        public string NumberCorrectAndIncorrect =>
            $"{this["Correct"]} {Database.Sum(r => r.True)}, " +
            $"{this["Incorrect"]} {Database.Sum(r => r.False)}, " +
            $"{this["Numbers.OfTests"]} {Database.Count}";

        public string CorrectAnswers => $"{this["CorrectAnswers"]}";

        public string Today => $"{this["Today"]}";
        public string TodayPercent => $"{Math.Round(_avgTrueToday, 1)}%";

        public string TodayPercentAbove =>
            _avgTrueToday - _avgTrue > 0 ? $"+{Math.Round(_avgTrueToday - _avgTrue, 1)}%" : "";

        public string TodayPercentBelow =>
            _avgTrueToday - _avgTrue < 0 ? $"{Math.Round(_avgTrueToday - _avgTrue, 1)}%" : "";

        public string Month => $"{this["Month"]}";
        public string MonthPercent => $"{Math.Round(_avgTrueMonth, 1)}%";

        public string MonthPercentAbove =>
            _avgTrueMonth - _avgTrue > 0 ? $"+{Math.Round(_avgTrueMonth - _avgTrue, 1)}%" : "";

        public string MonthPercentBelow =>
            _avgTrueMonth - _avgTrue < 0 ? $"{Math.Round(_avgTrueMonth - _avgTrue, 1)}%" : "";

        public string Average => $"{this["Average"]}";
        public string AveragePercent => $"{Math.Round(_avgTrue, 1)}%";
    }
}