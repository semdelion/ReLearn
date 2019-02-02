using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid.Views.Menu
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    [Register("relearn.droid.views.menu.SettingsFragment")]
    class SettingsFragment : BaseFragment<SettingsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings;

        protected override int Toolbar => Resource.Id.toolbarSetting;
    }
}