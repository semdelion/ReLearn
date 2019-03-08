using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReLearn.Droid.Statistics
{
    public class BaseStatistics
    {
        protected readonly float _width;
        protected readonly float _height;
        protected Canvas Canvas { get; set; }

        public BaseStatistics(Canvas canvas)
        {
            _width = canvas.Width;
            _height = canvas.Height;
            Canvas = canvas;
        }
    }
}