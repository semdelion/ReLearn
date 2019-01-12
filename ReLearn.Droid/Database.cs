using System;
using System.IO;
using Android.App;
using Android.Content;
using SQLite;
using Plugin.Settings;
using ReLearn.API.Database;
using ReLearn.API;

namespace ReLearn.Droid
{
    static class Database
    {
        const string _statistics = "database_statistics.db3";
        const string _english = "database_words.db3";
        const string _flags = "database_image.db3";

        public static void InstallDatabaseFromAssets()
        {
            InstallDB(_statistics);
            InstallDB(_english);
            InstallDB(_flags);
        }

        static void InstallDB(string FileName)
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
        public static void СreateTableLanguage()
        {
            foreach (string tableName in Enum.GetNames(typeof(TableNamesLanguage)))
            {
                if (DataBase.Languages.GetTableInfo(tableName).Count == 0)
                {
                    DataBase.Languages.Execute($"CREATE TABLE {tableName} (_id int PRIMARY KEY, Word string, TranslationWord string, Transcription string, NumberLearn int, DateRecurrence DateTime, Context string, Image string)");
                    using (StreamReader reader = new StreamReader(Application.Context.Assets.Open($"Database/{tableName}.txt")))
                    {
                        string str_line;
                        while ((str_line = reader.ReadLine()) != null)
                        {
                            var list = str_line.Split('|');
                            var query = $"INSERT INTO {tableName} (Word, TranslationWord, Transcription, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?, ?)";
                            DataBase.Languages.Execute(query, list[0].ToLower().Trim(), list[1].ToLower().Trim(), list[2].Trim(), Settings.StandardNumberOfRepeats, DateTime.Now);
                        }
                    }
                    DataBase.Statistics.Execute($"CREATE TABLE {tableName}_Statistics (_id int PRIMARY KEY, True int, False int, DateOfTesting DateTime)");
                }
            }
        }

        public static void СreateTableImage()
        {
            foreach (string tableName in Enum.GetNames(typeof(TableNamesImage)))
            {
                if (DataBase.Images.GetTableInfo(tableName).Count == 0)
                {
                    DataBase.Images.Execute($"CREATE TABLE {tableName} (_id int PRIMARY KEY, Image_name string, Name_image_en string, Name_image_ru string, NumberLearn int, DateRecurrence DateTime)");
                    using (StreamReader reader = new StreamReader(Application.Context.Assets.Open($"Database/{tableName}.txt")))
                    {
                        string str_line;
                        while ((str_line = reader.ReadLine()) != null)
                        {
                            var image = str_line.Split('|');
                            var query = $"INSERT INTO {tableName} (Image_name, Name_image_en, Name_image_ru, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?, ?)";
                            DataBase.Images.Execute(query, image[0], image[1], image[2], Settings.StandardNumberOfRepeats, DateTime.Now);
                        }
                    }
                    DataBase.Statistics.Execute($"CREATE TABLE {tableName}_Statistics (_id int PRIMARY KEY, True int, False int, DateOfTesting DateTime)");
                }
            }
        }
        public static void ADDCOLUMN()
        {
            foreach (string tableName in Enum.GetNames(typeof(TableNamesLanguage)))
            {
                if (!DBWords.ContainColumn("Transcription", DataBase.Languages.GetTableInfo(tableName)))
                {

                    DataBase.Languages.Execute($"ALTER TABLE {tableName} ADD COLUMN Transcription string");
                    if (tableName != TableNamesLanguage.My_Directly.ToString())
                        using (StreamReader reader = new StreamReader(Application.Context.Assets.Open($"Database/{tableName}.txt")))
                        {
                            string str_line;
                            while ((str_line = reader.ReadLine()) != null)
                            {
                                var list = str_line.Split('|');
                                int changes = DataBase.Languages.Execute($"UPDATE {tableName} SET Word = ?, Transcription = ? WHERE Word = ?", list[0].ToLower().Trim(), list[2].Trim(), list[0].ToLower().Trim())
                                            + DataBase.Languages.Execute($"UPDATE {tableName} SET Word = ?, Transcription = ? WHERE Word = ?", list[0].ToLower().Trim(), list[2].Trim(), list[0].ToLower().Trim() + " ")
                                            + DataBase.Languages.Execute($"UPDATE {tableName} SET Word = ?, Transcription = ? WHERE Word = ?", list[0].ToLower().Trim(), list[2].Trim(), list[0].ToLower().Trim() + "  ");
                                if (changes == 0)
                                {
                                    DataBase.Languages.Execute($"INSERT INTO {tableName} (Word, TranslationWord, Transcription, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?, ?)",
                                        list[0].ToLower().Trim(), list[1].ToLower().Trim(), list[2].Trim(), Settings.StandardNumberOfRepeats, DateTime.Now);
                                }
                                if (changes > 1) // из-за старых таблиц 
                                {
                                    DataBase.Languages.Execute($"DELETE FROM {tableName} WHERE Word = ?", list[0].ToLower().Trim());
                                    DataBase.Languages.Execute($"INSERT INTO {tableName} (Word, TranslationWord, Transcription, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?, ?)",
                                        list[0].ToLower().Trim(), list[1].ToLower().Trim(), list[2].Trim(), Settings.StandardNumberOfRepeats, DateTime.Now);
                                }
                            }
                            DataBase.Languages.Execute($"DELETE FROM {tableName} WHERE Transcription IS NULL OR trim(Transcription) = ''");
                        }
                }
            }
        }

    }
}