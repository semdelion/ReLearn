using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Support.V7.App;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels.Images;
using Android.Util;
using Android.Graphics.Drawables;

namespace ReLearn.Droid.Images
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RepeatActivity : MvxAppCompatActivity<RepeatViewModel>
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
            set => FindViewById<ImageView>(Resource.Id.imageView_Images_repeat).SetImageBitmap(value); 
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
                buttons[i].Text = ImagesDatabase[random_numbers[i]].ImageName;
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
            Statistics.Count++;
            Button_enable(false);
            if (buttons[0].Text == ImagesDatabase[CurrentWordNumber].ImageName)
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
                int index = Buttons.FindIndex(s => s.Text == ImagesDatabase[CurrentWordNumber].ImageName);
                Buttons[index].Background = GetDrawable(Resource.Drawable.button_true);
            }
        }

        void Unknown()
        {
            Statistics.Count++;
            Statistics.False++;
            Statistics.Add(ImagesDatabase, CurrentWordNumber, Settings.NeutralAnswer);
            int index = Buttons.FindIndex(s => s.Text == ImagesDatabase[CurrentWordNumber].ImageName);
            Buttons[index].Background = GetDrawable(Resource.Drawable.button_true);
        }

        [Java.Interop.Export("Button_Images_1_Click")]
        public void Button_Images_1_Click(View v) => Answer(Buttons[0], Buttons[1], Buttons[2], Buttons[3]);

        [Java.Interop.Export("Button_Images_2_Click")]
        public void Button_Images_2_Click(View v) => Answer(Buttons[1], Buttons[0], Buttons[2], Buttons[3]);

        [Java.Interop.Export("Button_Images_3_Click")]
        public void Button_Images_3_Click(View v) => Answer(Buttons[2], Buttons[0], Buttons[1], Buttons[3]);

        [Java.Interop.Export("Button_Images_4_Click")]
        public void Button_Images_4_Click(View v) => Answer(Buttons[3], Buttons[0], Buttons[1], Buttons[2]);

        [Java.Interop.Export("Button_Images_Next_Click")]
        public void Button_Images_Next_Click(View v)
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
                    CurrentWordNumber = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(ImagesDatabase.Count);
                    NextTest();
                    Button_enable(true);
                    TitleCount = $"{GetString(Resource.String.Repeated)} {Statistics.Count + 1}/{Settings.NumberOfRepeatsImage}";
                }
                else
                {
                    DBStatistics.Insert(Statistics.True, Statistics.False, DataBase.TableNameImage.ToString());
                    Statistics.Count = Statistics.True = Statistics.False = 0;
                    ViewModel.ToStatistic.Execute();
                    this.Finish();
                }
            }
            Button_next.button.Enabled = true;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ImagesRepeatActivity);
            
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarImagesRepeat);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ImagesDatabase = DBImages.GetDataNotLearned;

            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);
            var _background = new BitmapDrawable(Resources, Background.GetBackgroung(
            displayMetrics.WidthPixels - AdditionalFunctions.DpToPX(20),
            AdditionalFunctions.DpToPX(190)));
            FindViewById<LinearLayout>(Resource.Id.repeat_background).Background = _background;


            Statistics.Table = DataBase.TableNameImage.ToString();
            Buttons = new List<Button>{
                FindViewById<Button>(Resource.Id.button_I_choice1),
                FindViewById<Button>(Resource.Id.button_I_choice2),
                FindViewById<Button>(Resource.Id.button_I_choice3),
                FindViewById<Button>(Resource.Id.button_I_choice4)
            };
            Button_next = new ButtonNext
            {
                button = FindViewById<Button>(Resource.Id.button_I_Next),
                State = StateButton.Next
            };
            if (ImagesDatabase.Count == 0)
            {
                Toast.MakeText(this, GetString(Resource.String.RepeatedAllImages), ToastLength.Short).Show();
                Finish();
                return;
            }
           
            Statistics.Table = DataBase.TableNameImage.ToString();
            Button_Images_Next_Click(null);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}