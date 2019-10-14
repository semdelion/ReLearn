using ReLearn.API.Database.Interface;
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

    public class DatabaseWords : IDatabase
    {
        public DatabaseWords()
        {
        }

        public DatabaseWords(DatabaseWords x)
        {
            DateRecurrence = x.DateRecurrence;
            Word = x.Word;
            NumberLearn = x.NumberLearn;
            TranslationWord = x.TranslationWord;
        }

        [PrimaryKey]
        [AutoIncrement]
        [Column("_id")]
        public int Id { get; set; }

        public string Word { get; set; } = "";
        public string TranslationWord { get; set; } = "";
        public string Transcription { get; set; }
        public int NumberLearn { get; set; } = Settings.StandardNumberOfRepeats;
        public DateTime DateRecurrence { get; set; } = DateTime.Today;

        public DatabaseWords Find()
        {
            return this;
        }

        public static async Task UpdateData()
        {
            var toDay = DateTime.Today.AddMonths(-1);
            foreach (var tableName in Enum.GetNames(typeof(TableNamesLanguage)))
            {
                var dataBase =
                    await DataBase.Languages.QueryAsync<DatabaseWords>(
                        $"SELECT * FROM {tableName} WHERE NumberLearn = 0 ");
                foreach (var s in dataBase)
                {
                    if (s.DateRecurrence < toDay)
                    {
                        await DataBase.Languages.ExecuteAsync(
                            $"UPDATE {tableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?", DateTime.Now,
                            s.NumberLearn + 1, s.Word);
                    }
                }
            }
        }

        public static bool ContainColumn(string columnName, List<SQLiteConnection.ColumnInfo> columns)
        {
            foreach (var column in columns)
            {
                if (column.Name == columnName)
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task<bool> WordIsContained(string word)
        {
            var database =
                await DataBase.Languages.QueryAsync<DatabaseWords>(
                    $"SELECT * FROM {TableNamesLanguage.My_Directly} WHERE Word = ? LIMIT 1", word);
            return database.Count != 0 ? true : false;
        }

        public static async Task Insert(string Word, string translationWord)
        {
            await DataBase.Languages.ExecuteAsync(
                $"INSERT INTO {TableNamesLanguage.My_Directly} (Word, TranslationWord, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?)",
                Word, translationWord, Settings.StandardNumberOfRepeats, DateTime.Now);
        }

        public static async Task<List<DatabaseWords>> GetDataNotLearned()
        {
            return await DataBase.Languages.QueryAsync<DatabaseWords>(
                $"SELECT * FROM {DataBase.TableName} WHERE NumberLearn != 0 ORDER BY DateRecurrence ASC");
        }

        public static async Task UpdateLearningNext(string word)
        {
            await DataBase.Languages.ExecuteAsync(
                $"UPDATE {DataBase.TableName} SET DateRecurrence = ? WHERE Word = ?",
                DateTime.Now, word);
        }

        public static async Task UpdateLearningNotRepeat(string word)
        {
            await DataBase.Languages.ExecuteAsync(
                $"UPDATE {DataBase.TableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?", DateTime.Now, 0,
                word);
        }

        public static async Task Update(string word, int learn)
        {
            await DataBase.Languages.ExecuteAsync(
                $"UPDATE {DataBase.TableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?",
                DateTime.Now, learn, word);
        }

        public static async Task<List<DatabaseWords>> GetData()
        {
            return await DataBase.Languages.QueryAsync<DatabaseWords>(
                $"SELECT * FROM {DataBase.TableName}");
        }

        public static async Task Delete(string word)
        {
            await DataBase.Languages.ExecuteAsync(
                $"DELETE FROM {DataBase.TableName} WHERE Word = ?", word);
        }
    }
}