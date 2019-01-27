using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("relearn.droid.fragments.SettingsFragment")]
    public class AboutUslFragment : BaseFragment<AboutUsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_about_us;
    }
}