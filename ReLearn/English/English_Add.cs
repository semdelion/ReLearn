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
using System.Threading;

namespace ReLearn
{
    
    [Activity(Label = "Word Add", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class English_Add : Activity
    {
      
        [Java.Interop.Export("Button_English_Add_Word_Click")]
        public void Button_English_Add_Click(View v)
        {
            v.Enabled = false;
            try
            {
                EditText editText_foreign_word = FindViewById<EditText>(Resource.Id.editText_foreign_word);
                EditText editText_translation_word = FindViewById<EditText>(Resource.Id.editText_translation_word);
                var database = DataBase.Connect(Database_Name.English_DB);
                database.CreateTable<Database_Words>();
                var search_occurrences = database.Query<Database_Words>("SELECT * FROM My_Directly WHERE Word = ?", editText_foreign_word.Text);// поиск вхождения слова в БД
                if (editText_foreign_word.Text == "" || editText_translation_word.Text == "")
                    Toast.MakeText(this, "Enter word!", ToastLength.Short).Show();
                else if (search_occurrences.Count != 0)
                    Toast.MakeText(this, "The word exists!", ToastLength.Short).Show();
                else
                {
                    database.Query<Database_Words>($"INSERT INTO My_Directly " +
                        $"(Word, TranslationWord, NumberLearn, DateRecurrence) VALUES ("
                        + "\"" + editText_foreign_word.Text.ToLower() + "\"" + ","
                        + "\"" + editText_translation_word.Text.ToLower() + "\"" + ","
                        + Magic_constants.numberLearn + ", DATETIME('NOW'))");

                    Toast.MakeText(this, "Word added!", ToastLength.Short).Show();
                }
                editText_foreign_word.Text = "";
                editText_translation_word.Text = "";
                v.Enabled = true;
            }
            catch
            {
                Toast.MakeText(this, "Error : can't connect to database of Language", ToastLength.Long).Show();
            }
        }

        [Android.Runtime.Register("onWindowFocusChanged", "(Z)V", "GetOnWindowFocusChanged_ZHandler")]
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            if (hasFocus)
                FindViewById<Button>(Resource.Id.button_add_word).Enabled = true;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Add);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglishAdd);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
}