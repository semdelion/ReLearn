using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;
using ReLearn.Droid.Views.Base;

namespace ReLearn.Droid.Views.Menu
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("relearn.droid.views.menu.SettingsFragment")]
    internal class SettingsFragment : MvxBaseFragment<SettingsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_menu_settings;

        protected override int Toolbar => Resource.Id.toolbar_setting;
    }
}