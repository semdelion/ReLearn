using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.AppCompat;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;
using ReLearn.Droid.Services;
using ReLearn.Droid.Views.Base;

namespace ReLearn.Droid.Views.Menu
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("relearn.droid.views.menu.AddDictionaryFragments")]
    public class AddDictionaryFragments : MvxBaseFragment<AddDictionaryViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_menu_add_dictionary;

        protected override int Toolbar => Resource.Id.toolbar_add_dictionary;

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnPrepareOptionsMenu(menu);
            inflater.Inflate(Resource.Menu.menu_DictionaryReplenishment, menu);
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
                                                                          // nav drawer icon to replace 'Up' caret
                    Resource.String.navigation_drawer_open,
                    Resource.String.navigation_drawer_close);
            }

            SetHomeAsUp(ParentActivity.SupportFragmentManager.BackStackEntryCount == 0 ? false : true);
            return view;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.dictionary_replenishment_instruction)
            {
                var alert = new AlertDialog.Builder(ParentActivity);
                var factory = LayoutInflater.From(ParentActivity);
                alert.SetView(factory.Inflate(Resource.Layout.alert_dictionary, null));
                alert.SetTitle("Instruction"); // TODO
                alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
                alert.Show();
                return true;
            }

            if (ParentActivity.SupportFragmentManager.BackStackEntryCount >= 1)
            {
                ParentActivity.SupportFragmentManager.PopBackStack();
                return true;
            }

            return false;
        }
    }
}