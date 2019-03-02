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
using System;
using System.Collections.Generic;
using System.Timers;

namespace ReLearn.Droid.Images
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class BlitzPollActivity : MvxAppCompatActivity<BlitzPollViewModel>
    {
        private BitmapDrawable BackgroundWord { get; set; }
        private LinearLayout ViewPrev { get; set; }
        private LinearLayout ViewCurrent { get; set; }

        TextView GetTextView()
        {
            LinearLayout.LayoutParams param1 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
            {
                BottomMargin    = PixelConverter.DpToPX(20),
                RightMargin     = PixelConverter.DpToPX(10),
                LeftMargin      = PixelConverter.DpToPX(10)
            };
            int randIndex = (ViewModel.CurrentNumber + new Random(unchecked((int)(DateTime.Now.Ticks))).Next(1, ViewModel.Database.Count)) % ViewModel.Database.Count;
            var textView = new TextView(this)
            {
                TextSize = 20,
                LayoutParameters = param1,
                Text = $"{(ViewModel.Database[ViewModel.Answer ? ViewModel.CurrentNumber : randIndex]).ImageName}",
                Gravity = GravityFlags.CenterHorizontal
            };
            textView.SetTextColor(Colors.White);
            return textView;
        }

        ImageView GetImage()
        {
            LinearLayout.LayoutParams param = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, PixelConverter.DpToPX(200))
            {
                TopMargin    = PixelConverter.DpToPX(15),
                BottomMargin = PixelConverter.DpToPX(20),
                RightMargin  = PixelConverter.DpToPX(10),
                LeftMargin   = PixelConverter.DpToPX(10)
            };
            var ImageView = new ImageView(this) {LayoutParameters = param};
            using (Bitmap bitmap = BitmapFactory.DecodeStream(Application.Context.Assets.Open($"Image{DataBase.TableName}/{ViewModel.Database[ViewModel.CurrentNumber].Image_name}.png")))
            using (var bitmapRounded = BitmapHelper.GetRoundedCornerBitmap(bitmap, PixelConverter.DpToPX(5)))
                ImageView.SetImageBitmap(bitmapRounded);
            return ImageView;
        }

        LinearLayout GetLayout()
        {
            ViewModel.CurrentNumber = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(ViewModel.Database.Count);
            ViewModel.Answer = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(2) == 1 ? true : false;
            RelativeLayout.LayoutParams param = new RelativeLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, PixelConverter.DpToPX(320))
            {
                TopMargin       = PixelConverter.DpToPX(160),
                BottomMargin    = PixelConverter.DpToPX(10),
                RightMargin     = PixelConverter.DpToPX(10),
                LeftMargin      = PixelConverter.DpToPX(10)
            };
            var linearLayout = new LinearLayout(this)
            {
                Orientation     = Orientation.Vertical,
                LayoutParameters= param,
            };
            linearLayout.Background = BackgroundWord;
            linearLayout.AddView(GetImage());
            linearLayout.AddView(GetTextView());
            return linearLayout;
        }
        void RunAnimation(float distance)
        {
            FlingAnimation flingAnimation = new FlingAnimation(ViewCurrent, DynamicAnimation.TranslationX);
            flingAnimation.SetStartVelocity(distance);
            flingAnimation.SetFriction(2);
            flingAnimation.Start();
        }

        public void Answer(bool UserAnswer)
        {
            if (!(ViewModel.Answer ^ UserAnswer))
                ViewModel.True++;
            else
                ViewModel.False++;
            if (ViewPrev!=null)
                FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).RemoveView(ViewPrev);
            ViewCurrent.Background = GetDrawable(!(ViewModel.Answer ^ UserAnswer) ? Resource.Drawable.view_true : Resource.Drawable.view_false);
            RunAnimation((UserAnswer ? 1 : -1) * PixelConverter.DpToPX(5000));
            ViewPrev = ViewCurrent;
            ViewCurrent = GetLayout();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).AddView(ViewCurrent, 0);
            API.Statistics.Add(ViewModel.Database, ViewModel.CurrentNumber, !(ViewModel.Answer ^ UserAnswer) ? -1 : 1);
            ViewModel.TitleCount = $"{GetString(Resource.String.Repeated)} {ViewModel.True + ViewModel.False + 1 }";
        }

        [Java.Interop.Export("Button_Images_No_Click")]
        public void Button_Images_No_Click(View v) => Answer(false);
        
        [Java.Interop.Export("Button_Images_Yes_Click")]
        public void Button_Images_Yes_Click(View v) => Answer(true);

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

            float webViewWidth = Application.Context.Resources.DisplayMetrics.WidthPixels;
            float startX = 0;
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).Touch += (s, e) =>
            {
                var handled = false;
                if (e.Event.Action == MotionEventActions.Down)
                {
                    startX = e.Event.GetX();
                    handled = true;
                }
                else if (e.Event.Action == MotionEventActions.Up)
                {
                    float movement = e.Event.GetX() - startX;
                    float offset = webViewWidth/10;
                    if (Math.Abs(movement) > offset)
                        if (movement < 0)
                            Answer(false);
                        else
                            Answer(true);
                    handled = true;
                }
                e.Handled = handled;
            };
            
            ViewCurrent = GetLayout();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).AddView(ViewCurrent, 1);
            ViewModel.TitleCount = $"{GetString(Resource.String.Repeated)} {1}";
            TimerStart();
        }

        void TimerStart()
        {
            ViewModel.Timer = new Timer{ Interval = 100, Enabled = true };
            ViewModel.Timer.Elapsed += Timer_Elapsed;
            ViewModel.Timer.Start();
        }

        private void Cancel()
        {
            ViewModel.Timer.Dispose();
            ViewModel.Timer = null;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                if (ViewModel.Time > 0)
                {
                    ViewModel.Time--;
                    string sec = $"{ViewModel.Time % 10}0";
                    FindViewById<TextView>(Resource.Id.textView_Timer_Images).Text = $"{ViewModel.Time / 10}:{sec}";

                    if (ViewModel.Time == 50)
                        FindViewById<TextView>(Resource.Id.textView_Timer_Images).SetTextColor(Colors.Red);
                }
                else
                {
                    DBStatistics.Insert(ViewModel.True, ViewModel.False, $"{DataBase.TableName}");
                    ViewModel.ToStatistic.Execute();
                    Cancel();
                    Finish();
                }
            });
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Cancel();
            Finish();
            return base.OnOptionsItemSelected(item);
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Cancel();
            Finish();
        }
    }
}