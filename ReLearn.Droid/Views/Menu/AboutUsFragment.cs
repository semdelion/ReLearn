using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid.Views.Menu
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    [Register("relearn.droid.views.menu.AboutUslFragment")]
    public class AboutUslFragment : BaseFragment<AboutUsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_about_us;

        protected override int Toolbar => Resource.Id.toolbar_About_Us;
    }
}