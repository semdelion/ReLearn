using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLearn.API.Database
{
    public class DatabaseStatistics
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public int True { get; set; }
        public int False { get; set; }
        public DateTime DateOfTesting { get; set; }
    }

    public class DBStatistics
    {
        public int NumberLearn { get; set; }
        public DateTime DateRecurrence { get; set; }

        public DBStatistics() => DateRecurrence = DateTime.Today;

        public static async Task Insert(int True, int False, string TableName) =>
            await DataBase.Statistics.ExecuteAsync(
                $"INSERT INTO {TableName}_Statistics (True, False, DateOfTesting) VALUES (?, ?, ?)", True, False, DateTime.Now);

        public static async Task<List<DBStatistics>> GetImages(string TableName) => 
            await DataBase.Images.QueryAsync<DBStatistics>($"SELECT NumberLearn, DateRecurrence FROM {TableName}");

        public static async Task<List<DBStatistics>> GetWords(string TableName) => 
            await DataBase.Languages.QueryAsync<DBStatistics>($"SELECT NumberLearn, DateRecurrence FROM {TableName}");

        public static async Task<List<DatabaseStatistics>> GetData(string TableName) => 
            await DataBase.Statistics.QueryAsync<DatabaseStatistics>($"SELECT * FROM {TableName}_Statistics");

        public static async Task<float> AverageTrueToday(string tableName) =>
             await AveragePercentTrue(await DataBase.Statistics.QueryAsync<DatabaseStatistics>(
                 $"SELECT * FROM {tableName}_Statistics WHERE DateOfTesting >= ?", DateTime.Today.AddDays(-1)));
        
        public static async Task<float> AverageTrueMonth(string tableName) =>
           await AveragePercentTrue(await DataBase.Statistics.QueryAsync<DatabaseStatistics>(
               $"SELECT * FROM {tableName}_Statistics WHERE  DateOfTesting >= ?", DateTime.Today.AddMonths(-1)));

        public static async Task<float> AveragePercentTrue(List<DatabaseStatistics> database) => 
            await Task.Run(() => database.Count == 0 ? 0 : (database.Sum(r => r.True) * (100f / (database.Sum(r => r.True) + database.Sum(r => r.False)))));
    }
}