using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Views;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using Calligraphy;
using Android.Support.Animation;
using System.Threading;
using System.Threading.Tasks;
using Android.Util;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Timers;

namespace ReLearn.Droid.Languages
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class BlitzPoll : AppCompatActivity
    {
        System.Timers.Timer timer;
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
            RelativeLayout.LayoutParams param = new RelativeLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, AdditionalFunctions.DpToPX(300))
            {
                TopMargin = AdditionalFunctions.DpToPX(160),
                BottomMargin = AdditionalFunctions.DpToPX(10),
                RightMargin = AdditionalFunctions.DpToPX(25),
                LeftMargin = AdditionalFunctions.DpToPX(25)
            };
            CurrentWordNumber = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(WordDatabase.Count);
            int randIndex = (CurrentWordNumber + new Random(unchecked((int)(DateTime.Now.Ticks))).Next(1, WordDatabase.Count))% WordDatabase.Count;
            answer = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(2) == 1 ? true : false;
            string TranslationWord = WordDatabase[answer ? CurrentWordNumber : randIndex].TranslationWord;
            var textView = new TextView(this)
            {
                TextSize        = 30,
                Elevation       = AdditionalFunctions.DpToPX(10),
                LayoutParameters= param,
                Background      = GetDrawable(Resource.Drawable.viewNeutral),
                Text            = $"{WordDatabase[CurrentWordNumber].Word}\n\n{TranslationWord}",
                Gravity         = GravityFlags.CenterHorizontal | GravityFlags.Center
            };
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
            ViewCurrent.Background = GetDrawable(!(answer ^ UserAnswer) ? Resource.Drawable.viewTrue : Resource.Drawable.viewFalse);
            RunAnimation((UserAnswer ? 1 : -1) * AdditionalFunctions.DpToPX(5000));
            ViewPrev = ViewCurrent;
            ViewCurrent = GetTextView();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutLanguagesBlitzPoll).AddView(ViewCurrent, 0);
            Statistics.Add(WordDatabase, CurrentWordNumber, !(answer ^ UserAnswer) ? -1 : 1);
            TitleCount = $"{GetString(Resource.String.Repeated)} {True + False + 1 }";
        }

        [Java.Interop.Export("Button_Languages_No_Click")]
        public void Button_Languages_No_Click(View v) => Answer(false);
        
        [Java.Interop.Export("Button_Languages_Yes_Click")]
        public void Button_Languages_Yes_Click(View v) => Answer(true);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BlitzPoll_Languages);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguagesBlitzPoll);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

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
            timer = new System.Timers.Timer{ Interval = 100, Enabled = true };
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
                    DBStatistics.Insert(True, False, DataBase.TableNameLanguage.ToString());
                    StartActivity(typeof(Statistic));
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

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}