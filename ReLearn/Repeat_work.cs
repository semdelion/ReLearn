using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;
namespace ReLearn
{
    class Repeat_work
    {
        public static void DeleteRepeat(List<Statistics_learn> Stats, String identifier, int rand_word, int RepeatLearn)
        {
            for (int i = 0; i < Stats.Count; i++)
                if (Stats[i].Word == identifier)
                {
                    Stats[i] = new Statistics_learn(rand_word, identifier, RepeatLearn);
                    return;
                }
            Stats.Add(new Statistics_learn(rand_word, identifier, RepeatLearn));
        }
        public static void AddStatistics(int True, int False, string name_Database)// добовление статистики в базу данных 
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), name_Database);
            var database = new SQLiteConnection(databasePath); // подключение к БД
            database.CreateTable<Database_Stat>();
            var newStat = new Database_Stat();
            newStat.True = True;
            newStat.False = False;
            database.Insert(newStat); // добовление
        }
        public static string word_det(DatabaseOfFlags word) // возвращает флаг на языке  // 0 - eng / 1 - rus
        {
            if (magic_constants.language == 0)
                return word.name_flag_en;
            if (magic_constants.language == 1)
                return word.name_flag_ru;
            return "";
        }
    }
}