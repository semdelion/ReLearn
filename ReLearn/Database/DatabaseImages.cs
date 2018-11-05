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
        
        public DBImages Add(string image_n, string flag_en, string flag_ru, int nLearn, DateTime date)
        {
            Image_name = image_n;
            Name_image_en = flag_en;
            Name_image_ru = flag_ru;
            NumberLearn = nLearn;
            DateRecurrence = date;
            return this;
        }

        public static void  Update(List<Statistics> Stats) // изменение у BD элемента NumberLearn
        {
            var database = DataBase.Connect(Database_Name.Flags_DB);
            for (int i = 0; i < Stats.Count; i++)
            {
                var query = $"UPDATE {DataBase.TableNameImage} SET DateRecurrence = ?, NumberLearn = ? WHERE Image_name = ?";
                database.Execute(query, DateTime.Now, Stats[i].Learn, Stats[i].Word);
            }
        }
    }
}