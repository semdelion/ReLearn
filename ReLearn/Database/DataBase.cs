﻿using System;
using System.IO;
using Android.App;
using Android.Content;
using SQLite;
using Plugin.Settings;

namespace ReLearn
{
    public static class Database_Name
    {
        public static string Statistics { get => "database_statistics.db3"; }
        public static string English_DB { get => "database_words.db3"; }
        public static string Flags_DB { get => "database_image.db3"; }
    }

    public enum TableNames
    {
        My_Directly,
        Home,
        Education,
        Popular_Words,
        Flags,
    }
    
    public static class DataBase
    {
        public static string TableNameLanguage
        {
            get
            {
                if (String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault(Settings.DictionaryName.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(Settings.DictionaryName.ToString(), TableNames.Popular_Words.ToString());
                return CrossSettings.Current.GetValueOrDefault(Settings.DictionaryName.ToString(), null);
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(Settings.DictionaryName.ToString(), value);
            }
        }
        public static string TableNameImage
        {
            get
            {
                if (String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault(Settings.DictionaryNameImage.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(Settings.DictionaryNameImage.ToString(), TableNames.Flags.ToString());
                return CrossSettings.Current.GetValueOrDefault(Settings.DictionaryNameImage.ToString(), null);
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(Settings.DictionaryNameImage.ToString(), value);
            }
        }

        public static SQLiteConnection Connect(string nameDB)
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), nameDB);
            return new SQLiteConnection(databasePath);
        }

        public static void Install_database_from_assets(string sqliteFilename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            if (!File.Exists(path))
            {
                Context context = Application.Context;
                using (var dbAssetStream = context.Assets.Open("Database/" + sqliteFilename))
                {
                    using (var dbFileStream = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        var buffer = new byte[1024];
                        int length;
                        while ((length = dbAssetStream.Read(buffer, 0, buffer.Length)) > 0)
                            dbFileStream.Write(buffer, 0, length);
                        dbFileStream.Flush();
                    }
                }
            }          
        }

        public static void UpdateWordsToRepeat()
        {
            var toDay = DateTime.Today;
            var db = Connect(Database_Name.English_DB);
            var dataBase = db.Query<Database_Words>("SELECT * FROM " + TableNameLanguage + " WHERE NumberLearn = 0 AND DateRecurrence != DATETIME('NOW')");
            foreach (var s in dataBase) // UPDATE Database
            {
                if (s.DateRecurrence.Month != toDay.Month && toDay.Day >= s.DateRecurrence.Day)
                {
                    // обновление БД, при условии, что месяцы не совпадают и NumberLearn == 0. изменяем месяц на текущий и NumberLearn++;                
                    db.Query<Database_Words>("UPDATE " + TableNameLanguage + " SET DateRecurrence = DATETIME('NOW') WHERE Word = ?", s.Word);
                    db.Query<Database_Words>("UPDATE " + TableNameLanguage + " SET NumberLearn = " + s.NumberLearn + 1 + " WHERE Word = ?", s.Word);
                }                       
            }
        }

        public static void UpdateImagesToRepeat()
        {
            var toDay = DateTime.Today;
            var db = Connect(Database_Name.Flags_DB); 
            var dataBase = db.Query<Database_images>("SELECT * FROM " + TableNameImage + " WHERE NumberLearn = 0 AND DateRecurrence != DATETIME('NOW')");

            foreach (var s in dataBase) // UPDATE Flags
            {
                if (s.DateRecurrence.Month != toDay.Month && toDay.Day >= s.DateRecurrence.Day)
                {
                    db.Query<Database_images>("UPDATE " + TableNameImage + " SET DateRecurrence = DATETIME('NOW') WHERE Image_name = ?", s.Image_name);
                    db.Query<Database_images>("UPDATE " + TableNameImage + " SET NumberLearn = " + s.NumberLearn + 1 + " WHERE Image_name = ?", s.Image_name);
                }
            }
        }    
    }

    public class Database_Words //Класс для считывания базы данных English
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Word { get; set; }
        public string TranslationWord { get; set; }
        public int NumberLearn { get; set; }
        public DateTime DateRecurrence { get; set; }

        public Database_Words()
        {
            this.DateRecurrence = DateTime.Today;
            this.Word = "";
            this.NumberLearn = Magic_constants.StandardNumberOfRepeats;
            this.TranslationWord = "";
        }

        public Database_Words(Database_Words x)
        {
            this.DateRecurrence = x.DateRecurrence;
            this.Word = x.Word;
            this.NumberLearn = x.NumberLearn;
            this.TranslationWord = x.TranslationWord;
        }

        public Database_Words Find() => this;
    }

    public class Database_images // Класс для считывания базы данных flags
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Image_name { get; set; }
        public string Name_image_en { get; set; }
        public string Name_image_ru { get; set; }
        public int NumberLearn { get; set; }
        public DateTime DateRecurrence { get; set; }

        public Database_images()
        {
            Image_name = null;
            Name_image_en = null;
            Name_image_ru = null;
            NumberLearn = 0;
            DateRecurrence = DateTime.Today;
        }

        public Database_images Add(string image_n, string flag_en, string flag_ru, int nLearn, DateTime date)
        {
            this.Image_name = image_n;
            this.Name_image_en = flag_en;
            this.Name_image_ru = flag_ru;
            this.NumberLearn = nLearn;
            this.DateRecurrence = date;
            return this;
        }
    }

    public class Database_for_stats // Класс для считывания базы данных flags
    {
        public int NumberLearn { get; set; }
        public DateTime DateRecurrence { get; set; }

        public Database_for_stats()
        {
            NumberLearn = 0;
            DateRecurrence = DateTime.Today;
        }       
    }
}
