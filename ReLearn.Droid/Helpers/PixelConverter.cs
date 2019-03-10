using Android.Util;
using Android.Widget;

namespace ReLearn.Droid.Helpers
{
    public static class PixelConverter
    {
        public static int DpToPX(float dp) => (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, Android.App.Application.Context.Resources.DisplayMetrics);

        public static LinearLayout.LayoutParams GetParams(int width, int height,
            int marginLeft = 0, int marginTop = 0, int marginRight = 0, int marginBottom = 0)
        {
            return new LinearLayout.LayoutParams(width, height)
            {
                TopMargin = DpToPX(marginTop),
                BottomMargin = DpToPX(marginBottom),
                RightMargin = DpToPX(marginRight),
                LeftMargin = DpToPX(marginLeft)
            };
        }
        public static RelativeLayout.LayoutParams GetParamsRelative(int width, int height,
           int marginLeft = 0, int marginTop = 0, int marginRight = 0, int marginBottom = 0)
        {
            return new RelativeLayout.LayoutParams(width, height)
            {
                TopMargin = DpToPX(marginTop),
                BottomMargin = DpToPX(marginBottom),
                RightMargin = DpToPX(marginRight),
                LeftMargin = DpToPX(marginLeft)
            };
        }
    }
}