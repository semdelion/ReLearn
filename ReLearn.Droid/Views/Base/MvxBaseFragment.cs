using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;

namespace ReLearn.Droid.Views.Base
{
    public abstract class MvxBaseFragment : MvxFragment
    {
        protected Toolbar _toolbar;

        protected abstract int FragmentId { get; }

        public MvxAppCompatActivity ParentActivity => (MvxAppCompatActivity)Activity;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(FragmentId, null);
            return view;
        }
    }

    public abstract class MvxBaseFragment<TViewModel> : MvxBaseFragment where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get => (TViewModel)base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}