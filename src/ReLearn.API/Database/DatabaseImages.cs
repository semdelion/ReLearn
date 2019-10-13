using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReLearn.API.Database.Interface;
using SQLite;

namespace ReLearn.API.Database
{
    public enum TableNamesImage
    {
        Flags,
        Films
    }

    public class DatabaseImages : IDatabase
    {
        public DatabaseImages()
        {
            DateRecurrence = DateTime.Today;
        }

        [PrimaryKey]
        [AutoIncrement]
        [Column("_id")]
        public int Id { get; set; }

        public string Image_name { get; set; }
        public string Name_image_en { get; set; }
        public string Name_image_ru { get; set; }
        public int NumberLearn { get; set; }
        public DateTime DateRecurrence { get; set; }

        public string ImageName => Settings.Currentlanguage == Language.en ? Name_image_en : Name_image_ru;

        public DatabaseImages Find()
        {
            return this;
        }

        public static async Task Update(string image, int learn)
        {
            await DataBase.Images.ExecuteAsync(
                $"UPDATE {DataBase.TableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Image_name = ?",
                DateTime.Now, learn, image);
        }

        public static bool DatabaseIsContain(string nameDB)
        {
            Enum.TryParse(nameDB, out TableNamesImage name);
            if (nameDB != $"{name}")
                return false;
            return true;
        }

        public static async Task UpdateData()
        {
            var toDay = DateTime.Today.AddMonths(-1);
            foreach (var tableName in Enum.GetNames(typeof(TableNamesImage)))
            {
                var dataBase =
                    await DataBase.Images.QueryAsync<DatabaseImages>(
                        $"SELECT * FROM {tableName} WHERE NumberLearn = 0");
                foreach (var s in dataBase)
                    if (s.DateRecurrence < toDay)
                    {
                        var query = $"UPDATE {tableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?";
                        await DataBase.Images.ExecuteAsync(query, DateTime.Now, s.NumberLearn + 1, s.Image_name);
                    }
            }
        }

        public static async Task UpdateLearningNotRepeat(string imageName)
        {
            var query = $"UPDATE {DataBase.TableName} SET DateRecurrence = ?, NumberLearn = ? WHERE ";
            var tmp = Settings.Currentlanguage == Language.en ? "Name_image_en = ?" : "Name_image_ru = ?";
            await DataBase.Images.ExecuteAsync(query + tmp, DateTime.Now, 0, imageName);
        }

        public static async Task<List<DatabaseImages>> GetDataNotLearned()
        {
            return await DataBase.Images.QueryAsync<DatabaseImages>(
                $"SELECT * FROM {DataBase.TableName} WHERE NumberLearn != 0 ORDER BY DateRecurrence ASC");
        }

        public static async Task UpdateLearningNext(string imageName)
        {
            await DataBase.Images.ExecuteAsync(
                $"UPDATE {DataBase.TableName} SET DateRecurrence = ? WHERE Image_name = ?",
                DateTime.Now, imageName);
        }

        public static async Task<List<DatabaseImages>> GetData()
        {
            return await DataBase.Images.QueryAsync<DatabaseImages>($"SELECT * FROM {DataBase.TableName}");
        }
    }
}