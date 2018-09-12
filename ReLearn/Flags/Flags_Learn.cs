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
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags_Learn : Activity
    {
        ImageView imageView;
        TextView textView_learn_flag;

        [Java.Interop.Export("Button_Flags_Learn_Next_Click")]
        public void Button_Flags_Learn_Click(View v)
        {
            Following_Random_Image();
        }

        public void Following_Random_Image()
        {
            try
            {
                var db = DataBase.Connect(Database_Name.Flags_DB);        
                var dataBase = db.Query<Database_images>("SELECT * FROM " + DataBase.Table_Name);
                Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                int rand_word = rnd.Next(dataBase.Count);

                var his = Application.Context.Assets.Open("ImageFlags/" + dataBase[rand_word].Image_name + ".png");
                Bitmap bitmap = BitmapFactory.DecodeStream(his);
                imageView.SetImageBitmap(bitmap);

                textView_learn_flag.Text = Additional_functions.Name_of_the_flag(dataBase[rand_word]);
            }
            catch
            {
                Toast.MakeText(this, Additional_functions.GetResourceString("databaseNotConnect", this.Resources), ToastLength.Short).Show();             
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags_Learn);

            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarFlagsLearn);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            textView_learn_flag = FindViewById<TextView>(Resource.Id.textView_flag_learn);
            imageView = FindViewById<ImageView>(Resource.Id.imageView_Flags_learn);

            Following_Random_Image();        
        }

        public override bool OnOptionsItemSelected(IMenuItem item) // button home
        {
            this.Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
}