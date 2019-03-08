using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;
using System;
using System.Threading.Tasks;

namespace ReLearn.Droid.Views.Menu
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.navigation_frame)]
    [Register("relearn.droid.views.menu.MenuFragment")]
    public class MenuFragment : MvxFragment<MenuViewModel>, NavigationView.IOnNavigationItemSelectedListener
    {
        public static NavigationView NavigationView { get; private set; }
        private IMenuItem _previousMenuItem;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_navigation, null);

            NavigationView = view.FindViewById<NavigationView>(Resource.Id.nav_view);
            NavigationView.SetNavigationItemSelectedListener(this);
            NavigationView.Menu.FindItem(Resource.Id.study).SetChecked(true);
            
            return view;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            if (item != _previousMenuItem)
                _previousMenuItem?.SetChecked(false);
            item.SetCheckable(true);
            item.SetChecked(true);
            _previousMenuItem = item;
            Navigate(item.ItemId);
            return true;
        }

        private async Task Navigate(int itemId)
        {
            ((MainActivity)Activity).DrawerLayout.CloseDrawers();

            await Task.Delay(TimeSpan.FromMilliseconds(250));

            switch (itemId)
            {
                case Resource.Id.study:
                    ViewModel.ToHomeViewModel.Execute();
                    break;
                case Resource.Id.dictionaries:
                    ViewModel.ToSelectDictionary.Execute();
                    break;
                case Resource.Id.statistics:
                    ViewModel.ToStatisticViewModel.Execute();
                    break;
                case Resource.Id.view_dictionary:
                    ViewModel.ToViewDictionaryViewModel.Execute();
                    break;
                case Resource.Id.add_word:
                    ViewModel.ToAdditionViewModel.Execute();
                    break;
                case Resource.Id.settings_menu:
                    ViewModel.ToSettingsViewModel.Execute();
                    break;
                case Resource.Id.feedback:
                    ViewModel.ToFeedbackViewModel.Execute();
                    break;
                case Resource.Id.about_us:
                    ViewModel.ToAboutUsViewModel.Execute();
                    break;
            }
        }
    }
}