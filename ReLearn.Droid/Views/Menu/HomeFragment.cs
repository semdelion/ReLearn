using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid.Views.Menu
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    [Register("relearn.droid.views.menu.HomeFragment")]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_main;

        protected override int Toolbar => Resource.Id.background_toolbar_home;

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.settings, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            MenuFragment.NavigationView.Menu.FindItem(Resource.Id.study).SetChecked(false);
            MenuFragment.NavigationView.Menu.FindItem(Resource.Id.dictionaries).SetChecked(true);
            switch (item.ItemId)
            {
                case Resource.Id.MenuSelectDictionary:
                    ViewModel.ToSelectDictionary.Execute();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}