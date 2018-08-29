using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace ReLearn
{
    [Activity(Label = "Learn", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class English_Learn : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //setting layout
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Learn);
            GUI.Button_default(English.button_english_learn);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglishLearn);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            //this.ActionBar.SetBackgroundDrawable(GetDrawable(Resource.Drawable.BackgroundActionBar));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            TextView textView_learn_en = FindViewById<TextView>(Resource.Id.textView_learn_en);
            TextView textView_learn_ru = FindViewById<TextView>(Resource.Id.textView_learn_ru);
            Button button_learn_en_ru = FindViewById<Button>(Resource.Id.button_learn_en_ru);
            button_learn_en_ru.Touch += GUI.Button_Click;

            try
            {
                var db = DataBase.Connect(NameDatabase.English_DB);
                var dataBase = db.Query<Database_Words>("SELECT * FROM " + DataBase.Table_Name);

                Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                int rand_word = 0;

                rand_word = rnd.Next(dataBase.Count);  ////TODO говнокод
                textView_learn_en.Text = dataBase[rand_word].enWords;
                textView_learn_ru.Text = dataBase[rand_word].ruWords;

                button_learn_en_ru.Click += (s, e) =>
                {                  
                    rand_word = rnd.Next(dataBase.Count);
                    textView_learn_en.Text = dataBase[rand_word].enWords;
                    textView_learn_ru.Text = dataBase[rand_word].ruWords;
                };
            }
            catch{
                Toast.MakeText(this, "Error : can't connect to database of Language in Learn", ToastLength.Long).Show();
            }
        }     

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
    
}