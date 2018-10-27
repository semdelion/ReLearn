using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Support.V7.App;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags_Learn : AppCompatActivity
    {
        ImageView imageView;

        string ImageName
        {
            get { return FindViewById<TextView>(Resource.Id.textView_flag_learn).Text; }
            set { FindViewById<TextView>(Resource.Id.textView_flag_learn).Text = value; }
        }

        [Java.Interop.Export("Button_Flags_Learn_Next_Click")]
        public void Button_Flags_Learn_Click(View v) => RandomImage();
        
        public void RandomImage()
        {
            try
            {
                var db = DataBase.Connect(Database_Name.Flags_DB);        
                var dataBase = db.Query<Database_images>("SELECT * FROM " + DataBase.TableNameImage);
                Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                int rand_word = rnd.Next(dataBase.Count);

                var his = Application.Context.Assets.Open("ImageFlags/" + dataBase[rand_word].Image_name + ".png");
                Bitmap bitmap = BitmapFactory.DecodeStream(his);
                imageView.SetImageBitmap(bitmap);

                ImageName = Additional_functions.NameOfTheFlag(dataBase[rand_word]);
            }
            catch
            {
                Toast.MakeText(this, GetString(Resource.String.DatabaseNotConnect), ToastLength.Short).Show();             
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags_Learn);

            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarFlagsLearn);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            imageView = FindViewById<ImageView>(Resource.Id.imageView_Flags_learn);
            RandomImage();        
        }

        public override bool OnOptionsItemSelected(IMenuItem item) // button home
        {
            this.Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
}