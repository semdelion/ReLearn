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
using Plugin.Settings;

namespace ReLearn
{
    [Activity(Label = "Language", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class English : Activity
    {
        [Java.Interop.Export("Button_English_Add_Click")]
        public void Button_English_Add_Click(View v)
        {
            v.Enabled = false;
            Intent intent_english_add = new Intent(this, typeof(English_Add));
            StartActivity(intent_english_add);
        }

        [Java.Interop.Export("Button_English_Learn_Click")]
        public void Button_English_Learn_Click(View v)
        {
            v.Enabled = false;
            try
            {
                var database = DataBase.Connect(Database_Name.English_DB);
                database.CreateTable<Database_Words>();
                int search_occurrences = database.Query<Database_Words>("SELECT * FROM " + DataBase.Table_Name).Count;
                if (search_occurrences != 0)
                {
                    Intent intent_english_learn = new Intent(this, typeof(English_Learn));
                    StartActivity(intent_english_learn);
                }
                else
                    Toast.MakeText(this, "The database is empty", ToastLength.Short).Show();
            }
            catch { Toast.MakeText(this, "Error : can't connect to database", ToastLength.Long).Show(); }
        }

        [Java.Interop.Export("Button_English_Repeat_Click")]
        public void Button_English_Repeat_Click(View v)
        {
            v.Enabled = false;
            try
            {
                var database = DataBase.Connect(Database_Name.English_DB);
                database.CreateTable<Database_Words>();
                var search_occurrences = database.Query<Database_Words>("SELECT * FROM  " + DataBase.Table_Name);// поиск вхождения слова в БД
                var search_numberlearn_null = database.Query<Database_Words>("SELECT * FROM  " + DataBase.Table_Name + " WHERE NumberLearn = 0").Count;
                if (search_occurrences.Count == search_numberlearn_null)
                    Toast.MakeText(this, "You repeated all the words", ToastLength.Short).Show();
                else if (search_occurrences.Count != 0)
                {
                    Intent intent_english_repeat = new Intent(this, typeof(English_Repeat));
                    StartActivity(intent_english_repeat);
                }
                else
                    Toast.MakeText(this, "The database is empty", ToastLength.Short).Show();
            }
            catch { Toast.MakeText(this, "Error : can't connect to database", ToastLength.Long).Show(); }
        }

        [Android.Runtime.Register("onWindowFocusChanged", "(Z)V", "GetOnWindowFocusChanged_ZHandler")]
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            if (hasFocus)
            {
                FindViewById<Button>(Resource.Id.button_english_add).Enabled = true;
                FindViewById<Button>(Resource.Id.button_english_learn).Enabled = true;
                FindViewById<Button>(Resource.Id.button_english_repeat).Enabled = true;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {                  
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English);
            Toolbar toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglish);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            if (String.IsNullOrEmpty(DataBase.Table_Name))
                CrossSettings.Current.AddOrUpdateValue("DictionaryName", Table_name.My_Directly);
            DataBase.Table_Name = CrossSettings.Current.GetValueOrDefault("DictionaryName", null);
            DataBase.Update_English_DB();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_english, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {            
            int id = item.ItemId;
            if (id == Resource.Id.menuDatabase_MyDictionary)
            {
                DataBase.Table_Name = Table_name.My_Directly;
                CrossSettings.Current.AddOrUpdateValue("DictionaryName", DataBase.Table_Name);
                DataBase.Update_English_DB();

                return true;
            }
            if (id == Resource.Id.menuDatabase_PopularWords)
            {
                DataBase.Table_Name = Table_name.Popular_Words;
                CrossSettings.Current.AddOrUpdateValue("DictionaryName", DataBase.Table_Name);
                DataBase.Update_English_DB();
                return true;
            }
            if (id == Resource.Id.Stats){
                Intent intent_english_stat = new Intent(this, typeof(English_Stat));
                StartActivity(intent_english_stat);
                return true;
            }
            if(id == Resource.Id.Deleteword){
                Intent intent_english_add = new Intent(this, typeof(English_Delete));
                StartActivity(intent_english_add);
                return true;
            }
            if (id == Android.Resource.Id.Home)
                this.Finish(); 
            return true;
        }
    }
}