using SQLite;
using System;
using System.Collections.Generic;

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
        public string Word { get; set; } = "";
        public string TranslationWord { get; set; } = "";
        public string Transcription { get; set; }
        public int NumberLearn { get; set; } = Settings.StandardNumberOfRepeats;
        public DateTime DateRecurrence { get; set; } = DateTime.Today;

        public DBWords(){}

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

        public static bool ContainColumn(string columnName, List <SQLiteConnection.ColumnInfo> columns)
        {
            foreach (var column in columns)
                if (column.Name == columnName)
                    return true;
            return false;      
        }

       

        public static bool WordIsContained(string Word) =>
            DataBase.Languages.Query<DBWords>($"SELECT * FROM {TableNamesLanguage.My_Directly} WHERE Word = ? LIMIT 1", Word).Count != 0 
            ? true : false;

        public static void Insert(string Word, string TranslationWord) =>
            DataBase.Languages.Execute(
                $"INSERT INTO {TableNamesLanguage.My_Directly} (Word, TranslationWord, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?)",
                Word, TranslationWord, Settings.StandardNumberOfRepeats, DateTime.Now);

        public static List<DBWords> GetDataNotLearned => DataBase.Languages.Query<DBWords>(
            $"SELECT * FROM {DataBase.TableName} WHERE NumberLearn != 0 ORDER BY DateRecurrence ASC");

        public static void UpdateLearningNext(string Word) => DataBase.Languages.Execute(
            $"UPDATE {DataBase.TableName} SET DateRecurrence = ? WHERE Word = ?",
            DateTime.Now, Word);

        public static void UpdateLearningNotRepeat(string Word) => DataBase.Languages.Execute(
            $"UPDATE {DataBase.TableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?", DateTime.Now, 0, Word);

        public static void Update(string word, int learn) // изменение у BD элемента NumberLearn
        {
                DataBase.Languages.Execute(
                    $"UPDATE {DataBase.TableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?", 
                    DateTime.Now, learn, word);       
        }

        public static List<DBWords> GetData => DataBase.Languages.Query<DBWords>($"SELECT * FROM {DataBase.TableName}");

        public static void Delete(string Word) => DataBase.Languages.Execute($"DELETE FROM {DataBase.TableName} WHERE Word = ?", Word);
    }
}


    
