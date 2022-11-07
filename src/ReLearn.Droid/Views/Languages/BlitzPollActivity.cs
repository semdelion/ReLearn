using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Interop;
using ReLearn.Core.ViewModels.Languages;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Views.Facade;
using System;
using System.Threading.Tasks;

namespace ReLearn.Droid.Views.Languages
{
    [Activity(Label = "", ScreenOrientation = ScreenOrientation.Portrait)]
    public class BlitzPollActivity : MvxAppCompatActivityBlitzPoll<BlitzPollViewModel>
    {
        private TextView ViewPrev { get; set; }
        private TextView ViewCurrent { get; set; }

        private TextView GetTextView()
        {
            var param = PixelConverter.GetParamsRelative(ViewGroup.LayoutParams.MatchParent, PixelConverter.DpToPX(320),
                10, 160, 10, 10);

            ViewModel.CurrentNumber = new Random(unchecked((int)DateTime.Now.Ticks)).Next(ViewModel.Database.Count);
            var randIndex =
                (ViewModel.CurrentNumber +
                 new Random(unchecked((int)DateTime.Now.Ticks)).Next(1, ViewModel.Database.Count)) %
                ViewModel.Database.Count;
            ViewModel.Answer = new Random(unchecked((int)DateTime.Now.Ticks)).Next(2) == 1 ? true : false;
            var translationWord = ViewModel.Database[ViewModel.Answer ? ViewModel.CurrentNumber : randIndex]
                .TranslationWord;
            var textView = new TextView(this)
            {
                TextSize = 30,
                Elevation = PixelConverter.DpToPX(10),
                LayoutParameters = param,
                Text = $"{ViewModel.Database[ViewModel.CurrentNumber].Word}\n\n{translationWord}",
                Gravity = GravityFlags.CenterHorizontal | GravityFlags.Center
            };

            textView.Background = BackgroundWord;
            textView.SetTextColor(Colors.White);
            return textView;
        }

        public override async Task Answer(bool userAnswer)
        {
            await API.Statistics.Add(ViewModel.Database, ViewModel.CurrentNumber,
              !(ViewModel.Answer ^ userAnswer) ? -1 : 1);
            if (!(ViewModel.Answer ^ userAnswer))
            {
                ViewModel.True++;
            }
            else
            {
                ViewModel.False++;
            }

            if (ViewPrev != null)
            {
                FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutLanguagesBlitzPoll).RemoveView(ViewPrev);
            }

            ViewCurrent.Background = GetDrawable(!(ViewModel.Answer ^ userAnswer)
                ? Resource.Drawable.view_true
                : Resource.Drawable.view_false);
            RunAnimation(ViewCurrent, (userAnswer ? 1 : -1) * PixelConverter.DpToPX(5000));
            ViewPrev = ViewCurrent;
            ViewCurrent = GetTextView();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutLanguagesBlitzPoll).AddView(ViewCurrent, 0);
            ViewModel.TitleCount = $"{ViewModel.True + ViewModel.False + 1}";
        }

        [Export("Button_Languages_No_Click")]
        public async void Button_Languages_No_Click(View view)
        {
            await Answer(false);
        }

        [Export("Button_Languages_Yes_Click")]
        public async void Button_Languages_Yes_Click(View view)
        {
            await Answer(true);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_languages_blitz_poll);
            var toolbarMain = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar_languages_blitz_poll);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            var displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);

            BackgroundWord = BitmapHelper.GetBackgroung(Resources,
                displayMetrics.WidthPixels - PixelConverter.DpToPX(50), PixelConverter.DpToPX(300));
            using (var background = BitmapHelper.GetBackgroung(Resources,
                displayMetrics.WidthPixels - PixelConverter.DpToPX(200), PixelConverter.DpToPX(50)))
            {
                FindViewById<TextView>(Resource.Id.textView_Timer_language).Background = background;
            }

            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutLanguagesBlitzPoll).Touch += Swipes;

            ViewCurrent = GetTextView();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutLanguagesBlitzPoll).AddView(ViewCurrent, 1);
            ViewModel.TimerStart();
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