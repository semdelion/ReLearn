using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SQLite;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReLearn
{
    [Activity(Label = "Learn", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags_Learn : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //setting layout
              
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags_Learn);

            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarFlagsLearn);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            Window.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.backgroundEnFl));

            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(127, 0, 0, 0));

            GUI.button_default(Flags.button_flags_learn);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            TextView textView_learn_flag = FindViewById<TextView>(Resource.Id.textView_flag_learn);
            ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView_Flags_learn);
            Button button_learn_en_ru = FindViewById<Button>(Resource.Id.button_F_learn_Next);
            button_learn_en_ru.Touch += GUI.TouchAdd;
            

            try
            {
                var db = DataBase.Connect(NameDatabase.Flags_DB);
                db.CreateTable<Database_Flags>(); //
                List<DatabaseOfFlags> dataBase = new List<DatabaseOfFlags>();
                var table = db.Table<Database_Flags>();
                foreach (var word in table)
                { // создание БД в виде  List<DatabaseOfFlags>
                    DatabaseOfFlags w = new DatabaseOfFlags();
                    w.Add(word.image_name, word.name_flag_en, word.name_flag_ru, word.numberLearn, word.dateRepeat);
                    dataBase.Add(w);
                }
                int rand_word = 0;
                Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                rand_word = rnd.Next(dataBase.Count);

                imageView.SetImageResource(dataBase[rand_word].image_name);
                textView_learn_flag.Text = Repeat_work.word_det(dataBase[rand_word]);

                button_learn_en_ru.Click += (s, e) =>
                {
                    rand_word = rnd.Next(dataBase.Count);
                    imageView.SetImageResource(dataBase[rand_word].image_name);
                    textView_learn_flag.Text = Repeat_work.word_det(dataBase[rand_word]);
                };
            }
            catch {
                Toast.MakeText(this, "Error : can't connect to database of flags", ToastLength.Long).Show();
            }

        }

        public override bool OnOptionsItemSelected(IMenuItem item) // button home
        {
            this.Finish();
            return true;
        }
    }
}