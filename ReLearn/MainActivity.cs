using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using SQLite;
using System.IO;
using Android.Views;
using System.Threading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Runtime;
using Android.Graphics;

namespace ReLearn
{
    [Activity( MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        public static Button button_english;
        public static Button button_flags;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            GUI.Res = this;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundMain));
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(127, 0, 0, 0));

            button_english = FindViewById<Button>(Resource.Id.button_english);
            button_flags = FindViewById<Button>(Resource.Id.button_flags);

            button_english.Touch += GUI.NewTouch;
            button_flags.Touch += GUI.NewTouch;

            try
            {
                var databaseSetting = DataBase.Connect(NameDatabase.Setting_DB); // загружаем настройки
                databaseSetting.CreateTable<Setting_Database>();

                //databaseSetting.DropTable<Setting_Database>();
                // var databaseFlags = DataBase.Connect(NameDatabase.Flags_DB); // подключение к БД
                //databaseFlags.DropTable<Database_Flags>();
                // databaseFlags.CreateTable<Database_Flags>();
                var search_Setting = databaseSetting.Query<Setting_Database>("SELECT * FROM Setting_Database");
                if (search_Setting.Count == 0)
                    using (StreamReader reader = new StreamReader(Assets.Open("setting.txt")))
                        while (reader.Peek() >= 0)
                            DataBase.Add_Setting(reader, databaseSetting);
                search_Setting = databaseSetting.Query<Setting_Database>("SELECT full_or_empty  FROM Setting_Database WHERE Setting_bd = 'flags'");
                if (search_Setting[0].full_or_empty == 0)
                {
                    var databaseFlags = DataBase.Connect(NameDatabase.Flags_DB); // подключение к БД
                    databaseFlags.DropTable<Database_Flags>();
                    databaseFlags.CreateTable<Database_Flags>();
                    var tableFlags = databaseFlags.Table<Database_Flags>();
                    using (StreamReader reader = new StreamReader(Assets.Open("database_flags.txt")))
                        while (reader.Peek() >= 0)
                            DataBase.Add_newFlag(reader, databaseFlags);
                    databaseSetting.Query<Setting_Database>("UPDATE Setting_Database SET full_or_empty = " + 1 + " WHERE Setting_bd = 'flags'");

                    var db = DataBase.Connect(NameDatabase.English_DB);
                    db.CreateTable<Database>();

                    var tableWords = db.Table<Database>();
                    using (StreamReader reader = new StreamReader(Assets.Open("BDNew1.txt")))
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
                DataBase.update_English_DB(Month); // обнавление repeat_number если слово не повторялась более месяца 
                DataBase.update_Flags_DB(Month);   // обнавление repeat_number если слово не повторялась более месяца 
            }
            catch
            {
                Toast.MakeText(this, "Error : Can't connect to database or update", ToastLength.Long).Show();
            }

            button_english.Click += delegate
            {
                GUI.button_click(button_english);
                Intent intent_english = new Intent(this, typeof(English));
                StartActivity(intent_english);
            };

            button_flags.Click += delegate
            {
                GUI.button_click(button_flags);
                Intent intent_flags = new Intent(this, typeof(Flags));
                StartActivity(intent_flags);
            };
        }  
    }
}

