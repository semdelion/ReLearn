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

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_navigation, null);

            navigationView = view.FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.Menu.FindItem(Resource.Id.study).SetChecked(true);
            
            //ViewModel.ToHomeViewModel.Execute();
            return view;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            item.SetCheckable(true);
            item.SetChecked(true);
            previousMenuItem?.SetChecked(false);
            previousMenuItem = item;
            ((MainActivity)Activity).DrawerLayout.CloseDrawers();
            switch (item.ItemId)
            {
                case Resource.Id.study:
                    ViewModel.ToHomeViewModel.Execute();
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
            return true;
        }
    }
}