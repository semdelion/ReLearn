using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Animation;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Languages;
using ReLearn.Droid.Helpers;
using System;
using System.Collections.Generic;
using System.Timers;

namespace ReLearn.Droid.Languages
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class BlitzPollActivity : MvxAppCompatActivity<BlitzPollViewModel>
    {
        private BitmapDrawable BackgroundWord { get; set; }
        private TextView ViewPrev { get; set; }
        private TextView ViewCurrent { get; set; }

        TextView GetTextView()
        {
            RelativeLayout.LayoutParams param = new RelativeLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, PixelConverter.DpToPX(320))
            {
                TopMargin = PixelConverter.DpToPX(160),
                BottomMargin = PixelConverter.DpToPX(10),
                RightMargin = PixelConverter.DpToPX(10),
                LeftMargin = PixelConverter.DpToPX(10)
            };

            ViewModel.CurrentNumber = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(ViewModel.Database.Count);
            int randIndex = (ViewModel.CurrentNumber + new Random(unchecked((int)(DateTime.Now.Ticks))).Next(1, ViewModel.Database.Count))% ViewModel.Database.Count;
            ViewModel.Answer = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(2) == 1 ? true : false;
            string TranslationWord = ViewModel.Database[ViewModel.Answer ? ViewModel.CurrentNumber : randIndex].TranslationWord;
            var textView = new TextView(this)
            {
                TextSize        = 30,
                Elevation       = PixelConverter.DpToPX(10),
                LayoutParameters= param,
                Text            = $"{ViewModel.Database[ViewModel.CurrentNumber].Word}\n\n{TranslationWord}",
                Gravity         = GravityFlags.CenterHorizontal | GravityFlags.Center
            };

            textView.Background = BackgroundWord;
            textView.SetTextColor(Colors.White);
            return textView;
        }

        void RunAnimation(float distance)
        {
            FlingAnimation flingAnimation = new FlingAnimation(ViewCurrent, DynamicAnimation.TranslationX);
            flingAnimation.SetStartVelocity(distance);
            flingAnimation.SetFriction(2);
            flingAnimation.Start();
        }

        void Answer(bool UserAnswer)
        {
            if (!(ViewModel.Answer ^ UserAnswer))
                ViewModel.True++;
            else
                ViewModel.False++;
            if (ViewPrev != null)
                FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutLanguagesBlitzPoll).RemoveView(ViewPrev);
            ViewCurrent.Background = GetDrawable(!(ViewModel.Answer ^ UserAnswer) ? Resource.Drawable.view_true : Resource.Drawable.view_false);
            RunAnimation((UserAnswer ? 1 : -1) * PixelConverter.DpToPX(5000));
            ViewPrev = ViewCurrent;
            ViewCurrent = GetTextView();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutLanguagesBlitzPoll).AddView(ViewCurrent, 0);
            API.Statistics.Add(ViewModel.Database, ViewModel.CurrentNumber, !(ViewModel.Answer ^ UserAnswer) ? -1 : 1);
            ViewModel.TitleCount = $"{GetString(Resource.String.Repeated)} { ViewModel.True + ViewModel.False + 1 }";
        }

        [Java.Interop.Export("Button_Languages_No_Click")]
        public void Button_Languages_No_Click(View v) => Answer(false);
        
        [Java.Interop.Export("Button_Languages_Yes_Click")]
        public void Button_Languages_Yes_Click(View v) => Answer(true);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_languages_blitz_poll);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_languages_blitz_poll);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);

            BackgroundWord = BitmapHelper.GetBackgroung(Resources, displayMetrics.WidthPixels - PixelConverter.DpToPX(50), PixelConverter.DpToPX(300));
            using (var background = BitmapHelper.GetBackgroung(Resources, displayMetrics.WidthPixels - PixelConverter.DpToPX(200), PixelConverter.DpToPX(50)))
                FindViewById<TextView>(Resource.Id.textView_Timer_language).Background = background;
     
            float webViewWidth = Application.Context.Resources.DisplayMetrics.WidthPixels;
            float startX = 0;
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutLanguagesBlitzPoll).Touch += (s, e) =>
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
                    float offset = webViewWidth / 10;

                    if (Math.Abs(movement) > offset)
                        if (movement < 0)
                            Answer(false);
                        else
                            Answer(true);
                    handled = true;
                }
                e.Handled = handled;
            };
            
            ViewCurrent = GetTextView();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutLanguagesBlitzPoll).AddView(ViewCurrent, 1);
            ViewModel.TitleCount = $"{GetString(Resource.String.Repeated)} {1}";
            TimerStart();
        }

        void TimerStart()
        {
            ViewModel.Timer = new Timer{ Interval = 100, Enabled = true };
            ViewModel.Timer.Elapsed += TimerElapsed;
            ViewModel.Timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                if (ViewModel.Time > 0)
                {
                    ViewModel.Time--;
                    string sec = $"{ ViewModel.Time % 10}0";
                    FindViewById<TextView>(Resource.Id.textView_Timer_language).Text = $"{ ViewModel.Time / 10}:{sec}";
                    if (ViewModel.Time == 50)
                        FindViewById<TextView>(Resource.Id.textView_Timer_language).SetTextColor(Colors.Red);
                }
                else
                {
                    DBStatistics.Insert(ViewModel.True, ViewModel.False, DataBase.TableName.ToString());
                    ViewModel.ToStatistic.Execute();
                    ViewModel.Timer.Dispose();
                    Finish();
                }
            });
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ViewModel.Timer.Dispose();
            Finish();
            return base.OnOptionsItemSelected(item);
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            ViewModel.Timer.Dispose();
            Finish();
        }
    }
}