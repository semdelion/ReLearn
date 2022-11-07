using Android.Views;
using AndroidX.Core.View;
using AndroidX.RecyclerView.Widget;
using ReLearn.Droid.Implements.Listeners;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ReLearn.Droid.Implements
{
    public interface IViewInsets
    {
        public void Apply();
    }

    public class DefaultViewInsets : IViewInsets
    {
        private readonly View _view;
        private readonly int _insetsType = WindowInsetsCompat.Type.DisplayCutout() | WindowInsetsCompat.Type.SystemBars() | WindowInsetsCompat.Type.Ime();

        public DefaultViewInsets(View view) => _view = view;

        public void Apply()
        {
            ViewCompat.SetOnApplyWindowInsetsListener(_view, new InsetsListener(_insetsType, bottom: true));
            RequestApplyInsetsWhenAttached(_view);
        }

        public void RequestApplyInsetsWhenAttached(View view)
        {
            if (view.IsAttachedToWindow)
                ViewCompat.RequestApplyInsets(view);
            else
                view.AddOnAttachStateChangeListener(new InsetsAttachStateChangeListener());
        }
    }

    public class ToolbarViewInsets : IViewInsets
    {
        private readonly Toolbar _toolbar;
        private readonly int _insetsType = WindowInsetsCompat.Type.DisplayCutout() | WindowInsetsCompat.Type.SystemBars();

        public ToolbarViewInsets(Toolbar toolbar)
        {
            _toolbar = toolbar;
        }

        public void Apply()
        {
            if (_toolbar == null)
                return;

            ViewCompat.SetOnApplyWindowInsetsListener(_toolbar, new InsetsListener(_insetsType, top: true));
            RequestApplyInsetsWhenAttached(_toolbar);
        }

        public void RequestApplyInsetsWhenAttached(Toolbar view)
        {
            if (view.IsAttachedToWindow)
                ViewCompat.RequestApplyInsets(view);
            else
                view.AddOnAttachStateChangeListener(new InsetsAttachStateChangeListener());
        }
    }

    /// <summary>
    /// don't forget use android:clipToPadding  ="false" for RecyclerView
    /// </summary>
    public class RecyclerViewInsets : IViewInsets
    {
        private readonly RecyclerView _recycler;
        private readonly View _rootView;
        private readonly int _insetsRecycler = WindowInsetsCompat.Type.DisplayCutout() | WindowInsetsCompat.Type.SystemBars();
        private readonly int _insetsRootView = WindowInsetsCompat.Type.Ime();

        public RecyclerViewInsets(View rootView, RecyclerView recycler)
        {
            _recycler = recycler;
            _rootView = rootView;
        }

        public void Apply()
        {
            if (_recycler == null || _rootView == null)
                return;

            ViewCompat.SetOnApplyWindowInsetsListener(_recycler, new InsetsListener(_insetsRecycler, bottom: true));
            ViewCompat.SetOnApplyWindowInsetsListener(_rootView, new InsetsListener(_insetsRootView, bottom: true));
            RequestApplyInsetsWhenAttached(_recycler);
            RequestApplyInsetsWhenAttached(_rootView);
        }

        public void RequestApplyInsetsWhenAttached(View view)
        {
            if (view.IsAttachedToWindow)
                ViewCompat.RequestApplyInsets(view);
            else
                view.AddOnAttachStateChangeListener(new InsetsAttachStateChangeListener());
        }
    }

    public class CustomViewInsets : IViewInsets
    {
        private readonly View _view;
        private readonly BaseApplyWindowInsetsListener _insetsListener;

        public CustomViewInsets(View view, BaseApplyWindowInsetsListener insetsListener)
        {
            _view = view;
            _insetsListener = insetsListener;
        }

        public void Apply()
        {
            if (_view == null)
                return;

            ViewCompat.SetOnApplyWindowInsetsListener(_view, _insetsListener);
            RequestApplyInsetsWhenAttached(_view);
        }

        public void RequestApplyInsetsWhenAttached(View view)
        {
            if (view.IsAttachedToWindow)
                ViewCompat.RequestApplyInsets(view);
            else
                view.AddOnAttachStateChangeListener(new InsetsAttachStateChangeListener());
        }
    }
}