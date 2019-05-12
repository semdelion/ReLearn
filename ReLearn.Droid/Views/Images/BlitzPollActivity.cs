using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Interop;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Images;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Views.Facade;
using System;
using System.Threading.Tasks;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace ReLearn.Droid.Views.Images
{
    [Activity(Label = "", ScreenOrientation = ScreenOrientation.Portrait)]
    public class BlitzPollActivity : MvxAppCompatActivityBlitzPoll<BlitzPollViewModel>
    {
        private LinearLayout ViewPrev { get; set; }
        private LinearLayout ViewCurrent { get; set; }

        private TextView GetTextView()
        {
            var param = PixelConverter.GetParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent,
                10, 0, 10, 20);
            var randIndex =
                (ViewModel.CurrentNumber +
                 new Random(unchecked((int) DateTime.Now.Ticks)).Next(1, ViewModel.Database.Count)) %
                ViewModel.Database.Count;
            var textView = new TextView(this)
            {
                TextSize = 20,
                LayoutParameters = param,
                Text = $"{ViewModel.Database[ViewModel.Answer ? ViewModel.CurrentNumber : randIndex].ImageName}",
                Gravity = GravityFlags.CenterHorizontal
            };
            textView.SetTextColor(Colors.White);
            return textView;
        }

        private ImageView GetImage()
        {
            var param = PixelConverter.GetParams(ViewGroup.LayoutParams.MatchParent, PixelConverter.DpToPX(200), 10, 15,
                10, 20);
            var imageView = new ImageView(this) {LayoutParameters = param};
            using (var bitmap = BitmapFactory.DecodeStream(Application.Context.Assets.Open(
                $"Image{DataBase.TableName}/{ViewModel.Database[ViewModel.CurrentNumber].Image_name}.png")))
            using (var bitmapRounded = BitmapHelper.GetRoundedCornerBitmap(bitmap, PixelConverter.DpToPX(5)))
            {
                imageView.SetImageBitmap(bitmapRounded);
            }

            return imageView;
        }

        private LinearLayout GetLayout()
        {
            ViewModel.CurrentNumber = new Random(unchecked((int) DateTime.Now.Ticks)).Next(ViewModel.Database.Count);
            ViewModel.Answer = new Random(unchecked((int) DateTime.Now.Ticks)).Next(2) == 1 ? true : false;
            var param = PixelConverter.GetParamsRelative(ViewGroup.LayoutParams.MatchParent, PixelConverter.DpToPX(320),
                10, 160, 10, 10);
            var linearLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical,
                LayoutParameters = param
            };
            linearLayout.Background = BackgroundWord;
            linearLayout.AddView(GetImage());
            linearLayout.AddView(GetTextView());
            return linearLayout;
        }

        public override async Task Answer(bool userAnswer)
        {
            if (!(ViewModel.Answer ^ userAnswer))
                ViewModel.True++;
            else
                ViewModel.False++;
            if (ViewPrev != null)
                FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).RemoveView(ViewPrev);
            ViewCurrent.Background = GetDrawable(!(ViewModel.Answer ^ userAnswer)
                ? Resource.Drawable.view_true
                : Resource.Drawable.view_false);
            RunAnimation(ViewCurrent, (userAnswer ? 1 : -1) * PixelConverter.DpToPX(5000));
            ViewPrev = ViewCurrent;
            ViewCurrent = GetLayout();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).AddView(ViewCurrent, 0);
            await API.Statistics.Add(ViewModel.Database, ViewModel.CurrentNumber,
                !(ViewModel.Answer ^ userAnswer)? 1 : -1);
            ViewModel.TitleCount = $"{GetString(Resource.String.Repeated)} {ViewModel.True + ViewModel.False + 1}";
        }

        [Export("Button_Images_No_Click")]
        public async void Button_Images_No_Click(View v)
        {
            await Answer(false);
        }

        [Export("Button_Images_Yes_Click")]
        public async void Button_Images_Yes_Click(View v)
        {
            await Answer(true);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_images_blitz_poll);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbar_images_blitz_poll);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            var displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);

            BackgroundWord = BitmapHelper.GetBackgroung(Resources,
                displayMetrics.WidthPixels - PixelConverter.DpToPX(20), PixelConverter.DpToPX(300));
            using (var background = BitmapHelper.GetBackgroung(Resources,
                displayMetrics.WidthPixels - PixelConverter.DpToPX(200), PixelConverter.DpToPX(50)))
            {
                FindViewById<TextView>(Resource.Id.textView_Timer_Images).Background = background;
            }

            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).Touch += Swipes;

            ViewCurrent = GetLayout();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).AddView(ViewCurrent, 1);
            ViewModel.TitleCount = $"{GetString(Resource.String.Repeated)} {1}";
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