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
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Statistics_learn
    {  //Simple Statistics, 
        public int Position  { get; }            //Posithoin  in table words,
        public string Word { get; }           //Word Ebglish or Flag - "name"
        public static int AnswerTrue { get; set; }           //Answer ? true or false 
        public static int AnswerFalse { get; set; }
        public int Learn { get; }          //Answer ? true or false 
        public Statistics_learn(int position_new, string word_new, int Learn_new)
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
    }

    public class Database_Statistics // Класс для считывания базы данных Stat
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public int True { get; set; }
        public int False { get; set; }
        public DateTime DateOfTesting { get; set; }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    

    public static class Magic_constants // Маааагия!
    {
        public static int repeat_count = 20; // количество повторений;
        public static int maxLearn = 12;
        public static int numberLearn = 6;
        public static int false_answer = 3;
        public static int true_answer = 1;
        public static int language = 0; // 0 - eng, 1 - rus ...
    }
}
