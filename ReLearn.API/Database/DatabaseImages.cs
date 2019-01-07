using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace ReLearn.API.Database
{
    public enum TableNamesImage
    {
        Flags,
        Films
    }

    public class DBImages// Класс для считывания базы данных flags
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Image_name { get; set; }
        public string Name_image_en { get; set; }
        public string Name_image_ru { get; set; }
        public int NumberLearn { get; set; }
        public DateTime DateRecurrence { get; set; }

        public DBImages() => DateRecurrence = DateTime.Today;

        public DBImages Find() => this;

        public static void Update(string Image, int learn) // изменение у BD элемента NumberLearn
        {
                 DataBase.Images.Execute(
                    $"UPDATE {DataBase.TableNameImage} SET DateRecurrence = ?, NumberLearn = ? WHERE Image_name = ?", 
                    DateTime.Now, learn, Image);
        }

        public static bool DatabaseIsContain(string nameDB)
        {
            Enum.TryParse(nameDB, out TableNamesImage name);
            if (nameDB != name.ToString())
                return false;
            return true;
        }

        public static void UpdateData()
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
        //            DataBase.Images.Execute($"CREATE TABLE {tableName} (_id int PRIMARY KEY, Image_name string, Name_image_en string, Name_image_ru string, NumberLearn int, DateRecurrence DateTime)");
        //            using (StreamReader reader = new StreamReader(Application.Context.Assets.Open($"Database/{tableName}.txt")))
        //            {
        //                string str_line;
        //                while ((str_line = reader.ReadLine()) != null)
        //                {
        //                    var image = str_line.Split('|');
        //                    var query = $"INSERT INTO {tableName} (Image_name, Name_image_en, Name_image_ru, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?, ?)";
        //                    DataBase.Images.Execute(query, image[0], image[1], image[2], Settings.StandardNumberOfRepeats, DateTime.Now);
        //                }
        //            }
        //            DataBase.Statistics.Execute($"CREATE TABLE {tableName}_Statistics (_id int PRIMARY KEY, True int, False int, DateOfTesting DateTime)");
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

        public string ImageName { get => Settings.Currentlanguage == Language.en.ToString() ? Name_image_en : Name_image_ru; }
    }
}