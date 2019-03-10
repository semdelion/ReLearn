using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Animation;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Images;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Views.Facade;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace ReLearn.Droid.Images
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class BlitzPollActivity : MvxAppCompatActivityBlitzPoll<BlitzPollViewModel>
    {
        private LinearLayout ViewPrev { get; set; }
        private LinearLayout ViewCurrent { get; set; }

        TextView GetTextView()
        {
            var param = PixelConverter.GetParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent, 10, 0, 10, 20);
            int randIndex = (ViewModel.CurrentNumber + new Random(unchecked((int)(DateTime.Now.Ticks))).Next(1, ViewModel.Database.Count)) % ViewModel.Database.Count;
            var textView = new TextView(this)
            {
                TextSize = 20,
                LayoutParameters = param,
                Text = $"{(ViewModel.Database[ViewModel.Answer ? ViewModel.CurrentNumber : randIndex]).ImageName}",
                Gravity = GravityFlags.CenterHorizontal
            };
            textView.SetTextColor(Colors.White);
            return textView;
        }

        ImageView GetImage()
        {
            var param = PixelConverter.GetParams(ViewGroup.LayoutParams.MatchParent, PixelConverter.DpToPX(200), 10, 15, 10, 20);
            var ImageView = new ImageView(this) {LayoutParameters = param};
            using (Bitmap bitmap = BitmapFactory.DecodeStream(Application.Context.Assets.Open($"Image{DataBase.TableName}/{ViewModel.Database[ViewModel.CurrentNumber].Image_name}.png")))
            using (var bitmapRounded = BitmapHelper.GetRoundedCornerBitmap(bitmap, PixelConverter.DpToPX(5)))
                ImageView.SetImageBitmap(bitmapRounded);
            return ImageView;
        }

        LinearLayout GetLayout()
        {
            ViewModel.CurrentNumber = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(ViewModel.Database.Count);
            ViewModel.Answer = new Random(unchecked((int)DateTime.Now.Ticks)).Next(2) == 1 ? true : false;
            var param = PixelConverter.GetParamsRelative(ViewGroup.LayoutParams.MatchParent, PixelConverter.DpToPX(320), 10, 160, 10, 10);
            var linearLayout = new LinearLayout(this)
            {
                Orientation     = Orientation.Vertical,
                LayoutParameters = param,
            };
            linearLayout.Background = BackgroundWord;
            linearLayout.AddView(GetImage());
            linearLayout.AddView(GetTextView());
            return linearLayout;
        }
        
        public override async Task Answer(bool UserAnswer)
        {
            if (!(ViewModel.Answer ^ UserAnswer))
                ViewModel.True++;
            else
                ViewModel.False++;
            if (ViewPrev!=null)
                FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).RemoveView(ViewPrev);
            ViewCurrent.Background = GetDrawable(!(ViewModel.Answer ^ UserAnswer) ? Resource.Drawable.view_true : Resource.Drawable.view_false);
            RunAnimation(ViewCurrent,(UserAnswer ? 1 : -1) * PixelConverter.DpToPX(5000));
            ViewPrev = ViewCurrent;
            ViewCurrent = GetLayout();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).AddView(ViewCurrent, 0);
            await API.Statistics.Add(ViewModel.Database, ViewModel.CurrentNumber, !(ViewModel.Answer ^ UserAnswer) ? -1 : 1);
            ViewModel.TitleCount = $"{GetString(Resource.String.Repeated)} {ViewModel.True + ViewModel.False + 1 }";
        }

        void TimerStart()
        {
            ViewModel.Timer = new Timer { Interval = 100, Enabled = true };
            ViewModel.Timer.Elapsed += TimerElapsed;
            ViewModel.Timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(async () =>
            {
                if (ViewModel.Time > 0)
                {
                    ViewModel.Time--;
                    ViewModel.TimerText = $"{ViewModel.Time / 10}:{ViewModel.Time % 10}0";
                    if (ViewModel.Time == 50)
                        FindViewById<TextView>(Resource.Id.textView_Timer_Images).SetTextColor(Colors.Red);
                }
                else
                {
                    await DBStatistics.Insert(ViewModel.True, ViewModel.False, $"{DataBase.TableName}");
                    ViewModel.ToStatistic.Execute();
                    ViewModel.Cancel();
                    Finish();
                }
            });
        }


        [Java.Interop.Export("Button_Images_No_Click")]
        public async void Button_Images_No_Click(View v) => await Answer(false);
        
        [Java.Interop.Export("Button_Images_Yes_Click")]
        public async void Button_Images_Yes_Click(View v) => await Answer(true);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_images_blitz_poll);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_images_blitz_poll);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);

            BackgroundWord = BitmapHelper.GetBackgroung(Resources, displayMetrics.WidthPixels - PixelConverter.DpToPX(20), PixelConverter.DpToPX(300));
            using (var background = BitmapHelper.GetBackgroung(Resources, displayMetrics.WidthPixels - PixelConverter.DpToPX(200),PixelConverter.DpToPX(50)))
                FindViewById<TextView>(Resource.Id.textView_Timer_Images).Background = background;

            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).Touch += Swipes;
            
            ViewCurrent = GetLayout();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).AddView(ViewCurrent, 1);
            ViewModel.TitleCount = $"{GetString(Resource.String.Repeated)} {1}";
            TimerStart();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ViewModel.Cancel();
            Finish();
            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            ViewModel.Cancel();
            Finish();
        }
    }
}