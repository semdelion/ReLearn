using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views.InputMethods;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Droid.Services;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace ReLearn.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Label = "", ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Locale)]
    public class MainActivity : MvxAppCompatActivity<MainViewModel>, INavigationActivity,
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
            SupportFragmentManager.AddOnBackStackChangedListener(this);
            ViewModel.ShowMenu();
        }

        public override void OnBackPressed()
        {
            if (DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start))
                DrawerLayout.CloseDrawers();
            else if (SupportFragmentManager.BackStackEntryCount >= 1)
                SupportFragmentManager.PopBackStack();
            else
                base.OnBackPressed();
        }

        public void HideSoftKeyboard()
        {
            if (CurrentFocus == null) return;
            var inputMethodManager = (InputMethodManager) GetSystemService(InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
            CurrentFocus.ClearFocus();
        }
    }
}