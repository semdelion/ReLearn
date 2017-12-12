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
    class DatabaseOfWords // Строка базы данных
    {
        public string enWords = null;
        public string ruWords = null;
        public int numberLearn = 0;
        public int dateRepeat;
        public DatabaseOfWords Add(string en, string ru, int n, int date)
        {
            this.enWords = en;
            this.ruWords = ru;
            this.numberLearn = n;
            this.dateRepeat = date;
            return this;
        }
    }
    public class Words // Строка базы данных
    {
        public string enWords { get; set; }
        public string ruWords { get; set; }
        public int numberLearn { get; set; }
        public int dateRepeat { get; set; }      
    }
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
    class DatabaseOfFlags // флаг 
    {
        public int image_name = 0;
        public string name_flag_en = null;
        public string name_flag_ru = null;
        public int numberLearn = 0;
        public int dateRepeat = 0;
        public DatabaseOfFlags Add(int image_n, string flag_en, string flag_ru, int nLearn, int date)
        {
            this.image_name = image_n;
            this.name_flag_en = flag_en;
            this.name_flag_ru = flag_ru;
            this.numberLearn = nLearn;
            this.dateRepeat = date;
            return this;
        }
    }
    public class Database_Flags // Класс для считывания базы данных flags
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public int image_name { get; set; }
        public string name_flag_en { get; set; }
        public string name_flag_ru { get; set; }
        public int numberLearn { get; set; }
        public int dateRepeat { get; set; }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    class DatabaseOfSetting// настройки
    {      
        public string Setting_bd = null;
        public string Name_bd = null;
        public int full_or_empty = 0;
        public int language = 0;      
        public DatabaseOfSetting Add(string Set_bd, string N_bd, int f_or_e, int lan)
        {
            this.Setting_bd = Set_bd;
            this.Name_bd = N_bd;
            this.full_or_empty = f_or_e;
            this.language = lan;
            return this;
        }
    }
    public class Setting_Database // Класс для считывания базы данных Stat
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Setting_bd { get; set; }
        public string Name_bd { get; set; }
        public int full_or_empty { get; set; }
        public int language { get; set; }
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

            if (learn + Learn_new > magic_constants.maxLearn)
                learn = magic_constants.maxLearn;
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
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static class NameDatabase // Имена баз данных
    {
        public static string Statistics = "database_english_stat.db3";

        public static string English_DB = "database.db3";
        public static string English_Stat_DB = "database_english_stat.db3";

        public static string Flags_DB = "database_flags_en_ru.db3";
        public static string Flags_DB_Stat_DB = "database_flags_stat.db3";

        public static string Flags_DB_short = "database_flags_short_en_ru.db3";
        public static string Flags_DB_long = "database_flags_en_ru.db3";

        public static string Setting_DB = "Setting_Database.db3";
    }

    public static class magic_constants // Маааагия!
    {
        public static int repeat_count = 20; // количество повторений;
        public static int maxLearn = 20;
        public static int numberLearn = 10;
        public static int false_answer = 3;
        public static int true_answer = 1;
        public static int language = 0; // 0 - eng, 1 - rus ...
    }
}
