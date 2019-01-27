using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Android;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Droid.Support.V4;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;
using ReLearn.Droid.Views;

namespace ReLearn.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.navigation_frame)]
    [Register("relearn.droid.fragments.MenuFragment")]
    public class MenuFragment : MvxFragment<MenuViewModel>, NavigationView.IOnNavigationItemSelectedListener
    {
        private NavigationView navigationView;
        private IMenuItem previousMenuItem;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_navigation, null);

            navigationView = view.FindViewById<NavigationView>(Resource.Id.navigation_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.Menu.FindItem(Resource.Id.nav_home).SetChecked(true);

            return view;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            item.SetCheckable(true);
            item.SetChecked(true);
            previousMenuItem?.SetChecked(false);
            previousMenuItem = item;
            Navigate(item.ItemId);

            return true;
        }

        private async Task Navigate(int itemId)
        {
            ((MainActivity)Activity).DrawerLayout.CloseDrawers ();
            await Task.Delay (TimeSpan.FromMilliseconds (250));

            switch (itemId) {
            case Resource.Id.nav_home:
                ViewModel.ToHomeViewModel.Execute();
                break;
			case Resource.Id.nav_second_home:
                ViewModel.ToAboutUsViewModel.Execute();
                break;
            }
        }
    }
}