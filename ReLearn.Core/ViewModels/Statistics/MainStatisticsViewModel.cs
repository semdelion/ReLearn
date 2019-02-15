using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReLearn.Core.ViewModels.Statistics
{
    public class MainStatisticsViewModel : MvxViewModel
    {
        public List<DatabaseStatistics> Database { get; private set; }
        public int? True { get; private set; } = null;
        public int? False { get; private set; } = null;

        public MainStatisticsViewModel()
        {
            Database = DBStatistics.GetData(DataBase.TableName.ToString());
            if (Database.Count != 0)
            {
                True = Database[Database.Count - 1].True;
                False = Database[Database.Count - 1].False;
            }
        }


    }
}
