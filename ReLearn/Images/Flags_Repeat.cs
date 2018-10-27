using System;
using System.Collections.Generic;
using SQLite;
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
    class Flags_Repeat : AppCompatActivity
    {
        ImageView ImageView_image;
        Button Button1;
        Button Button2;
        Button Button3;
        Button Button4;
        ButtonNext Button_next;
        readonly List<Statistics> Stats = new List<Statistics>();
        List<Database_images> dataBase;
        TextView Title_textView;
        int Count = -1;
        int CurrentWordNumber { get; set; }

        void Button_enable(bool state)
        {
            Button1.Enabled = Button2.Enabled = Button3.Enabled = Button4.Enabled = state;
            if (state)
            {
                Button_next.State = StateButton.Unknown;
                Button_next.button.Text = GetString(Resource.String.Unknown);
                Button1.Background = GetDrawable(Resource.Drawable.button_style_standard);
                Button2.Background = GetDrawable(Resource.Drawable.button_style_standard);
                Button3.Background = GetDrawable(Resource.Drawable.button_style_standard);
                Button4.Background = GetDrawable(Resource.Drawable.button_style_standard);
            }
            else
            {
                Button_next.State = StateButton.Next;
                Button_next.button.Text = GetString(Resource.String.Next);
            }
        }

        void Random_Button(Button B1, Button B2, Button B3, Button B4)   //загружаем варианты ответа в текст кнопок
        {
            Additional_functions.RandomFourNumbers(CurrentWordNumber, dataBase.Count, out List<int> random_numbers);
            B1.Text = Additional_functions.NameOfTheFlag(dataBase[random_numbers[0]]);
            B2.Text = Additional_functions.NameOfTheFlag(dataBase[random_numbers[1]]);
            B3.Text = Additional_functions.NameOfTheFlag(dataBase[random_numbers[2]]);
            B4.Text = Additional_functions.NameOfTheFlag(dataBase[random_numbers[3]]);
        }

        void NextTest() //new test
        {
            Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
            var his = Application.Context.Assets.Open("ImageFlags/" + dataBase[CurrentWordNumber].Image_name+ ".png");
            Bitmap bitmap = BitmapFactory.DecodeStream(his);
            ImageView_image.SetImageBitmap(bitmap);
            switch (rnd.Next(4))
            {                            
                case 0:{
                    Random_Button(Button1, Button2, Button3, Button4);
                    break;
                }
                case 1:{
                    Random_Button(Button2, Button1, Button3, Button4);
                    break;
                }
                case 2:{
                    Random_Button(Button3, Button1, Button2, Button4);
                    break;
                }
                case 3:{
                    Random_Button(Button4, Button1, Button2, Button3);
                    break;
                }
            }
        }

        void Answer(Button B1, Button B2, Button B3, Button B4) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            Button_enable(false);
            if (B1.Text == Additional_functions.NameOfTheFlag(dataBase[CurrentWordNumber]))
            {
                Additional_functions.UpdateNumberLearn(Stats, Convert.ToString(dataBase[CurrentWordNumber].Image_name), CurrentWordNumber, dataBase[CurrentWordNumber].NumberLearn -= Magic_constants.TrueAnswer);
                Statistics.AnswerTrue++;
                B1.Background = GetDrawable(Resource.Drawable.button_true);
            }
            else
            {
                Additional_functions.UpdateNumberLearn(Stats, Convert.ToString(dataBase[CurrentWordNumber].Image_name), CurrentWordNumber, dataBase[CurrentWordNumber].NumberLearn += Magic_constants.FalseAnswer);
                Statistics.AnswerFalse++;
                B1.Background = GetDrawable(Resource.Drawable.button_false);
                if (B2.Text == Additional_functions.NameOfTheFlag(dataBase[CurrentWordNumber]))
                    B2.Background = GetDrawable(Resource.Drawable.button_true);
                else if (B3.Text == Additional_functions.NameOfTheFlag(dataBase[CurrentWordNumber]))
                    B3.Background = GetDrawable(Resource.Drawable.button_true);
                else
                    B4.Background = GetDrawable(Resource.Drawable.button_true);
            }
        }

        void Unknown()
        {
            Button tmp;
            if (Button1.Text == Additional_functions.NameOfTheFlag(dataBase[CurrentWordNumber])) tmp = Button1;
            else if (Button2.Text == Additional_functions.NameOfTheFlag(dataBase[CurrentWordNumber])) tmp = Button2;
            else if (Button3.Text == Additional_functions.NameOfTheFlag(dataBase[CurrentWordNumber])) tmp = Button3;
            else tmp = Button4;
            Statistics.AnswerFalse++;
            Additional_functions.UpdateNumberLearn(Stats, Convert.ToString(dataBase[CurrentWordNumber].Image_name), CurrentWordNumber, dataBase[CurrentWordNumber].NumberLearn += Magic_constants.NeutralAnswer);
            tmp.Background = GetDrawable(Resource.Drawable.button_true);
        }

        void Update_Database() // изменение у BD элемента NumberLearn
        {
            var database = DataBase.Connect(Database_Name.Flags_DB);         
            for (int i = 0; i < Stats.Count; i++)
            {
                database.Query<Database_images>("UPDATE " + DataBase.TableNameImage + " SET DateRecurrence = DATETIME('NOW') WHERE Image_name = ?", Stats[i].Word);
                database.Query<Database_images>("UPDATE " + DataBase.TableNameImage + " SET NumberLearn = " + Stats[i].Learn + " WHERE Image_name = ?", Stats[i].Word);
            }
        }

        [Java.Interop.Export("Button_Flags_1_Click")]
        public void Button_Flags_1_Click(View v) => Answer(Button1, Button2, Button3, Button4);


        [Java.Interop.Export("Button_Flags_2_Click")]
        public void Button_Flags_2_Click(View v) => Answer(Button2, Button1, Button3, Button4);


        [Java.Interop.Export("Button_Flags_3_Click")]
        public void Button_Flags_3_Click(View v) => Answer(Button3, Button1, Button2, Button4);


        [Java.Interop.Export("Button_Flags_4_Click")]
        public void Button_Flags_4_Click(View v) => Answer(Button4, Button1, Button2, Button3);


        [Java.Interop.Export("Button_Flags_Next_Click")]
        public void Button_Flags_Next_Click(View v)
        {
            Button_next.button.Enabled = false;
            if (Button_next.State == StateButton.Unknown)
            {
                Button_next.State = StateButton.Next;
                Button_enable(false);
                Unknown();
            }
            else
            {
                if (Count < Magic_constants.NumberOfRepeatsImage - 1)
                {
                    Count++;
                    Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                    CurrentWordNumber = rnd.Next(dataBase.Count);
                    NextTest();
                    Button_enable(true);
                    Title_textView.Text = Convert.ToString(GetString(Resource.String.Repeat) + " " + (Count + 1) + "/" + Magic_constants.NumberOfRepeatsImage);
                }
                else
                {
                    Statistics.Add_Statistics(Statistics.AnswerTrue, Statistics.AnswerFalse, DataBase.TableNameImage.ToString());
                    Update_Database();
                    Intent intent_flags_stat = new Intent(this, typeof(Flags_Stats));
                    StartActivity(intent_flags_stat);
                    this.Finish();
                }
            }
            Button_next.button.Enabled = true;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags_Repeat);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarFlagsRepeat);
            Title_textView = toolbarMain.FindViewById<TextView>(Resource.Id.Repeat_toolbar_textview_fl);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            Statistics.Statistics_update();
            ImageView_image = FindViewById<ImageView>(Resource.Id.imageView_Flags_repeat);
            Button1 = FindViewById<Button>(Resource.Id.button_F_choice1);
            Button2 = FindViewById<Button>(Resource.Id.button_F_choice2);
            Button3 = FindViewById<Button>(Resource.Id.button_F_choice3);
            Button4 = FindViewById<Button>(Resource.Id.button_F_choice4);
            Button_next = new ButtonNext
            {
                button = FindViewById<Button>(Resource.Id.button_F_Next),
                State = StateButton.Next
            };
            try
            {
                SQLiteConnection db = DataBase.Connect(Database_Name.Flags_DB);
                dataBase = db.Query<Database_images>("SELECT * FROM " + DataBase.TableNameImage + " WHERE NumberLearn > 0");
                Button_Flags_Next_Click(null);
            }
            catch
            {
                Toast.MakeText(this, GetString(Resource.String.DatabaseNotConnect), ToastLength.Short).Show();              
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
}