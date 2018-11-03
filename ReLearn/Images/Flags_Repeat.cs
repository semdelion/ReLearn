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
        List<Button> Buttons { get; set; }
        ButtonNext Button_next;
        readonly List<Statistics> Stats = new List<Statistics>();
        List<Database_images> dataBase;
        
        int Count = -1;
        int CurrentWordNumber { get; set; }

        string TitleCount
        {
            set { FindViewById<TextView>(Resource.Id.Repeat_toolbar_textview_fl).Text = value; }
        }

        void Button_enable(bool state)
        {
            foreach (var button in Buttons) button.Enabled = state;
            if (state)
            {
                Button_next.State = StateButton.Unknown;
                Button_next.button.Text = GetString(Resource.String.Unknown);
                foreach (var button in Buttons) button.Background = GetDrawable(Resource.Drawable.button_style_standard);
            }
            else
            {
                Button_next.State = StateButton.Next;
                Button_next.button.Text = GetString(Resource.String.Next);
            }
        }

        void Random_Button(params Button[] buttons)   //загружаем варианты ответа в текст кнопок
        {
            Additional_functions.RandomFourNumbers(CurrentWordNumber, dataBase.Count, out List<int> random_numbers);
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].Text = Additional_functions.NameOfTheFlag(dataBase[random_numbers[i]]);
        }

        void NextTest() //new test
        {
            Bitmap bitmap = BitmapFactory.DecodeStream(Application.Context.Assets.Open(
                $"ImageFlags/{dataBase[CurrentWordNumber].Image_name}.png"));
            ImageView_image.SetImageBitmap(bitmap);
            const int four = 4;
            int first = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(four);
            List<int> random_numbers = new List<int> { first, 0, 0, 0 };
            for (int i = 1; i < four; i++)
                random_numbers[i] = (first + i) % four;
            Random_Button(Buttons[random_numbers[0]], Buttons[random_numbers[1]], Buttons[random_numbers[2]], Buttons[random_numbers[3]]);
        }

        void Answer(params Button[] buttons) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            Button_enable(false);
            if (buttons[0].Text == Additional_functions.NameOfTheFlag(dataBase[CurrentWordNumber]))
            {
                Additional_functions.UpdateNumberLearn(
                    Stats, Convert.ToString(dataBase[CurrentWordNumber].Image_name),
                    CurrentWordNumber, dataBase[CurrentWordNumber].NumberLearn -= Settings.TrueAnswer);
                Statistics.AnswerTrue++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_true);
            }
            else
            {
                Additional_functions.UpdateNumberLearn(
                    Stats, Convert.ToString(dataBase[CurrentWordNumber].Image_name), 
                    CurrentWordNumber, dataBase[CurrentWordNumber].NumberLearn += Settings.FalseAnswer);
                Statistics.AnswerFalse++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_false);
                for (int i = 1; i < buttons.Length; i++)
                    if (buttons[i].Text == Additional_functions.NameOfTheFlag(dataBase[CurrentWordNumber]))
                    {
                        buttons[i].Background = GetDrawable(Resource.Drawable.button_true);
                        return;
                    }
            }
        }

        void Unknown()
        {
            Statistics.AnswerFalse++;
            Additional_functions.UpdateNumberLearn(Stats, Convert.ToString(dataBase[CurrentWordNumber].Image_name), CurrentWordNumber, dataBase[CurrentWordNumber].NumberLearn += Settings.NeutralAnswer);
            for (int i = 0; i < Buttons.Count; i++)
                if (Buttons[i].Text == Additional_functions.NameOfTheFlag(dataBase[CurrentWordNumber]))
                {
                    Buttons[i].Background = GetDrawable(Resource.Drawable.button_true);
                    return;
                }
        }

        void Update_Database() // изменение у BD элемента NumberLearn
        {
            var database = DataBase.Connect(Database_Name.Flags_DB);         
            for (int i = 0; i < Stats.Count; i++)
            {
                var query = $"UPDATE {DataBase.TableNameImage} SET DateRecurrence = ?, NumberLearn = ? WHERE Image_name = ?";
                database.Execute(query, DateTime.Now, Stats[i].Learn, Stats[i].Word);
            }
        }

        [Java.Interop.Export("Button_Flags_1_Click")]
        public void Button_Flags_1_Click(View v) => Answer(Buttons[0], Buttons[1], Buttons[2], Buttons[3]);


        [Java.Interop.Export("Button_Flags_2_Click")]
        public void Button_Flags_2_Click(View v) => Answer(Buttons[1], Buttons[0], Buttons[2], Buttons[3]);


        [Java.Interop.Export("Button_Flags_3_Click")]
        public void Button_Flags_3_Click(View v) => Answer(Buttons[2], Buttons[0], Buttons[1], Buttons[3]);


        [Java.Interop.Export("Button_Flags_4_Click")]
        public void Button_Flags_4_Click(View v) => Answer(Buttons[3], Buttons[0], Buttons[1], Buttons[2]);

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
                if (Count < Settings.NumberOfRepeatsImage - 1)
                {
                    Count++;
                    Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                    CurrentWordNumber = rnd.Next(dataBase.Count);
                    NextTest();
                    Button_enable(true);
                    TitleCount = Convert.ToString(GetString(Resource.String.Repeat) + " " + (Count + 1) + "/" + Settings.NumberOfRepeatsImage);
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
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            Statistics.Statistics_update();
            ImageView_image = FindViewById<ImageView>(Resource.Id.imageView_Flags_repeat);
            Buttons = new List<Button>{
                FindViewById<Button>(Resource.Id.button_F_choice1),
                FindViewById<Button>(Resource.Id.button_F_choice2),
                FindViewById<Button>(Resource.Id.button_F_choice3),
                FindViewById<Button>(Resource.Id.button_F_choice4)
            };
            Button_next = new ButtonNext
            {
                button = FindViewById<Button>(Resource.Id.button_F_Next),
                State = StateButton.Next
            };
            try
            {
                SQLiteConnection db = DataBase.Connect(Database_Name.Flags_DB);
                dataBase = db.Query<Database_images>($"SELECT * FROM {DataBase.TableNameImage} WHERE NumberLearn > 0");
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