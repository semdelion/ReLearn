using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Interop;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels.Languages;
using ReLearn.Droid.Helpers;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace ReLearn.Droid.Views.Languages
{
    [Activity(Label = "", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LearnActivity : MvxAppCompatActivity<LearnViewModel>
    {
        [Export("Button_Languages_Learn_Voice_Enable")]
        public void Button_Languages_Learn_Voice_Enable(View v)
        {
            ViewModel.VoiceEnable = !ViewModel.VoiceEnable;
            FindViewById<ImageButton>(Resource.Id.Button_Speak_TurnOn_TurnOff).SetImageDrawable(
                GetDrawable(ViewModel.VoiceEnable ? Resource.Mipmap.speak_on : Resource.Mipmap.speak_off));
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_languages_learn);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbar_languages_learn);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            var displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);
            using (var background = BitmapHelper.GetBackgroung(Resources,
                displayMetrics.WidthPixels - PixelConverter.DpToPX(70), PixelConverter.DpToPX(300)))
            {
                FindViewById<TextView>(Resource.Id.textView_learn_en).Background = background;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}