using Android.Util;

namespace ReLearn.Droid.Helpers
{
    public static class PixelConverter
    {
        public static int DpToPX(float dp) => (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, Android.App.Application.Context.Resources.DisplayMetrics);
    }
}