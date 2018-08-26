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
            return new SQLiteConnection(databasePath);
        }

        public static void Update_English_DB(int Month)
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

        public static void Update_Flags_DB(int Month)
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

        public static void Add_new_Flag(StreamReader reader, SQLiteConnection databaseFlags)
        {
            string str_line = reader.ReadLine();
            var list_imageName_flag = str_line.Split('|');
            var database_Flags = new Database_Flags
            {
                image_name = list_imageName_flag[0],
                name_flag_en = list_imageName_flag[1],
                name_flag_ru = list_imageName_flag[2],
                numberLearn = 10,
                dateRepeat = System.DateTime.Today.Month
            };
            databaseFlags.Insert(database_Flags);
        }

        public static void Add_Setting(StreamReader reader, SQLiteConnection databaseSetting)
        {
            string str_line = reader.ReadLine();
            var list_sett = str_line.Split('|');
            var sett_bd = new Setting_Database
            {
                Setting_bd = list_sett[0],
                Name_bd = list_sett[1],
                full_or_empty = System.Convert.ToInt32(list_sett[2]),
                language = System.Convert.ToInt32(list_sett[3])
            };
            databaseSetting.Insert(sett_bd);
        }

        public static void Add_English_word(string eng, string rus, SQLiteConnection database)
        {
            var newWords = new Database
            {
                enWords = eng.ToLower(),
                ruWords = rus.ToLower(),
                numberLearn = Magic_constants.numberLearn,
                dateRepeat = System.DateTime.Today.Month
            };
            database.Insert(newWords);
        }

        public static void Check_and_update_database()
        {
            try
            {
                var databaseSetting = DataBase.Connect(NameDatabase.Setting_DB); // загружаем настройки
                databaseSetting.CreateTable<Setting_Database>();

                /*databaseSetting.DropTable<Setting_Database>();
                var databaseFlags2 = DataBase.Connect(NameDatabase.Flags_DB); // подключение к БД
                databaseFlags2.DropTable<Database_Flags>();
                databaseFlags2.CreateTable<Database_Flags>();*/

                var search_Setting = databaseSetting.Query<Setting_Database>("SELECT * FROM Setting_Database");
                if (search_Setting.Count == 0)
                    using (StreamReader reader = new StreamReader(GUI.Res.Assets.Open("setting.txt")))
                        while (reader.Peek() >= 0)
                            DataBase.Add_Setting(reader, databaseSetting);
                search_Setting = databaseSetting.Query<Setting_Database>("SELECT full_or_empty  FROM Setting_Database WHERE Setting_bd = 'flags'");
                if (search_Setting[0].full_or_empty == 0)
                {
                    var databaseFlags = DataBase.Connect(NameDatabase.Flags_DB); // подключение к БД
                    databaseFlags.DropTable<Database_Flags>();
                    databaseFlags.CreateTable<Database_Flags>();
                    var tableFlags = databaseFlags.Table<Database_Flags>();
                    using (StreamReader reader = new StreamReader(GUI.Res.Assets.Open(/*"database_flags.txt"*/"databaseFlags.txt")))
                        while (reader.Peek() >= 0)
                            DataBase.Add_new_Flag(reader, databaseFlags);
                    databaseSetting.Query<Setting_Database>("UPDATE Setting_Database SET full_or_empty = " + 1 + " WHERE Setting_bd = 'flags'");

                    var db = DataBase.Connect(NameDatabase.English_DB);
                    db.CreateTable<Database>();

                    var tableWords = db.Table<Database>();
                    using (StreamReader reader = new StreamReader(GUI.Res.Assets.Open("BDNew1.txt")))
                        while (reader.Peek() >= 0)
                        {
                            string str_line = reader.ReadLine();
                            var list_en_ru = str_line.Split('|');
                            DataBase.Add_English_word(list_en_ru[0], list_en_ru[1], db);
                        }

                    List<DatabaseOfWords> dataBase = new List<DatabaseOfWords>();
                    var table = db.Table<Database>();
                    foreach (var word in table)
                    {   // создание БД в виде  List<DatabaseOfWords>
                        DatabaseOfWords w = new DatabaseOfWords();
                        if (word.numberLearn != 0)
                        {
                            w.Add(word.enWords, word.ruWords, word.numberLearn, word.dateRepeat);
                            dataBase.Add(w);
                        }
                    }
                }

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
}