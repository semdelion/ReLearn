using Android.Animation;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.AppCompat;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;
using ReLearn.Droid.Services;
using System.Threading.Tasks;

namespace ReLearn.Droid.Views.Base
{
    public abstract class MvxBaseFragment : MvxFragment
    {
        protected MvxActionBarDrawerToggle _drawerToggle;
        protected Toolbar _toolbar;
        protected bool isHomeAsUp;

        protected MvxBaseFragment()
        {
            RetainInstance = true;
        }

        protected abstract int FragmentId { get; }
        protected abstract int Toolbar { get; }

        public MvxActivity ParentActivity => (MvxActivity)Activity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(FragmentId, null);
            _toolbar = view.FindViewById<Toolbar>(Toolbar);

            if (_toolbar != null)
            {
                ParentActivity.SetSupportActionBar(_toolbar);
                ParentActivity.SupportActionBar.SetHomeButtonEnabled(true);
                ParentActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                _drawerToggle = new MvxActionBarDrawerToggle(
                    Activity, // host Activity
                    (ParentActivity as INavigationActivity).DrawerLayout, // DrawerLayout object
                    _toolbar, // nav drawer icon to replace 'Up' caret
                    Resource.String.navigation_drawer_open,
                    Resource.String.navigation_drawer_close);
                _drawerToggle.DrawerOpened += (sender, e) => (Activity as MainActivity)?.HideSoftKeyboard();
                (ParentActivity as INavigationActivity).DrawerLayout.AddDrawerListener(_drawerToggle);
            }

            Task.Run(() => SetHomeAsUp(ParentActivity.SupportFragmentManager.BackStackEntryCount == 0 ? false : true));
            return view;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (_toolbar != null)
            {
                _drawerToggle.OnConfigurationChanged(newConfig);
            }
        }

        public override void OnPause()
        {
            base.OnPause();
            Task.Run(() => SetHomeAsUp(ParentActivity.SupportFragmentManager.BackStackEntryCount == 0 ? false : true));
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            if (_toolbar != null)
            {
                _drawerToggle?.SyncState();
            }
        }

        protected void SetHomeAsUp(bool isHomeAsUp)
        {
            if (this.isHomeAsUp != isHomeAsUp)
            {
                this.isHomeAsUp = isHomeAsUp;
                var anim = isHomeAsUp ? ValueAnimator.OfFloat(0, 1) : ValueAnimator.OfFloat(1, 0);
                anim.SetDuration(300);
                anim.Update += (sender, e) =>
                {
                    var value = (float)anim.AnimatedValue;
                    _drawerToggle.DrawerArrowDrawable.Progress = value;
                };
                anim.Start();
            }
        }
    }

    public abstract class MvxBaseFragment<TViewModel> : MvxBaseFragment where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get => (TViewModel)base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}