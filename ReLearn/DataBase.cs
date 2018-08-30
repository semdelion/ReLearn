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
using Plugin.Settings;

namespace ReLearn
{
    public static class Database_Name // Имена баз данных
    {
        public static string Statistics = "database_statistics.db3";
        public static string English_DB = "database_words.db3";
        public static string Flags_DB = "database_flags.db3";
        public static string Setting_DB = "Setting_Database.db3";
    }
    public static class Table_name// Имена баз данных
    {
        public const string My_Directly = "My_Directly";
        public const string Popular_Words = "Popular_Words";
        public const string Database_Flags = "Database_Flags";
    }


    public static class DataBase
    {
        public static string Table_Name = CrossSettings.Current.GetValueOrDefault("DictionaryName", null);

        public static SQLiteConnection Connect(string nameDB)
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), nameDB);
            return new SQLiteConnection(databasePath);
        }

        public static void GetDatabasePath(string sqliteFilename)
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            // копирование файла из папки Assets по пути path
            if (!File.Exists(path))
            {
                // получаем контекст приложения
                Context context = Android.App.Application.Context;
                var dbAssetStream = context.Assets.Open("Database/" + sqliteFilename);
                var dbFileStream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate);
                var buffer = new byte[1024];
                int length;

                while ((length = dbAssetStream.Read(buffer, 0, buffer.Length)) > 0)
                    dbFileStream.Write(buffer, 0, length);
                dbFileStream.Flush();
                dbFileStream.Close();
                dbAssetStream.Close();
            }          
        }

        public static void Update_English_DB(int Month)
        {
            var database = DataBase.Connect(Database_Name.English_DB); // подключение к БД
            database.CreateTable<Database_Words>();
            var table = database.Table<Database_Words>();
            foreach (var s in table) // UPDATE Database
                if (Month != s.DateRecurrence && s.NumberLearn == 0)
                {  // обновление БД, при условии, что месяцы не совпадают и NumberLearn == 0. изменяем месяц на текущий и NumberLearn++;                
                    database.Query<Database_Words>("UPDATE Database SET DateRecurrence = " + Month + " WHERE Word = ?", s.Word);
                    database.Query<Database_Words>("UPDATE Database SET NumberLearn = " + s.NumberLearn + 1 + " WHERE Word = ?", s.Word);
                }
        }

        public static void Update_Flags_DB(int Month)
        {
            var databaseFlags = DataBase.Connect(Database_Name.Flags_DB); // подключение к БД
            databaseFlags.CreateTable<Database_Flags>();
            var tableFlags = databaseFlags.Table<Database_Flags>();
            foreach (var s in tableFlags) // UPDATE Database_Flags
                if (Month != s.DateRecurrence && s.NumberLearn == 0)
                {  // обновление БД, при условии, что месяцы не совпадают и NumberLearn == 0. изменяем месяц на текущий и NumberLearn++;                
                    databaseFlags.Query<Database_Flags>("UPDATE Database_Flags SET DateRecurrence = " + Month + " WHERE Image_name = ?", s.Image_name);
                    databaseFlags.Query<Database_Flags>("UPDATE Database_Flags SET NumberLearn = " + s.NumberLearn + 1 + " WHERE Image_name = ?", s.Image_name);
                }
        }

        public static void Check_and_update_database()
        {
            try
            {
                int Month = System.DateTime.Today.Month;
                Update_English_DB(Month); // обнавление repeat_number если слово не повторялась более месяца 
                Update_Flags_DB(Month);   // обнавление repeat_number если слово не повторялась более месяца 
            }
            catch
            {
                Toast.MakeText(GUI.Res, "Error : Can't connect to database or update", ToastLength.Long).Show();
            }
        }
    }

    public class Database_Words //Класс для считывания базы данных English
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Word { get; set; }
        public string TranslationWord { get; set; }
        public int NumberLearn { get; set; }
        public int DateRecurrence { get; set; }

        public Database_Words()
        {
            this.DateRecurrence = 1;
            this.Word = "";
            this.NumberLearn = 6;
            this.TranslationWord = "";
        }

        public Database_Words(Database_Words x)
        {
            this.DateRecurrence = x.DateRecurrence;
            this.Word = x.Word;
            this.NumberLearn = x.NumberLearn;
            this.TranslationWord = x.TranslationWord;
        }

        public Database_Words Find() => this;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Database_Flags // Класс для считывания базы данных flags
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Image_name { get; set; }
        public string Name_flag_en { get; set; }
        public string Name_flag_ru { get; set; }
        public int NumberLearn { get; set; }
        public int DateRecurrence { get; set; }

        public Database_Flags Add(string image_n, string flag_en, string flag_ru, int nLearn, int date)
        {
            this.Image_name = image_n;
            this.Name_flag_en = flag_en;
            this.Name_flag_ru = flag_ru;
            this.NumberLearn = nLearn;
            this.DateRecurrence = date;
            return this;
        }
        public Database_Flags()
        {
            Image_name = null;
            Name_flag_en = null;
            Name_flag_ru = null;
            NumberLearn = 0;
            DateRecurrence = 0;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    //class DatabaseOfSetting// настройки
    //{
    //    public string Setting_bd = null;
    //    public string Name_bd = null;
    //    public int full_or_empty = 0;
    //    public int language = 0;
    //    public DatabaseOfSetting Add(string Set_bd, string N_bd, int f_or_e, int lan)
    //    {
    //        this.Setting_bd = Set_bd;
    //        this.Name_bd = N_bd;
    //        this.full_or_empty = f_or_e;
    //        this.language = lan;
    //        return this;
    //    }
    //}
    //public class Setting_Database // Класс для считывания базы данных Stat
    //{
    //    [PrimaryKey, AutoIncrement, Column("_id")]
    //    public int Id { get; set; }
    //    public string Setting_bd { get; set; }
    //    public string Name_bd { get; set; }
    //    public int full_or_empty { get; set; }
    //    public int language { get; set; }
    //}
}


//public static void Add_new_Flag(StreamReader reader, SQLiteConnection databaseFlags)
//{
//    string str_line = reader.ReadLine();
//    var list_imageName_flag = str_line.Split('|');
//    var database_Flags = new Database_Flags
//    {
//        Image_name = list_imageName_flag[0],
//        Name_flag_en = list_imageName_flag[1],
//        Name_flag_ru = list_imageName_flag[2],
//        NumberLearn = 10,
//        DateRecurrence = System.DateTime.Today.Month
//    };
//    databaseFlags.Insert(database_Flags);
//}

//public static void Add_Setting(StreamReader reader, SQLiteConnection databaseSetting)
//{
//    string str_line = reader.ReadLine();
//    var list_sett = str_line.Split('|');
//    var sett_bd = new Setting_Database
//    {
//        Setting_bd = list_sett[0],
//        Name_bd = list_sett[1],
//        full_or_empty = System.Convert.ToInt32(list_sett[2]),
//        language = System.Convert.ToInt32(list_sett[3])
//    };
//    databaseSetting.Insert(sett_bd);
//}