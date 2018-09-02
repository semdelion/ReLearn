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


namespace ReLearn
{
    public class Statistics
    {  
        public int Position  { get; }                        //Posithoin  in table words,
        public string Word { get; }                          //Word Ebglish or Flag - "name"
        public static int AnswerTrue = 0;                    //Answer ? true or false 
        public static int AnswerFalse = 0;
        public int Learn { get; }

        public static void Statistics_update()
        {
            AnswerTrue = 0;                    
            AnswerFalse = 0;
        }

        public Statistics(int position_new, string word_new, int Learn_new)
        {
            Position = position_new;
            Word = word_new;

            if (Learn + Learn_new > Magic_constants.maxLearn)
                Learn = Magic_constants.maxLearn;
            else if (Learn + Learn_new < 0)
                Learn = 0;
            else
                Learn = Learn_new;
        }

        public static void Add_Statistics(int True, int False)// добовление статистики в базу данных 
        {
            string name_table = DataBase.Table_Name + "_Statistics";
            var database = DataBase.Connect(Database_Name.Statistics);
            database.Query<Database_Statistics>("INSERT INTO " + name_table + " (True, False, DateOfTesting) VALUES (" + True + "," + False + ", DATETIME('NOW'))");
        }
    }

    public class Database_Statistics // Класс для считывания базы данных Stat
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public int True { get; set; }
        public int False { get; set; }
        public DateTime DateOfTesting { get; set; }
    }
}
