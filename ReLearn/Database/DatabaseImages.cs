using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace ReLearn
{
    enum TableNamesImage
    {
        Flags
    }

    public class DBImages // Класс для считывания базы данных flags
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Image_name { get; set; }
        public string Name_image_en { get; set; }
        public string Name_image_ru { get; set; }
        public int NumberLearn { get; set; }
        public DateTime DateRecurrence { get; set; }

        public DBImages() => DateRecurrence = DateTime.Today;

        public static void Update(string Image, int learn) // изменение у BD элемента NumberLearn
        {
                DataBase.Images.Execute(
                    $"UPDATE {DataBase.TableNameImage} SET DateRecurrence = ?, NumberLearn = ? WHERE Image_name = ?", 
                    DateTime.Now, learn, Image);
        }

        public static void UpdateDate()
        {
            var toDay = DateTime.Today.AddMonths(-1);
            foreach (string tableName in Enum.GetNames(typeof(TableNamesImage)))
            {
                var dataBase = DataBase.Images.Query<DBImages>($"SELECT * FROM {tableName} WHERE NumberLearn = 0");
                foreach (var s in dataBase)
                    if (s.DateRecurrence < toDay)
                    {
                        var query = $"UPDATE {tableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?";
                        DataBase.Images.Execute(query, DateTime.Now, s.NumberLearn + 1, s.Image_name);
                    }
            }
        }

        //public static void СreateTable()
        //{
        //    foreach (string tableName in Enum.GetNames(typeof(TableNamesImage)))
        //    {
        //        if (DataBase.Images.GetTableInfo(tableName).Count == 0)
        //        {
        //            DataBase.Images.Query<DBImages>($"CREATE TABLE {tableName} (_id int PRIMARY KEY, Word string, TranslationWord string, NumberLearn int, DateRecurrence DateTime, Context string, Image string)");
        //            using (StreamReader reader = new StreamReader(Application.Context.Assets.Open($"Database/{tableName}.txt")))
        //            {
        //                string str_line;
        //                while ((str_line = reader.ReadLine()) != null)
        //                {
        //                    var list_en_ru = str_line.Split('|');
        //                    var query = $"INSERT INTO {tableName} (Word, TranslationWord, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?)";
        //                    DataBase.Images.Execute(query, list_en_ru[0].ToLower(), list_en_ru[1].ToLower(), Settings.StandardNumberOfRepeats, DateTime.Now);
        //                }
        //            }
        //            DataBase.Statistics.Query<DatabaseStatistics>($"CREATE TABLE {tableName}_Statistics (_id int PRIMARY KEY, True int, False int, DateOfTesting DateTime)");
        //        }
        //    }
        //}

        public static void UpdateLearningNotRepeat(string ImageName)
        {
            var query = $"UPDATE {DataBase.TableNameImage} SET DateRecurrence = ?, NumberLearn = ? WHERE ";
            var tmp = Settings.Currentlanguage == Language.en.ToString() ? "Name_image_en = ?" : "Name_image_ru = ?";
            DataBase.Images.Execute(query + tmp, DateTime.Now, 0, ImageName);
        }

        public static List<DBImages> GetDataNotLearned => DataBase.Images.Query<DBImages>(
            $"SELECT * FROM {DataBase.TableNameImage} WHERE NumberLearn != 0 ORDER BY DateRecurrence ASC");

        public static void UpdateLearningNext(string ImageName) => DataBase.Images.Execute(
            $"UPDATE {DataBase.TableNameImage} SET DateRecurrence = ? WHERE Image_name = ?", 
            DateTime.Now, ImageName);

        public static List<DBImages> GetData => DataBase.Images.Query<DBImages>($"SELECT * FROM {DataBase.TableNameImage.ToString()}");
    }
}