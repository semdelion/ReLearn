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
        Timer timer;
        BitmapDrawable _backgroundWord;
        TextView ViewPrev;
        TextView ViewCurrent;
        List<DBWords> WordDatabase { get; set; }
        bool answer;
        int CurrentWordNumber;
        string TitleCount { set => FindViewById<TextView>(Resource.Id.BlitzPoll_toolbar_textview).Text = value; }
        int Time = Settings.TimeToBlitz * 10;
        int True = 0, False = 0;
        TextView GetTextView()
        {
            RelativeLayout.LayoutParams param = new RelativeLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, PixelConverter.DpToPX(320))
            {
                TopMargin = PixelConverter.DpToPX(160),
                BottomMargin = PixelConverter.DpToPX(10),
                RightMargin = PixelConverter.DpToPX(10),
                LeftMargin = PixelConverter.DpToPX(10)
            };
           
            CurrentWordNumber = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(WordDatabase.Count);
            int randIndex = (CurrentWordNumber + new Random(unchecked((int)(DateTime.Now.Ticks))).Next(1, WordDatabase.Count))% WordDatabase.Count;
            answer = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(2) == 1 ? true : false;
            string TranslationWord = WordDatabase[answer ? CurrentWordNumber : randIndex].TranslationWord;
            var textView = new TextView(this)
            {
                TextSize        = 30,
                Elevation       = PixelConverter.DpToPX(10),
                LayoutParameters= param,
                Text            = $"{WordDatabase[CurrentWordNumber].Word}\n\n{TranslationWord}",
                Gravity         = GravityFlags.CenterHorizontal | GravityFlags.Center
            };

            textView.Background = _backgroundWord;
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
            if (!(answer ^ UserAnswer))
                True++;
            else
                False++;
            if (ViewPrev != null)
                FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutLanguagesBlitzPoll).RemoveView(ViewPrev);
            ViewCurrent.Background = GetDrawable(!(answer ^ UserAnswer) ? Resource.Drawable.view_true : Resource.Drawable.view_false);
            RunAnimation((UserAnswer ? 1 : -1) * PixelConverter.DpToPX(5000));
            ViewPrev = ViewCurrent;
            ViewCurrent = GetTextView();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutLanguagesBlitzPoll).AddView(ViewCurrent, 0);
            API.Statistics.Add(WordDatabase, CurrentWordNumber, !(answer ^ UserAnswer) ? -1 : 1);
            TitleCount = $"{GetString(Resource.String.Repeated)} {True + False + 1 }";
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

            _backgroundWord = BitmapHelper.GetBackgroung(Resources,
                displayMetrics.WidthPixels - PixelConverter.DpToPX(50),
                PixelConverter.DpToPX(300));
            var _backgroundTimer = BitmapHelper.GetBackgroung(Resources,
                displayMetrics.WidthPixels - PixelConverter.DpToPX(200),
                PixelConverter.DpToPX(50));


            float webViewWidth = Android.App.Application.Context.Resources.DisplayMetrics.WidthPixels;
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
                    {
                        if (movement < 0)
                            Answer(false);
                        else
                            Answer(true);
                    }
                    handled = true;
                }
                e.Handled = handled;
            };
            FindViewById<TextView>(Resource.Id.textView_Timer_language).Background = _backgroundTimer;
            WordDatabase = DBWords.GetDataNotLearned;
            if (WordDatabase.Count == 0)
            {
                Toast.MakeText(this, GetString(Resource.String.RepeatedAllWords), ToastLength.Short).Show();
                Finish();
                return;
            }
            ViewCurrent = GetTextView();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutLanguagesBlitzPoll).AddView(ViewCurrent, 1);
            TitleCount = $"{GetString(Resource.String.Repeated)} {1}";
            TimerStart();
            

        }

        void TimerStart()
        {
            timer = new Timer{ Interval = 100, Enabled = true };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                if (Time > 0)
                {
                    Time--;
                    string sec = $"{Time % 10}0";
                    FindViewById<TextView>(Resource.Id.textView_Timer_language).Text = $"{Time / 10}:{sec}";
                    if (Time == 50)
                        FindViewById<TextView>(Resource.Id.textView_Timer_language).SetTextColor(Colors.Red);
                }
                else
                {
                    DBStatistics.Insert(True, False, DataBase.TableName.ToString());
                    ViewModel.ToStatistic.Execute();
                    timer.Dispose();
                    Finish();
                }
            });
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            timer.Dispose();
            Finish();
            return base.OnOptionsItemSelected(item);
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            timer.Dispose();
            Finish();
        }
    }
}