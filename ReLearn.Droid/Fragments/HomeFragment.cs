using Android;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("relearn.droid.fragments.HomeFragment")]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_main;

        protected override int Toolbar => Resource.Id.toolbarMain;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.settings, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.MenuSelectDictionary:
                    ViewModel.ToSelectDictionary.Execute();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        //public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    base.OnCreateView(inflater, container, savedInstanceState);

        //    var view = this.BindingInflate(FragmentId, null);
        //    _toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarMain);


        //    return null;
        //}
    }
}

//protected override void OnCreate(Bundle savedInstanceState)
//{
//    base.OnCreate(savedInstanceState);
//    SetContentView(Resource.Layout.main_activity);
//    var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarMain);
//    SetSupportActionBar(toolbar);

//    DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
//    ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
//    drawer.AddDrawerListener(toggle);
//    toggle.SyncState();

//    NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
//    navigationView.SetNavigationItemSelectedListener(this);
//}
