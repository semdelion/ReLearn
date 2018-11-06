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

        public static void Update(List<Statistics> Stats) // изменение у BD элемента NumberLearn
        {
            for (int i = 0; i < Stats.Count; i++)
                DataBase.Images.Execute(
                    $"UPDATE {DataBase.TableNameImage} SET DateRecurrence = ?, NumberLearn = ? WHERE Image_name = ?", 
                    DateTime.Now, Stats[i].Learn, Stats[i].Word);
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