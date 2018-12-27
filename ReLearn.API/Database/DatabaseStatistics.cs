using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace ReLearn.API.Database
{
    public class DatabaseStatistics    // class for reading databse
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public int True { get; set; }
        public int False { get; set; }
        public DateTime DateOfTesting { get; set; }
    }

    public class DBStatistics          // Класс для считывания базы данных Stats
    {
        public int NumberLearn { get; set; }
        public DateTime DateRecurrence { get; set; }

        public DBStatistics() => DateRecurrence = DateTime.Today;

        public static void Insert(int True, int False, string TableName) => 
            DataBase.Statistics.Execute($"INSERT INTO {TableName}_Statistics (True, False, DateOfTesting) VALUES (?, ?, ?)", True, False, DateTime.Now);

        public static List<DBStatistics> GetImages(string TableName) => DataBase.Images.Query<DBStatistics>($"SELECT NumberLearn, DateRecurrence FROM {TableName}");

        public static List<DBStatistics> GetWords(string TableName) => DataBase.Languages.Query<DBStatistics>($"SELECT NumberLearn, DateRecurrence FROM {TableName}");

        public static List<DatabaseStatistics> GetData(string TableName) => DataBase.Statistics.Query<DatabaseStatistics>($"SELECT * FROM {TableName}_Statistics");

        public static float AverageTrueToday(string TableName) =>
             AveragePercentTrue(DataBase.Statistics.Query<DatabaseStatistics>($"SELECT * FROM {TableName}_Statistics WHERE DateOfTesting >= ?", DateTime.Today.AddDays(-1)));
        
        public static float AverageTrueMonth(string TableName) => 
            AveragePercentTrue(DataBase.Statistics.Query<DatabaseStatistics>($"SELECT * FROM {TableName}_Statistics WHERE  DateOfTesting >= ?", DateTime.Today.AddMonths(-1)));
        
        public static float AveragePercentTrue(List<DatabaseStatistics> Database_Stat) =>
            Database_Stat.Count == 0 ? 0 : (Database_Stat.Sum(r => r.True) * (100f / (Database_Stat.Sum(r => r.True) + Database_Stat.Sum(r => r.False))));
    }
}