using System;
using System.IO;
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
    [Activity(Label = "Flags", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    
    class Flags : Activity
    {
        public static Button button_flags_learn;
        public static Button button_flags_repeat;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags);

            Toolbar toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarFlags);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundEnFl));
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(127, 0, 0, 0));

            button_flags_learn = FindViewById<Button>(Resource.Id.button_flags_learn);
            button_flags_repeat = FindViewById<Button>(Resource.Id.button_flags_repeat);
            button_flags_learn.Touch += GUI.NewTouch;
            button_flags_repeat.Touch += GUI.NewTouch;

            try
            {
                var databaseSetting = DataBase.Connect(NameDatabase.Setting_DB);
                databaseSetting.CreateTable<Setting_Database>();
                var table = databaseSetting.Table<Setting_Database>();
                var search_Setting = databaseSetting.Query<Setting_Database>("SELECT * FROM Setting_Database WHERE Setting_bd = 'flags'");
                if (search_Setting.Count != 0)
                    magic_constants.language = search_Setting[0].language;
                else
                    throw new Exception("Error : can't connect to setting database");
            }
            catch (Exception e){
                Android.Util.Log.Error(e.Message, e.ToString());
            }

            button_flags_learn.Click += delegate
            {
                GUI.button_click(button_flags_learn);
                Intent intent_flags_learn = new Intent(this, typeof(Flags_Learn));
                StartActivity(intent_flags_learn);
            };
            button_flags_repeat.Click += delegate
            {
                GUI.button_click(button_flags_repeat);
                Intent intent_flags_repeat = new Intent(this, typeof(Flags_Repeat));
                StartActivity(intent_flags_repeat);
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.menu_flags, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var databaseSetting = DataBase.Connect(NameDatabase.Setting_DB); // подключение к БД
            databaseSetting.CreateTable<Setting_Database>();            
            int id = item.ItemId;
            if (id == Resource.Id.Stats_Flags)
            {
                Intent intent_flags_stat = new Intent(this, typeof(Flags_Stats));
                StartActivity(intent_flags_stat);
                return true;
            }
            if (id == Resource.Id.language_eng)
            {
                databaseSetting.Query<Setting_Database>("UPDATE Setting_Database SET language = " + 0 + " WHERE Setting_bd = ?", "flags");
                magic_constants.language = 0;
                return true;
            }
            if (id == Resource.Id.language_rus)
            {
                databaseSetting.Query<Setting_Database>("UPDATE Setting_Database SET language = " + 1 + " WHERE Setting_bd = ?", "flags");
                magic_constants.language = 1;
                return true;
            }
            if (id == Android.Resource.Id.Home)
                this.Finish();
            return true;
        }
    }
    
}