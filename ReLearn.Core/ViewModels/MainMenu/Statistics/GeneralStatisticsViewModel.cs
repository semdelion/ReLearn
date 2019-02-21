using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.Localization;
using System.Collections.Generic;
using System.Linq;

namespace ReLearn.Core.ViewModels.MainMenu.Statistics
{
    public class GeneralStatisticsViewModel : MvxViewModel
    {
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

        public GeneralStatisticsViewModel()
        {
            Database = DBStatistics.GetData($"{DataBase.TableName}");
            bool isContain = DBImages.DatabaseIsContain($"{DataBase.TableName}");
            DatabaseStats = isContain ? DBStatistics.GetImages($"{DataBase.TableName}") :
                                      DBStatistics.GetWords($"{DataBase.TableName}");
        }
    }
}
