using System;
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

    public enum TableNamesLanguage
    {
        My_Directly,
        Home,
        Education,
        Popular_Words,
        ThreeFormsOfVerb
    }

    enum Language
    {
        en,
        ru
    }

    public enum TableNamesImage
    {
        Flags
    }

    public static class DataBase
    {
        public static TableNamesLanguage TableNameLanguage
        {
            get
            {
                if (String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault(Settings.DictionaryNameLanguages.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(Settings.DictionaryNameLanguages.ToString(), TableNamesLanguage.Popular_Words.ToString());
                Enum.TryParse(CrossSettings.Current.GetValueOrDefault(Settings.DictionaryNameLanguages.ToString(), null), out TableNamesLanguage name);
                return name;
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(Settings.DictionaryNameLanguages.ToString(), value.ToString());
            }
        }
        public static TableNamesImage TableNameImage
        {
            get
            {
                if (String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault(Settings.DictionaryNameImage.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(Settings.DictionaryNameImage.ToString(), TableNamesImage.Flags.ToString());
                    Enum.TryParse(CrossSettings.Current.GetValueOrDefault(Settings.DictionaryNameImage.ToString(), null), out TableNamesImage name);
                return name;
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(Settings.DictionaryNameImage.ToString(), value.ToString());
            }
        }

        public static SQLiteConnection Connect(string nameDB)
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), nameDB);
            return new SQLiteConnection(databasePath);
        }

        public static void СreateNewTableToLanguagesDataBase()
        {
            var db = Connect(Database_Name.English_DB);
            foreach (string tableName in Enum.GetNames(typeof(TableNamesLanguage)))
            {
                if (db.GetTableInfo(tableName).Count == 0)
                {
                    db.Query<Database_Words>($"CREATE TABLE {tableName} (_id int PRIMARY KEY, Word string, TranslationWord string, NumberLearn int, DateRecurrence DateTime, Context string, Image string)");
                    using (StreamReader reader = new StreamReader(Application.Context.Assets.Open($"Database/{tableName}.txt")))
                        while (reader.Peek() >= 0)
                        {
                            string str_line = reader.ReadLine();
                            var list_en_ru = str_line.Split('|');

                            var query = $"INSERT INTO {tableName} (Word, TranslationWord, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?)";
                            db.Execute(query, list_en_ru[0].ToLower(), list_en_ru[1].ToLower(), Magic_constants.StandardNumberOfRepeats, DateTime.Now);                            
                        }

                    var dbStatEn = Connect(Database_Name.Statistics);
                    dbStatEn.Query<Database_Statistics>($"CREATE TABLE {tableName}_Statistics (_id int PRIMARY KEY, True int, False int, DateOfTesting DateTime)");
                }
            }
        }

        public static void InstallDatabaseFromAssets(string FileName)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, FileName);
            if (!File.Exists(path))
            {
                Context context = Application.Context;
                using (var dbAssetStream = context.Assets.Open($"Database/{FileName}"))
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
        //db.Query<Database_Words>("UPDATE " + TableNameLanguage + " SET DateRecurrence = DATETIME('NOW') WHERE Word = ?", s.Word);
        //db.Query<Database_Words>("UPDATE " + TableNameLanguage + " SET NumberLearn = " + s.NumberLearn + 1 + " WHERE Word = ?", s.Word);
        public static void UpdateWordsToRepeat()
        {
            var toDay = DateTime.Today.AddMonths(-1);
            var db = Connect(Database_Name.English_DB);
            var dataBase = db.Query<Database_Words>($"SELECT * FROM {TableNameLanguage} WHERE NumberLearn = 0 ");
            foreach (var s in dataBase)
            {
                if (s.DateRecurrence < toDay)
                {
                    var query = $"UPDATE {TableNameLanguage} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?";
                    db.Execute(query, DateTime.Now, s.NumberLearn + 1, s.Word);
                }                       
            }
        }

        public static void UpdateImagesToRepeat()
        {
            var toDay = DateTime.Today.AddMonths(-1);
            var db = Connect(Database_Name.Flags_DB); 
            var dataBase = db.Query<Database_images>($"SELECT * FROM {TableNameImage} WHERE NumberLearn = 0");
            foreach (var s in dataBase)
            {
                if (s.DateRecurrence < toDay)
                {
                    var query = $"UPDATE {TableNameImage} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?";
                    db.Execute(query, DateTime.Now, s.NumberLearn + 1, s.Image_name);
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
    
    public class Database_for_stats // Класс для считывания базы данных Stats
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

