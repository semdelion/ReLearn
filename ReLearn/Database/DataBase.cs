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

    enum Pronunciation
    {
        en,
        uk
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
                if (String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault(DBSettings.DictionaryNameLanguages.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(DBSettings.DictionaryNameLanguages.ToString(), TableNamesLanguage.Popular_Words.ToString());
                    Enum.TryParse(CrossSettings.Current.GetValueOrDefault(DBSettings.DictionaryNameLanguages.ToString(), null), out TableNamesLanguage name);
                return name;
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(DBSettings.DictionaryNameLanguages.ToString(), value.ToString());
            }
        }

        public static TableNamesImage TableNameImage
        {
            get
            {
                if (String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault(DBSettings.DictionaryNameImage.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(DBSettings.DictionaryNameImage.ToString(), TableNamesImage.Flags.ToString());
                Enum.TryParse(CrossSettings.Current.GetValueOrDefault(DBSettings.DictionaryNameImage.ToString(), null), out TableNamesImage name);
                return name;
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(DBSettings.DictionaryNameImage.ToString(), value.ToString());
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
                    db.Query<DBWords>($"CREATE TABLE {tableName} (_id int PRIMARY KEY, Word string, TranslationWord string, NumberLearn int, DateRecurrence DateTime, Context string, Image string)");
                    using (StreamReader reader = new StreamReader(Application.Context.Assets.Open($"Database/{tableName}.txt")))
                    {
                        string str_line;
                        while ((str_line = reader.ReadLine()) != null)
                        {
                            var list_en_ru = str_line.Split('|');
                            var query = $"INSERT INTO {tableName} (Word, TranslationWord, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?)";
                            db.Execute(query, list_en_ru[0].ToLower(), list_en_ru[1].ToLower(), Settings.StandardNumberOfRepeats, DateTime.Now);
                        }
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
        
        public static void UpdateWordsToRepeat()
        {
            var toDay = DateTime.Today.AddMonths(-1);
            var db = Connect(Database_Name.English_DB);
            var dataBase = db.Query<DBWords>($"SELECT * FROM {TableNameLanguage} WHERE NumberLearn = 0 ");
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
            var dataBase = db.Query<DBImages>($"SELECT * FROM {TableNameImage} WHERE NumberLearn = 0");
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
}