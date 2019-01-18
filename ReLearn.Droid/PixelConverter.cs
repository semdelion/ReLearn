using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ReLearn.Droid
{
    public static class PixelConverter
    {
        public static int DpToPX(float dp) => (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, Android.App.Application.Context.Resources.DisplayMetrics);
    }
}