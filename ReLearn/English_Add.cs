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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //setting layout
           
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Add);

            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglishAdd);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundEnFl));
            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(127, 0, 0, 0));
            GUI.button_default(English.button_english_add);
            //this.ActionBar.SetBackgroundDrawable(GetDrawable(Resource.Drawable.BackgroundActionBar));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            EditText editText_foreign_word = FindViewById<EditText>(Resource.Id.editText_foreign_word);
            EditText editText_translation_word = FindViewById<EditText>(Resource.Id.editText_translation_word);
            Button button_add_word = FindViewById<Button>(Resource.Id.button_add_word);
            button_add_word.Touch += GUI.TouchAdd;
            try
            {
                var database = DataBase.Connect(NameDatabase.English_DB);
                database.CreateTable<Database>();
                button_add_word.Click += (s, e) =>
                { // добавление элемента в БД
                    var search_occurrences = database.Query<Database>("SELECT * FROM Database WHERE enWords = ?", editText_foreign_word.Text);// поиск вхождения слова в БД
                    if (editText_foreign_word.Text == "" || editText_translation_word.Text == "")
                        Toast.MakeText(this, "Enter word!", ToastLength.Short).Show();
                    else if (search_occurrences.Count != 0)
                        Toast.MakeText(this, "The word exists!", ToastLength.Short).Show();
                    else
                    {
                        DataBase.Add_English_word(editText_foreign_word.Text, editText_translation_word.Text, database);
                        Toast.MakeText(this, "Word added!", ToastLength.Short).Show();
                    }
                    editText_foreign_word.Text = "";
                    editText_translation_word.Text = "";
                   
                };
            }
            catch{
                Toast.MakeText(this, "Error : can't connect to database of Language", ToastLength.Long).Show();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
}