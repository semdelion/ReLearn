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
       
        int CurrentWordNumber { get; set; }
        List<Button> Buttons { get; set; }
        ButtonNext Button_next;
        List<DBImages> ImagesDatabase;
        
        string TitleCount
        {
            set => FindViewById<TextView>(Resource.Id.Repeat_toolbar_textview_fl).Text = value; 
        }

        Bitmap CurrentImage
        {
            set => FindViewById<ImageView>(Resource.Id.imageView_Flags_repeat).SetImageBitmap(value); 
        }

        void Button_enable(bool state)
        {
            foreach (var button in Buttons)
                button.Enabled = state;
            if (state)
            {
                Button_next.State = StateButton.Unknown;
                Button_next.button.Text = GetString(Resource.String.Unknown);
                foreach (var button in Buttons)
                    button.Background = GetDrawable(Resource.Drawable.button_style_standard);
            }
            else
            {
                Button_next.State = StateButton.Next;
                Button_next.button.Text = GetString(Resource.String.Next);
            }
        }

        void Random_Button(params Button[] buttons)   //загружаем варианты ответа в текст кнопок
        {
            AdditionalFunctions.RandomFourNumbers(CurrentWordNumber, ImagesDatabase.Count, out List<int> random_numbers);
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].Text = AdditionalFunctions.NameOfTheFlag(ImagesDatabase[random_numbers[i]]);
        }

        void NextTest() //new test
        {
            using (Bitmap bitmap = BitmapFactory.DecodeStream(Application.Context.Assets.Open(
                $"Image{DataBase.TableNameImage}/{ImagesDatabase[CurrentWordNumber].Image_name}.png")))
                CurrentImage = bitmap;
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
            if (buttons[0].Text == AdditionalFunctions.NameOfTheFlag(ImagesDatabase[CurrentWordNumber]))
            {
                Statistics.Add(ImagesDatabase, CurrentWordNumber, -Settings.TrueAnswer);
                Statistics.True++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_true);
            }
            else
            {
                Statistics.Add(ImagesDatabase,CurrentWordNumber, Settings.FalseAnswer);
                Statistics.False++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_false);
                for (int i = 1; i < buttons.Length; i++)
                    if (buttons[i].Text == AdditionalFunctions.NameOfTheFlag(ImagesDatabase[CurrentWordNumber]))
                    {
                        buttons[i].Background = GetDrawable(Resource.Drawable.button_true);
                        return;
                    }
            }
        }

        void Unknown()
        {
            Statistics.False++;
            Statistics.Add(ImagesDatabase, CurrentWordNumber, Settings.NeutralAnswer);
            for (int i = 0; i < Buttons.Count; i++)
                if (Buttons[i].Text == AdditionalFunctions.NameOfTheFlag(ImagesDatabase[CurrentWordNumber]))
                {
                    Buttons[i].Background = GetDrawable(Resource.Drawable.button_true);
                    return;
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
                if (Statistics.Count < Settings.NumberOfRepeatsImage - 1)
                {
                    if (v != null)
                        Statistics.Count++;
                    CurrentWordNumber = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(ImagesDatabase.Count);
                    NextTest();
                    Button_enable(true);
                    TitleCount = $"{GetString(Resource.String.Repeat)} {Statistics.Count + 1}/{Settings.NumberOfRepeatsImage}";
                }
                else
                {
                    DBStatistics.Insert(Statistics.True, Statistics.False, DataBase.TableNameImage.ToString());
                    Statistics.Count = Statistics.True = Statistics.False = 0;
                    StartActivity(typeof(Flags_Stats));
                    this.Finish();
                }
            }
            Button_next.button.Enabled = true;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags_Repeat);
            
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarFlagsRepeat);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ImagesDatabase = DBImages.GetDataNotLearned;
            Statistics.Table = DataBase.TableNameImage.ToString();
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
            if (ImagesDatabase.Count == 0)
            {
                Toast.MakeText(this, GetString(Resource.String.RepeatedAllImages), ToastLength.Short).Show();
                Finish();
                return;
            }
           
            Statistics.Table = DataBase.TableNameImage.ToString();
            Button_Flags_Next_Click(null);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
}