using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public static async Task UpdateData()
        {
            var toDay = DateTime.Today.AddMonths(-1);
            foreach (string tableName in Enum.GetNames(typeof(TableNamesLanguage)))
            {
                var dataBase = await DataBase.Languages.QueryAsync<DBWords>($"SELECT * FROM {tableName} WHERE NumberLearn = 0 ");
                foreach (var s in dataBase)
                    if (s.DateRecurrence < toDay)
                        await DataBase.Languages.ExecuteAsync($"UPDATE {tableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?", DateTime.Now, s.NumberLearn + 1, s.Word);
            }
        }

        public static bool ContainColumn(string columnName, List <SQLiteConnection.ColumnInfo> columns)
        {
            foreach (var column in columns)
                if (column.Name == columnName)
                    return true;
            return false;      
        }

        public static async Task<bool> WordIsContained(string Word)
        {
            var database = await DataBase.Languages.QueryAsync<DBWords>($"SELECT * FROM {TableNamesLanguage.My_Directly} WHERE Word = ? LIMIT 1", Word);
            return database.Count != 0 ? true : false;
        }

        public static async Task Insert(string Word, string TranslationWord) =>
            await DataBase.Languages.ExecuteAsync(
                $"INSERT INTO {TableNamesLanguage.My_Directly} (Word, TranslationWord, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?)",
                Word, TranslationWord, Settings.StandardNumberOfRepeats, DateTime.Now);

        public static async Task<List<DBWords>> GetDataNotLearned() => 
            await DataBase.Languages.QueryAsync<DBWords>(
                $"SELECT * FROM {DataBase.TableName} WHERE NumberLearn != 0 ORDER BY DateRecurrence ASC");

        public static async Task UpdateLearningNext(string Word) => 
            await DataBase.Languages.ExecuteAsync(
                $"UPDATE {DataBase.TableName} SET DateRecurrence = ? WHERE Word = ?",
                DateTime.Now, Word);

        public static async Task UpdateLearningNotRepeat(string Word) => 
            await DataBase.Languages.ExecuteAsync(
                $"UPDATE {DataBase.TableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?", DateTime.Now, 0, Word);

        public static async Task Update(string word, int learn) =>
            await DataBase.Languages.ExecuteAsync(
                $"UPDATE {DataBase.TableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?",
                DateTime.Now, learn, word);       
        
        public static async Task<List<DBWords>> GetData() => 
            await DataBase.Languages.QueryAsync<DBWords>(
                $"SELECT * FROM {DataBase.TableName}");

        public static async Task Delete(string Word) => 
            await DataBase.Languages.ExecuteAsync(
                $"DELETE FROM {DataBase.TableName} WHERE Word = ?", Word);
    }
}


    
