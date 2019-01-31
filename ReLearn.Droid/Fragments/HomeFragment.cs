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
    }
}