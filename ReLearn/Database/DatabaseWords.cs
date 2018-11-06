using System;
using System.Collections.Generic;
using System.IO;
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
    enum TableNamesLanguage
    {
        My_Directly,
        Home,
        Education,
        Popular_Words,
        ThreeFormsOfVerb
    }

    public class DBWords //Класс для считывания базы данных English
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Word { get; set; }
        public string TranslationWord { get; set; }
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

        public static void UpdateWordsToRepeat()
        {
            var toDay = DateTime.Today.AddMonths(-1);
            foreach (string tableName in Enum.GetNames(typeof(TableNamesLanguage)))
            {
                var dataBase = DataBase.Languages.Query<DBWords>($"SELECT * FROM {tableName} WHERE NumberLearn = 0 ");
                foreach (var s in dataBase)
                {
                    if (s.DateRecurrence < toDay)
                    {
                        var query = $"UPDATE {tableName} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?";
                        DataBase.Languages.Execute(query, DateTime.Now, s.NumberLearn + 1, s.Word);
                    }
                }
            }
        }

        public static void СreateNewTableToLanguagesDataBase()
        {
            foreach (string tableName in Enum.GetNames(typeof(TableNamesLanguage)))
            {
                if (DataBase.Languages.GetTableInfo(tableName).Count == 0)
                {
                    DataBase.Languages.Query<DBWords>($"CREATE TABLE {tableName} (_id int PRIMARY KEY, Word string, TranslationWord string, NumberLearn int, DateRecurrence DateTime, Context string, Image string)");
                    using (StreamReader reader = new StreamReader(Application.Context.Assets.Open($"Database/{tableName}.txt")))
                    {
                        string str_line;
                        while ((str_line = reader.ReadLine()) != null)
                        {
                            var list_en_ru = str_line.Split('|');
                            var query = $"INSERT INTO {tableName} (Word, TranslationWord, NumberLearn, DateRecurrence) VALUES (?, ?, ?, ?)";
                            DataBase.Languages.Execute(query, list_en_ru[0].ToLower(), list_en_ru[1].ToLower(), Settings.StandardNumberOfRepeats, DateTime.Now);
                        }
                    }
                    DataBase.Statistics.Query<DatabaseStatistics>($"CREATE TABLE {tableName}_Statistics (_id int PRIMARY KEY, True int, False int, DateOfTesting DateTime)");
                }
            }
        }

        public static bool WordIsContained(string Word) =>
            DataBase.Languages.Query<DBWords>($"SELECT * FROM {TableNamesLanguage.My_Directly.ToString()} WHERE Word = ? LIMIT 1", Word).Count != 0 
            ? true : false;

        public static void Add(string Word, string TranslationWord) =>
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

        public static void Update(List<Statistics> Stats) // изменение у BD элемента NumberLearn
        {
            for (int i = 0; i < Stats.Count; i++)
                DataBase.Languages.Execute(
                    $"UPDATE {DataBase.TableNameLanguage} SET DateRecurrence = ?, NumberLearn = ? WHERE Word = ?", 
                    DateTime.Now, Stats[i].Learn, Stats[i].Word);       
        }

        public static List<DBWords> GetData => DataBase.Languages.Query<DBWords>($"SELECT * FROM {DataBase.TableNameLanguage.ToString()}");

        public static void Delete(string Word) => DataBase.Languages.Query<DBWords>($"DELETE FROM {DataBase.TableNameLanguage} WHERE Word = ?", Word);

    }
}


    
