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
using Android.Graphics;

namespace ReLearn
{
    [Activity(Label = "Learn", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags_Learn : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags_Learn);
            GUI.Button_default(Flags.button_flags_learn);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarFlagsLearn);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            TextView textView_learn_flag = FindViewById<TextView>(Resource.Id.textView_flag_learn);
            ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView_Flags_learn);
            Button button_learn_en_ru = FindViewById<Button>(Resource.Id.button_F_learn_Next);

            button_learn_en_ru.Touch += GUI.Button_Click;

            try
            {
                var db = DataBase.Connect(Database_Name.Flags_DB);
                db.CreateTable<Database_Flags>(); //
                List<Database_Flags> dataBase = new List<Database_Flags>();
                var table = db.Table<Database_Flags>();
                foreach (var word in table)
                { // создание БД в виде  List<DatabaseOfFlags>
                    Database_Flags w = new Database_Flags();
                    w.Add(word.Image_name, word.Name_flag_en, word.Name_flag_ru, word.NumberLearn, word.DateRecurrence);
                    dataBase.Add(w);
                }
                int rand_word = 0;
                Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                rand_word = rnd.Next(dataBase.Count);

                var his = Application.Context.Assets.Open("ImageFlags/" + dataBase[rand_word].Image_name + ".png");
                Bitmap bitmap = BitmapFactory.DecodeStream(his);
                imageView.SetImageBitmap(bitmap);

               // imageView.SetImageResource(dataBase[rand_word].Image_name);
                textView_learn_flag.Text = Repeat_work.Word_det(dataBase[rand_word]);

                button_learn_en_ru.Click += (s, e) =>
                {
                    rand_word = rnd.Next(dataBase.Count);

                    var hisi = Application.Context.Assets.Open("ImageFlags/" + dataBase[rand_word].Image_name + ".png");
                    Bitmap bitm = BitmapFactory.DecodeStream(hisi);
                    imageView.SetImageBitmap(bitm);

                    //imageView.SetImageResource(dataBase[rand_word].Image_name);
                    textView_learn_flag.Text = Repeat_work.Word_det(dataBase[rand_word]);
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