using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace ReLearn
{

    public static class DataBase
    {

        public static SQLiteConnection Connect(string nameDB )
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), nameDB);
            return new SQLiteConnection(databasePath); ;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //update database
        public static void update_English_DB(int Month)
        {
            
            var database = DataBase.Connect(NameDatabase.English_DB); // подключение к БД
            database.CreateTable<Database>();
            var table = database.Table<Database>();
            foreach (var s in table) // UPDATE Database
                if (Month != s.dateRepeat && s.numberLearn == 0)
                {  // обновление БД, при условии, что месяцы не совпадают и numberLearn == 0. изменяем месяц на текущий и numberLearn++;                
                    database.Query<Database>("UPDATE Database SET dateRepeat = " + Month + " WHERE enWords = ?", s.enWords);
                    database.Query<Database>("UPDATE Database SET numberLearn = " + s.numberLearn + 1 + " WHERE enWords = ?", s.enWords);
                }
        }
        public static void update_Flags_DB(int Month)
        {
            var databaseFlags = DataBase.Connect(NameDatabase.Flags_DB); // подключение к БД
            databaseFlags.CreateTable<Database_Flags>();
            var tableFlags = databaseFlags.Table<Database_Flags>();
            foreach (var s in tableFlags) // UPDATE Database_Flags
                if (Month != s.dateRepeat && s.numberLearn == 0)
                {  // обновление БД, при условии, что месяцы не совпадают и numberLearn == 0. изменяем месяц на текущий и numberLearn++;                
                    databaseFlags.Query<Database_Flags>("UPDATE Database_Flags SET dateRepeat = " + Month + " WHERE image_name = ?", s.image_name);
                    databaseFlags.Query<Database_Flags>("UPDATE Database_Flags SET numberLearn = " + s.numberLearn + 1 + " WHERE image_name = ?", s.image_name);
                }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Add new element
        public static void Add_newFlag(StreamReader reader, SQLiteConnection databaseFlags)
        {
            string str_line = reader.ReadLine();
            var list_imageName_flag = str_line.Split('|');
            var database_Flags = new Database_Flags();
            database_Flags.image_name = System.Convert.ToInt32(list_imageName_flag[0]);
            database_Flags.name_flag_en = list_imageName_flag[1];
            database_Flags.name_flag_ru = list_imageName_flag[2];
            database_Flags.numberLearn = 10;
            database_Flags.dateRepeat = System.DateTime.Today.Month;
            databaseFlags.Insert(database_Flags);
        }
        public static void Add_Setting(StreamReader reader, SQLiteConnection databaseSetting)
        {
            string str_line = reader.ReadLine();
            var list_sett = str_line.Split('|');
            var sett_bd = new Setting_Database();
            sett_bd.Setting_bd = list_sett[0];
            sett_bd.Name_bd = list_sett[1];
            sett_bd.full_or_empty = System.Convert.ToInt32(list_sett[2]);
            sett_bd.language = System.Convert.ToInt32(list_sett[3]);
            databaseSetting.Insert(sett_bd);
        }
        public static void Add_English_word(string eng, string rus, SQLiteConnection database)
        {
            var newWords = new Database();
            newWords.enWords = eng.ToLower();
            newWords.ruWords = rus.ToLower();
            newWords.numberLearn = magic_constants.numberLearn;
            newWords.dateRepeat = System.DateTime.Today.Month;
            database.Insert(newWords);
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}