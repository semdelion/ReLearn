using Android.OS;
using Android.Runtime;
using Android.Views;
using Google.Android.Material.Navigation;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
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
        private IMenuItem _previousMenuItem;
        public static NavigationView NavigationView { get; private set; }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            if (item != _previousMenuItem)
            {
                _previousMenuItem?.SetChecked(false);
            }

            item.SetCheckable(true);
            item.SetChecked(true);
            _previousMenuItem = item;
            Navigate(item.ItemId).ConfigureAwait(false);
            return true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_navigation, null);

            NavigationView = view.FindViewById<NavigationView>(Resource.Id.nav_view);
            NavigationView.SetNavigationItemSelectedListener(this);
            NavigationView.Menu.FindItem(Resource.Id.study).SetChecked(true);
            UpdateNavigatiomView();
            
            return view;
        }

        public static void UpdateNavigatiomView()
        {
            if (NavigationView != null)
            {
                NavigationView.Menu.FindItem(Resource.Id.study).SetTitle(Xamarin.Yaml.Localization.I18N.Instance.Translate("Menu.Study"));
                NavigationView.Menu.FindItem(Resource.Id.dictionaries).SetTitle(Xamarin.Yaml.Localization.I18N.Instance.Translate("Menu.Dictionaries"));
                NavigationView.Menu.FindItem(Resource.Id.statistics).SetTitle(Xamarin.Yaml.Localization.I18N.Instance.Translate("Menu.Statistics"));
                NavigationView.Menu.FindItem(Resource.Id.view_dictionary).SetTitle(Xamarin.Yaml.Localization.I18N.Instance.Translate("Menu.ViewDictionary"));
                NavigationView.Menu.FindItem(Resource.Id.add_word).SetTitle(Xamarin.Yaml.Localization.I18N.Instance.Translate("Menu.AddWord"));
                NavigationView.Menu.FindItem(Resource.Id.settings_menu).SetTitle(Xamarin.Yaml.Localization.I18N.Instance.Translate("Menu.Settings"));
                NavigationView.Menu.FindItem(Resource.Id.feedback).SetTitle(Xamarin.Yaml.Localization.I18N.Instance.Translate("Menu.Feedback"));
               // NavigationView.Menu.FindItem(Resource.Id.about_us).SetTitle(Xamarin.Yaml.Localization.I18N.Instance.Translate("Menu.AboutUs"));
            }
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
                /*case Resource.Id.about_us:
                    ViewModel.ToAboutUsViewModel.Execute();
                    break;*/
            }
        }
    }
}