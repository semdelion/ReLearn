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

    public class Database //Класс для считывания базы данных English
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string enWords { get; set; }
        public string ruWords { get; set; }
        public int numberLearn { get; set; }
        public int dateRepeat { get; set; }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Statistics_learn
    {  //Simple Statistics, 
        int position = 0;            //Posithoin  in table words,
        string word = "";            //Word Ebglish or Flag - "name"
        public static int answerTrue = 0;          //Answer ? true or false 
        public static int answerFalse = 0;
        int learn = 0;          //Answer ? true or false 
        public Statistics_learn(int position_new, string word_new, int Learn_new)
        {
            position = position_new;
            word = word_new;

            if (learn + Learn_new > Magic_constants.maxLearn)
                learn = Magic_constants.maxLearn;
            else if (learn + Learn_new < 0)
                learn = 0;
            else
                learn = Learn_new;
        }
        public string Word { get { return word; } }
        public int Position { get { return position; } }
        public int Learn { get { return learn; } }
    }

    public class Database_Stat // Класс для считывания базы данных Stat
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public int True { get; set; }
        public int False { get; set; }
        public DateTime DateOfTesting { get; set; }
    }

    class Database_Stat_My_Directly : Database_Stat { }
    class Database_Stat_PopularWords : Database_Stat{ }
    class Database_Stat_Flags : Database_Stat { }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static class NameDatabase // Имена баз данных
    {
        public static string Statistics = "database_stat.db3";

        public static string English_DB = "database_words.db3";
        public static string Flags_DB = "database_flags.db3";

        public static string English_Stat_DB = "database_english_stat.db3";

        public static string Flags_DB_Stat_DB = "database_flags_stat.db3";
        public static string Setting_DB = "Setting_Database.db3";
    }

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
