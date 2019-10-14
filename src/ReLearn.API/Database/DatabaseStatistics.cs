using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLearn.API.Database
{
    public class DatabaseStatistics
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("_id")]
        public int Id { get; set; }

        public int True { get; set; }
        public int False { get; set; }
        public DateTime DateOfTesting { get; set; }
    }

    public class DBStatistics
    {
        public DBStatistics()
        {
            DateRecurrence = DateTime.Today;
        }

        public int NumberLearn { get; set; }
        public DateTime DateRecurrence { get; set; }

        public static async Task Insert(int @true, int @false, string tableName)
        {
            await DataBase.Statistics.ExecuteAsync(
                $"INSERT INTO {tableName}_Statistics (True, False, DateOfTesting) VALUES (?, ?, ?)", @true, @false,
                DateTime.Now);
        }

        public static async Task<List<DBStatistics>> GetImages(string tableName)
        {
            return await DataBase.Images.QueryAsync<DBStatistics>(
                $"SELECT NumberLearn, DateRecurrence FROM {tableName}");
        }

        public static async Task<List<DBStatistics>> GetWords(string tableName)
        {
            return await DataBase.Languages.QueryAsync<DBStatistics>(
                $"SELECT NumberLearn, DateRecurrence FROM {tableName}");
        }

        public static async Task<List<DatabaseStatistics>> GetData(string tableName)
        {
            return await DataBase.Statistics.QueryAsync<DatabaseStatistics>($"SELECT * FROM {tableName}_Statistics");
        }

        public static async Task<float> AverageTrueToday(string tableName)
        {
            return await AveragePercentTrue(await DataBase.Statistics.QueryAsync<DatabaseStatistics>(
                $"SELECT * FROM {tableName}_Statistics WHERE DateOfTesting >= ?", DateTime.Today.AddDays(-1)));
        }

        public static async Task<float> AverageTrueMonth(string tableName)
        {
            return await AveragePercentTrue(await DataBase.Statistics.QueryAsync<DatabaseStatistics>(
                $"SELECT * FROM {tableName}_Statistics WHERE  DateOfTesting >= ?", DateTime.Today.AddMonths(-1)));
        }

        public static async Task<float> AveragePercentTrue(List<DatabaseStatistics> database)
        {
            return await Task.Run(() =>
                database.Count == 0
                    ? 0
                    : database.Sum(r => r.True) * (100f / (database.Sum(r => r.True) + database.Sum(r => r.False))));
        }
    }
}