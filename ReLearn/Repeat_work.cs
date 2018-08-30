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
        public static void Delete_Repeat(List<Statistics_learn> Stats, String identifier, int rand_word, int RepeatLearn)
        {
            for (int i = 0; i < Stats.Count; i++)
                if (Stats[i].Word == identifier)
                {
                    Stats[i] = new Statistics_learn(rand_word, identifier, RepeatLearn);
                    return;
                }
            Stats.Add(new Statistics_learn(rand_word, identifier, RepeatLearn));
        }

        public static void Add_Statistics(int True, int False)// добовление статистики в базу данных 
        {
            string name_table = DataBase.Table_Name + "_Statistics";
            var database = DataBase.Connect(Database_Name.Statistics);
            database.CreateTable<Database_Statistics>();
            database.Query<Database_Statistics>("INSERT INTO " + name_table + " (True, False, DateOfTesting) VALUES (" + True + "," + False + ", DATETIME('NOW'))");
        }

        public static string Word_det(Database_Flags word) // возвращает флаг на языке  // 0 - eng / 1 - rus
        {
            if (Magic_constants.language == 0)
                return word.Name_flag_en;
            if (Magic_constants.language == 1)
                return word.Name_flag_ru;
            return "";
        }
    }
}