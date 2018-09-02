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
    [Activity(Label = "Repeat ", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags_Repeat : Activity
    {
        ImageView ImageView_image;
        Button Button1;
        Button Button2;
        Button Button3;
        Button Button4;
        Button Button_next;
        int Count = -1;
        int Rand_word { get; set; }
        readonly List<Statistics> Stats = new List<Statistics>();
        List<Database_images> dataBase;

        void Button_enable()
        {
            Button1.Enabled = false;
            Button2.Enabled = false;
            Button3.Enabled = false;
            Button4.Enabled = false;
            Button_next.Enabled = true;
        }

        void Button_Refresh()
        {
            Button1.Enabled = true; Button2.Enabled = true; Button3.Enabled = true; Button4.Enabled = true;
            Button1.Background = GetDrawable(Resource.Drawable.button_style_standard);
            Button2.Background = GetDrawable(Resource.Drawable.button_style_standard);
            Button3.Background = GetDrawable(Resource.Drawable.button_style_standard);
            Button4.Background = GetDrawable(Resource.Drawable.button_style_standard);
        }

        void Random_Button(Button B1, Button B2, Button B3, Button B4, int i)   //загружаем варианты ответа в текст кнопок
        {
            Additional_functions.Random_4_numbers(i, dataBase.Count, out List<int> random_numbers);
            B1.Text = Additional_functions.Name_of_the_flag(dataBase[random_numbers[0]]);
            B2.Text = Additional_functions.Name_of_the_flag(dataBase[random_numbers[1]]);
            B3.Text = Additional_functions.Name_of_the_flag(dataBase[random_numbers[2]]);
            B4.Text = Additional_functions.Name_of_the_flag(dataBase[random_numbers[3]]);
        }

        void Function_Next_Test(int rand_word) //new test
        {
            Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
            var his = Application.Context.Assets.Open("ImageFlags/" + dataBase[rand_word].Image_name+ ".png");
            Bitmap bitmap = BitmapFactory.DecodeStream(his);
            ImageView_image.SetImageBitmap(bitmap);
            switch (rnd.Next(4))
            {                            
                case 0:{
                    Random_Button(Button1, Button2, Button3, Button4, rand_word);
                    break;
                }
                case 1:{
                    Random_Button(Button2, Button1, Button3, Button4, rand_word);
                    break;
                }
                case 2:{
                    Random_Button(Button3, Button1, Button2, Button4, rand_word);
                    break;
                }
                case 3:{
                    Random_Button(Button4, Button1, Button2, Button3, rand_word);
                    break;
                }
            }
        }

        void Answer(Button B1, Button B2, Button B3, Button B4, Button BNext, int rand_word) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            Button_enable();
            if (B1.Text == Additional_functions.Name_of_the_flag(dataBase[rand_word]))
            {
                Additional_functions.Update_number_learn(Stats, Convert.ToString(dataBase[rand_word].Image_name), rand_word, dataBase[rand_word].NumberLearn -= Magic_constants.true_answer);
                Statistics.AnswerTrue++;
                B1.Background = GetDrawable(Resource.Drawable.button_true);
            }
            else
            {
                Additional_functions.Update_number_learn(Stats, Convert.ToString(dataBase[rand_word].Image_name), rand_word, dataBase[rand_word].NumberLearn += Magic_constants.false_answer);
                Statistics.AnswerFalse++;
                B1.Background = GetDrawable(Resource.Drawable.button_false);
                if (B2.Text == Additional_functions.Name_of_the_flag(dataBase[rand_word]))
                    B2.Background = GetDrawable(Resource.Drawable.button_true);
                else if (B3.Text == Additional_functions.Name_of_the_flag(dataBase[rand_word]))
                    B3.Background = GetDrawable(Resource.Drawable.button_true);
                else
                    B4.Background = GetDrawable(Resource.Drawable.button_true);
            }
        }

        public void Update_Database() // изменение у бвзы данных элемента NumberLearn
        {
            var database = DataBase.Connect(Database_Name.Flags_DB);
            int month = DateTime.Today.Month;
            for (int i = 0; i < Stats.Count; i++)
            {
                database.Query<Database_images>("UPDATE " + DataBase.Table_Name + " SET DateRecurrence = " + month + " WHERE Image_name = ?", Stats[i].Word);
                database.Query<Database_images>("UPDATE " + DataBase.Table_Name + " SET NumberLearn = " + Stats[i].Learn + " WHERE Image_name = ?", Stats[i].Word);
            }
        }

        [Java.Interop.Export("Button_Flags_1_Click")]
        public void Button_Flags_1_Click(View v) => Answer(Button1, Button2, Button3, Button4, Button_next, Rand_word);


        [Java.Interop.Export("Button_Flags_2_Click")]
        public void Button_Flags_2_Click(View v) => Answer(Button2, Button1, Button3, Button4, Button_next, Rand_word);


        [Java.Interop.Export("Button_Flags_3_Click")]
        public void Button_Flags_3_Click(View v) => Answer(Button3, Button1, Button2, Button4, Button_next, Rand_word);


        [Java.Interop.Export("Button_Flags_4_Click")]
        public void Button_Flags_4_Click(View v) => Answer(Button4, Button1, Button2, Button3, Button_next, Rand_word);


        [Java.Interop.Export("Button_Flags_Next_Click")]
        public void Button_Flags_Next_Click(View v)
        {
            Button_next.Enabled = false;
            if (Count < Magic_constants.repeat_count - 1)
            {
                Count++;
                Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                Rand_word = rnd.Next(dataBase.Count);
                Function_Next_Test(Rand_word);
                Button_Refresh();
                ActionBar.Title = Convert.ToString("Repeat " + (Count + 1) + "/" + Magic_constants.repeat_count);
            }
            else
            {
                Statistics.Add_Statistics(Statistics.AnswerTrue, Statistics.AnswerFalse);
                Update_Database();
                Intent intent_flags_stat = new Intent(this, typeof(Flags_Stats));
                StartActivity(intent_flags_stat);
                this.Finish();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags_Repeat);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarFlagsRepeat);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            Statistics.Statistics_update();
            ImageView_image = FindViewById<ImageView>(Resource.Id.imageView_Flags_repeat);
            Button1 = FindViewById<Button>(Resource.Id.button_F_choice1);
            Button2 = FindViewById<Button>(Resource.Id.button_F_choice2);
            Button3 = FindViewById<Button>(Resource.Id.button_F_choice3);
            Button4 = FindViewById<Button>(Resource.Id.button_F_choice4);
            Button_next = FindViewById<Button>(Resource.Id.button_F_Next);

            try
            {
                SQLiteConnection db = DataBase.Connect(Database_Name.Flags_DB);
                dataBase = db.Query<Database_images>("SELECT * FROM " + DataBase.Table_Name + " WHERE NumberLearn > 0");
                Button_Flags_Next_Click(null);
            }
            catch{
                Toast.MakeText(this, "Error : can't connect to database of flags", ToastLength.Long).Show();
            }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
}