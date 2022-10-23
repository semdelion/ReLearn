using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using ReLearn.Core.ViewModels;
using ReLearn.Droid.Services;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;

namespace ReLearn.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Label = ""
              , Theme = "@style/AppTheme"
              , LaunchMode = LaunchMode.SingleTask
              , ScreenOrientation = ScreenOrientation.Portrait
              , WindowSoftInputMode = SoftInput.AdjustResize)]
    public class MainActivity : MvxActivity<MainViewModel>, INavigationActivity,
         FragmentManager.IOnBackStackChangedListener
    {
        public DrawerLayout DrawerLayout { get; set; }

        public void OnBackStackChanged()
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            UserDialogs.Init(this);
            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            this.SupportFragmentManager.AddOnBackStackChangedListener(this);
            ViewModel.ShowMenu();
            AppCenter.Start("48121c30-efdd-4468-b386-68ba1a6c7080", typeof(Analytics), typeof(Crashes));
        }

        public override void OnBackPressed()
        {
            if (DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start))
            {
                DrawerLayout.CloseDrawers();
            }
            else if (SupportFragmentManager.BackStackEntryCount >= 1)
            {
                SupportFragmentManager.PopBackStack();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public void HideSoftKeyboard()
        {
            if (CurrentFocus == null)
            {
                return;
            }

            var inputMethodManager = (InputMethodManager)GetSystemService(InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
            CurrentFocus.ClearFocus();
        }
    }
}