using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.MainMenu.Statistics
{
    public class GeneralStatisticsViewModel : MvxViewModel
    {
        private float _avgTrueToday { get; set; }
        private float _avgTrueMonth { get; set; }
        private float _avgTrue { get; set; }

        public List<DatabaseStatistics> Database { get; private set; }
        public List<DBStatistics> DatabaseStats { get; private set; }

        public string DegreeOfStudy => AppResources.GeneralStatisticsViewModel_DegreeOfStudy;

        public string NumberLearnedCount => $"{DatabaseStats.Count(r => r.NumberLearn == 0)} " +
            $"{AppResources.GeneralStatisticsViewModel_Of} {DatabaseStats.Count}";
        public string NumberLearned
        {
            get
            {
                if ($"{DataBase.TableName}" == $"{TableNamesImage.Flags}")
                    return AppResources.GeneralStatisticsViewModel_NumberLearnedFlags;
                if ($"{DataBase.TableName}" == $"{TableNamesImage.Films}")
                    return AppResources.GeneralStatisticsViewModel_NumberLearnedFilms;
                else
                    return AppResources.GeneralStatisticsViewModel_NumberLearnedWords;
            }
        }
        public string NumberInconvenientCount =>
            $"{DatabaseStats.Count(r => r.NumberLearn == API.Settings.MaxNumberOfRepeats)} " +
            $"{AppResources.GeneralStatisticsViewModel_Of} {DatabaseStats.Count}";

        public string NumberInconvenient
        {
            get
            {
                if ($"{DataBase.TableName}" == $"{TableNamesImage.Flags}")
                    return AppResources.GeneralStatisticsViewModel_NumberInconvenientFlags;
                if ($"{DataBase.TableName}" == $"{TableNamesImage.Films}")
                    return AppResources.GeneralStatisticsViewModel_NumberInconvenientFilms;
                else
                    return AppResources.GeneralStatisticsViewModel_NumberInconvenientWords;
            }
        }

        public string NumberCorrectAnswers =>
            AppResources.GeneralStatisticsViewModel_NumberCorrectAnswers;

        public string NumberCorrectAndIncorrect =>
            $"{AppResources.GeneralStatisticsViewModel_Correct} {Database.Sum(r => r.True)}, " +
            $"{AppResources.GeneralStatisticsViewModel_Incorrect} {Database.Sum(r => r.False)}, " +
            $"{AppResources.GeneralStatisticsViewModel_NumberOfTests} {Database.Count}";

        public string CorrectAnswers    => $"{AppResources.GeneralStatisticsViewModel_CorrectAnswers}";

        public string Today             => $"{AppResources.GeneralStatisticsViewModel_Today}";
        public string TodayPercent      => $"{Math.Round(_avgTrueToday, 1)}%";
        public string TodayPercentAbove => _avgTrueToday - _avgTrue > 0 ? $"+{Math.Round(_avgTrueToday - _avgTrue, 1)}%" : "";
        public string TodayPercentBelow => _avgTrueToday - _avgTrue < 0 ? $"{Math.Round(_avgTrueToday - _avgTrue, 1)}%" : "";

        public string Month             => $"{AppResources.GeneralStatisticsViewModel_Month}";
        public string MonthPercent      => $"{Math.Round(_avgTrueMonth, 1)}%";
        public string MonthPercentAbove => _avgTrueMonth - _avgTrue > 0 ? $"+{Math.Round(_avgTrueMonth - _avgTrue, 1)}%" : "";
        public string MonthPercentBelow => _avgTrueMonth - _avgTrue < 0 ? $"{Math.Round(_avgTrueMonth - _avgTrue, 1)}%" : "";

        public string Average           => $"{AppResources.GeneralStatisticsViewModel_Average}";
        public string AveragePercent    => $"{Math.Round(_avgTrue, 1)}%";

        public override async Task Initialize()
        {
            Database = await DBStatistics.GetData($"{DataBase.TableName}");
            bool isContain = DBImages.DatabaseIsContain($"{DataBase.TableName}");
            DatabaseStats = isContain ? await DBStatistics.GetImages($"{DataBase.TableName}") :
                                     await DBStatistics.GetWords($"{DataBase.TableName}");
            _avgTrueToday = await DBStatistics.AverageTrueToday($"{DataBase.TableName}");
            _avgTrueMonth = await DBStatistics.AverageTrueMonth($"{DataBase.TableName}");
            _avgTrue = await DBStatistics.AveragePercentTrue(Database);
        }
    }
}
