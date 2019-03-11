using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.Helpers;
using ReLearn.Core.ViewModels.Images;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Views.Facade;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Droid.Images
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RepeatActivity : MvxAppCompatActivityRepeat<RepeatViewModel>
    {
        protected override void RandomButton(params Button[] buttons)   //загружаем варианты ответа в текст кнопок
        {
            RandomNumbers.RandomFourNumbers(ViewModel.CurrentNumber, ViewModel.Database.Count, out List<int> random_numbers);
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].Text = ViewModel.Database[random_numbers[i]].ImageName;
        }

        protected override void NextTest() //new test
        {
            using (Bitmap bitmap = BitmapFactory.DecodeStream(Application.Context.Assets.Open(
                $"Image{DataBase.TableName}/{ViewModel.Database[ViewModel.CurrentNumber].Image_name}.png")))
            using (var bitmapRounded = BitmapHelper.GetRoundedCornerBitmap(bitmap, PixelConverter.DpToPX(5)))
                FindViewById<ImageView>(Resource.Id.imageView_Images_repeat).SetImageBitmap(bitmapRounded);

            const int four = 4;
            int first = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(four);
            List<int> random_numbers = new List<int> { first, 0, 0, 0 };
            for (int i = 1; i < four; i++)
                random_numbers[i] = (first + i) % four;
            RandomButton(Buttons[random_numbers[0]], Buttons[random_numbers[1]], Buttons[random_numbers[2]], Buttons[random_numbers[3]]);
        }

        protected override async Task Answer(params Button[] buttons) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            API.Statistics.Count++;
            ButtonEnable(false);
            if (buttons[0].Text == ViewModel.Database[ViewModel.CurrentNumber].ImageName)
            {
                await API.Statistics.Add(ViewModel.Database, ViewModel.CurrentNumber, - Settings.TrueAnswer);
                API.Statistics.True++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_true);
            }
            else
            {
                await API.Statistics.Add(ViewModel.Database, ViewModel.CurrentNumber, Settings.FalseAnswer);
                API.Statistics.False++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_false);
                int index = Buttons.FindIndex(s => s.Text == ViewModel.Database[ViewModel.CurrentNumber].ImageName);
                Buttons[index].Background = GetDrawable(Resource.Drawable.button_true);
            }
        }

        protected override async Task Unknown()
        {
            API.Statistics.Count++;
            API.Statistics.False++;
            await API.Statistics.Add(ViewModel.Database, ViewModel.CurrentNumber, Settings.NeutralAnswer);
            int index = Buttons.FindIndex(s => s.Text == ViewModel.Database[ViewModel.CurrentNumber].ImageName);
            Buttons[index].Background = GetDrawable(Resource.Drawable.button_true);
        }

        [Java.Interop.Export("Button_Images_1_Click")]
        public async void Button_Images_1_Click(View v) => await Answer(Buttons[0], Buttons[1], Buttons[2], Buttons[3]);

        [Java.Interop.Export("Button_Images_2_Click")]
        public async void Button_Images_2_Click(View v) => await Answer(Buttons[1], Buttons[0], Buttons[2], Buttons[3]);

        [Java.Interop.Export("Button_Images_3_Click")]
        public async void Button_Images_3_Click(View v) => await Answer(Buttons[2], Buttons[0], Buttons[1], Buttons[3]);

        [Java.Interop.Export("Button_Images_4_Click")]
        public async void Button_Images_4_Click(View v) => await Answer(Buttons[3], Buttons[0], Buttons[1], Buttons[2]);

        [Java.Interop.Export("Button_Images_Next_Click")]
        public async void Button_Images_Next_Click(View v)
        {
            ButtonNext.button.Enabled = false;
            if (ButtonNext.State == StateButton.Unknown)
            {
                ButtonNext.State = StateButton.Next;
                ButtonEnable(false);
                await Unknown();
            }
            else
            {
                if (API.Statistics.Count < Settings.NumberOfRepeatsImage)
                {
                    ViewModel.CurrentNumber = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(ViewModel.Database.Count);
                    NextTest();
                    ButtonEnable(true);
                    ViewModel.TitleCount = $"{GetString(Resource.String.Repeated)} {API.Statistics.Count + 1}/{Settings.NumberOfRepeatsImage}";
                }
                else
                {
                    await DBStatistics.Insert(API.Statistics.True, API.Statistics.False, $"{DataBase.TableName}");
                    API.Statistics.Delete();
                    ViewModel.ToStatistic.Execute();
                    this.Finish();
                }
            }
            ButtonNext.button.Enabled = true;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_images_repeat);
            
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_images_repeat);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            using (var background = BitmapHelper.GetBackgroung(Resources, _displayWidth - PixelConverter.DpToPX(20), PixelConverter.DpToPX(190)))
                FindViewById<LinearLayout>(Resource.Id.repeat_background).Background = background;

            Buttons = new List<Button>{
                FindViewById<Button>(Resource.Id.button_I_choice1),
                FindViewById<Button>(Resource.Id.button_I_choice2),
                FindViewById<Button>(Resource.Id.button_I_choice3),
                FindViewById<Button>(Resource.Id.button_I_choice4)
            };
            ButtonNext = new ButtonNext
            {
                button = FindViewById<Button>(Resource.Id.button_I_Next),
                State = StateButton.Next
            };
            Button_Images_Next_Click(null);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}