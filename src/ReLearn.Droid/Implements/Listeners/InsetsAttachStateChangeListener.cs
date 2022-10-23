using Android.Views;
using AndroidX.Core.View;

namespace ReLearn.Droid.Implements.Listeners
{
    public class InsetsAttachStateChangeListener : Java.Lang.Object, View.IOnAttachStateChangeListener
    {
        public void OnViewAttachedToWindow(View attachedView)
        {
            var insets = ViewCompat.GetRootWindowInsets(attachedView).ToWindowInsets();
            attachedView.RemoveOnAttachStateChangeListener(this);
            attachedView.OnApplyWindowInsets(insets);
        }

        public void OnViewDetachedFromWindow(View detachedView)
        {

        }
    }
}