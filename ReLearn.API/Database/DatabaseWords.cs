using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace ReLearn.API.Database
{
    public enum TableNamesLanguage
    {
        My_Directly,
        Home,
        Education,
        Popular_Words,
        ThreeFormsOfVerb,
        ComputerScience,
        Nature
    }

    public class DBWords //Класс для считывания базы данных English
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Word { get; set; }
        public string TranslationWord { get; set; }
        public string Transcription { get; set; }
        public int NumberLearn { get; set; }
        public DateTime DateRecurrence { get; set; }

        public DBWords()
        {
            DateRecurrence = DateTime.Today;
            Word = "";
            NumberLearn = Settings.StandardNumberOfRepeats;
            TranslationWord = "";
        }

        public DBWords(DBWords x)
        {
            DateRecurrence = x.DateRecurrence;
            Word = x.Word;
            NumberLearn = x.NumberLearn;
            TranslationWord = x.TranslationWord;
        }

        public DBWords Find() => this;    

        public static void UpdateData()
        {
            var toDay = DateTime.Today.AddMonths(-1);
            foreach (string tableName in Enum.GetNames(typeof(TableNamesLanguage)))
            {
                var dataBase = DataBase.Languages.Query<DBWords>($"SELECT * FROM {tableName} WHERE NumberLearn = 0 ");
                foreach (var s in dataBase)
                    if (s.DateRecurrence < toDay)
                        DataBase.Languages.Execute($"UPDATE {tableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?", DateTime.Now, s.NumberLearn + 1, s.Word);
            }
        }

        //public static void СreateTable()
        //{
        //    foreach (string tableName in Enum.GetNames(typeof(TableNamesLanguage)))
        //    {
        //        if (DataBase.Languages.GetTableInfo(tableName).Count == 0)
        //        {
        //            DataBase.Languages.Execute($"CREATE TABLE {tableName} (_id int PRIMARY KEY, Word string, TranslationWord string, Transcription string, NumberLearn int, DateRecurrence DateTime, Context string, Image string)");
        //            using (StreamReader reader = new StreamReader(Application.Context.Assets.Open($"Database/{tableName}.txt")))
        //            {
        //                string str_line;
        //                while ((str_line = reader.ReadLine()) != null)
        //                {
        //                    var list = str_line.Split('|');
        //                    var query = $"INSERT INTO {tableName} (Word, TranslationWord, Transcription, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?, ?)";
        //                    DataBase.Languages.Execute(query, list[0].ToLower().Trim(), list[1].ToLower().Trim(), list[2].Trim(), Settings.StandardNumberOfRepeats, DateTime.Now);
        //                }
        //            }
        //            DataBase.Statistics.Execute($"CREATE TABLE {tableName}_Statistics (_id int PRIMARY KEY, True int, False int, DateOfTesting DateTime)");
        //        }
        //    }
        //}

        static bool ContainColumn(string columnName, List <SQLiteConnection.ColumnInfo> columns)
        {
            foreach (var column in columns)
                if (column.Name == columnName)
                    return true;
            return false;      
        }

        //public static void ADDCOLUMN()
        //{
        //    foreach (string tableName in Enum.GetNames(typeof(TableNamesLanguage)))
        //    {
        //        if (!ContainColumn("Transcription", DataBase.Languages.GetTableInfo(tableName)))
        //        {
                    
        //            DataBase.Languages.Execute($"ALTER TABLE {tableName} ADD COLUMN Transcription string");
        //            if (tableName != TableNamesLanguage.My_Directly.ToString())
        //                using (StreamReader reader = new StreamReader(Application.Context.Assets.Open($"Database/{tableName}.txt")))
        //                {
        //                    string str_line;
        //                    while ((str_line = reader.ReadLine()) != null)
        //                    {
        //                        var list = str_line.Split('|');
        //                        int changes = DataBase.Languages.Execute($"UPDATE {tableName} SET Word = ?, Transcription = ? WHERE Word = ?", list[0].ToLower().Trim(), list[2].Trim(), list[0].ToLower().Trim())
        //                                    + DataBase.Languages.Execute($"UPDATE {tableName} SET Word = ?, Transcription = ? WHERE Word = ?", list[0].ToLower().Trim(), list[2].Trim(), list[0].ToLower().Trim() + " ")
        //                                    + DataBase.Languages.Execute($"UPDATE {tableName} SET Word = ?, Transcription = ? WHERE Word = ?", list[0].ToLower().Trim(), list[2].Trim(), list[0].ToLower().Trim() + "  ");
        //                        if (changes == 0)
        //                        {
        //                            DataBase.Languages.Execute($"INSERT INTO {tableName} (Word, TranslationWord, Transcription, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?, ?)",
        //                                list[0].ToLower().Trim(), list[1].ToLower().Trim(), list[2].Trim(), Settings.StandardNumberOfRepeats, DateTime.Now);
        //                        }
        //                        if (changes > 1) // из-за старых таблиц 
        //                        {
        //                            DataBase.Languages.Execute($"DELETE FROM {tableName} WHERE Word = ?", list[0].ToLower().Trim());
        //                            DataBase.Languages.Execute($"INSERT INTO {tableName} (Word, TranslationWord, Transcription, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?, ?)",
        //                                list[0].ToLower().Trim(), list[1].ToLower().Trim(), list[2].Trim(), Settings.StandardNumberOfRepeats, DateTime.Now);
        //                        }
        //                    }
        //                    DataBase.Languages.Execute($"DELETE FROM {tableName} WHERE Transcription IS NULL OR trim(Transcription) = ''");
        //                }
        //        }
        //    }
        //}

        public static bool WordIsContained(string Word) =>
            DataBase.Languages.Query<DBWords>($"SELECT * FROM {TableNamesLanguage.My_Directly.ToString()} WHERE Word = ? LIMIT 1", Word).Count != 0 
            ? true : false;

        public static void Insert(string Word, string TranslationWord) =>
            DataBase.Languages.Execute(
                $"INSERT INTO {TableNamesLanguage.My_Directly.ToString()} (Word, TranslationWord, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?)",
                Word, TranslationWord, Settings.StandardNumberOfRepeats, DateTime.Now);

        public static List<DBWords> GetDataNotLearned => DataBase.Languages.Query<DBWords>(
            $"SELECT * FROM {DataBase.TableNameLanguage} WHERE NumberLearn != 0 ORDER BY DateRecurrence ASC");

        public static void UpdateLearningNext(string Word) => DataBase.Languages.Execute(
            $"UPDATE {DataBase.TableNameLanguage} SET DateRecurrence = ? WHERE Word = ?",
            DateTime.Now, Word);

        public static void UpdateLearningNotRepeat(string Word) => DataBase.Languages.Execute(
            $"UPDATE {DataBase.TableNameLanguage} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?", DateTime.Now, 0, Word);

        public static void Update(string word, int learn) // изменение у BD элемента NumberLearn
        {
                DataBase.Languages.Execute(
                    $"UPDATE {DataBase.TableNameLanguage} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?", 
                    DateTime.Now, learn, word);       
        }

        public static List<DBWords> GetData => DataBase.Languages.Query<DBWords>($"SELECT * FROM {DataBase.TableNameLanguage.ToString()}");

        public static void Delete(string Word) => DataBase.Languages.Execute($"DELETE FROM {DataBase.TableNameLanguage} WHERE Word = ?", Word);
    }
}


    
