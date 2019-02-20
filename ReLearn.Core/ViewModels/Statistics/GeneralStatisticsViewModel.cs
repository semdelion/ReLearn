using MvvmCross.ViewModels;
using ReLearn.API.Database;
using System.Collections.Generic;

namespace ReLearn.Core.ViewModels.Statistics
{
    public class GeneralStatisticsViewModel: MvxViewModel
    {
        public List<DatabaseStatistics> Database { get; private set; }
        public List<DBStatistics> DatabaseStats { get; private set; }

        public GeneralStatisticsViewModel()
        {
            Database = DBStatistics.GetData($"{DataBase.TableName}");
            bool isContain = DBImages.DatabaseIsContain($"{DataBase.TableName}");
            DatabaseStats = isContain ? DBStatistics.GetImages($"{DataBase.TableName}") :
                                      DBStatistics.GetWords($"{DataBase.TableName}");
        }
    }
}
