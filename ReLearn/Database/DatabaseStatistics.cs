using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReLearn
{
    public class DBStatistics // Класс для считывания базы данных Stats
    {
        public int NumberLearn { get; set; }
        public DateTime DateRecurrence { get; set; }

        public DBStatistics() => DateRecurrence = DateTime.Today;

        public static void Insert(int True, int False, string TableName)      // add stats to the database
        {
            var database = DataBase.Connect(Database_Name.Statistics);
            var query = $"INSERT INTO {TableName}_Statistics (True, False, DateOfTesting) VALUES (?, ?, ?)";
            database.Execute(query, True, False, DateTime.Now);
        }
    }
}