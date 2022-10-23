using Android.Views;
using AndroidX.Core.View;

namespace ReLearn.Droid.Implements.Listeners
{
    public abstract class BaseApplyWindowInsetsListener : Java.Lang.Object, IOnApplyWindowInsetsListener
    {
        private readonly bool _left;
        private readonly bool _top;
        private readonly bool _right;
        private readonly bool _bottom;

        private PaddingsRect Paddings { get; set; }

        protected abstract int InsetTypes { get; set; }

        public BaseApplyWindowInsetsListener(bool left, bool top, bool right, bool bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        public virtual WindowInsetsCompat OnApplyWindowInsets(View view, WindowInsetsCompat insets)
        {
            Paddings ??= new PaddingsRect(view);
            var currentInsets = insets.GetInsets(InsetTypes);
            view.SetPadding(
                Paddings.Left + (_left ? currentInsets.Left : 0),
                Paddings.Top + (_top ? currentInsets.Top : 0),
                Paddings.Right + (_right ? currentInsets.Right : 0),
                Paddings.Bottom + (_bottom ? currentInsets.Bottom : 0));

            return insets;
        }

        protected class PaddingsRect
        {
            public int Left = 0;
            public int Top = 0;
            public int Right = 0;
            public int Bottom = 0;

            public PaddingsRect(View view)
            {
                Left = view.PaddingLeft;
                Top = view.PaddingTop;
                Right = view.PaddingRight;
                Bottom = view.PaddingBottom;
            }
        }
    }

    public class InsetsListener : BaseApplyWindowInsetsListener
    {
        protected override int InsetTypes { get; set; } =
            WindowInsetsCompat.Type.DisplayCutout() | WindowInsetsCompat.Type.SystemBars();

        public InsetsListener(int insetTypes,
            bool left = false, bool top = false,
            bool right = false, bool bottom = false) : base(left, top, right, bottom)
        {
            InsetTypes = insetTypes;
        }
    }
}