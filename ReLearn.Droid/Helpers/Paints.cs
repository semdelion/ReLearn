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
using static Android.Graphics.Shader;

namespace ReLearn.Droid.Helpers
{
    public static class Paints
    {
        public readonly static Paint Border;
        public readonly static Paint Gradient;
        public readonly static Paint Background;
        public readonly static Paint BackgroundLine;
        public readonly static Paint Text;

        static Paints()
        {
            var stepGrad = PixelConverter.DpToPX(4);
            var strokeWidth = PixelConverter.DpToPX(2);
            
            LinearGradient gradient = new LinearGradient(stepGrad, 0, 0, stepGrad,
            Color.Argb(80, 60, 90, 125), Color.Transparent, TileMode.Repeat);
            Gradient = new Paint { AntiAlias = true };
            Gradient.SetShader(gradient);
            Text = new Paint { StrokeWidth = 4, Color = Colors.White, AntiAlias = true };
            Text.TextSize = PixelConverter.DpToPX(11);
            BackgroundLine = new Paint { Color = Color.Rgb(29, 43, 59), AntiAlias = true };
            Background = new Paint { Color = Color.Argb(150, 16, 19, 38), AntiAlias = true };
            Border = new Paint { StrokeWidth = strokeWidth, Color = Colors.White, AntiAlias = true };
            Border.SetStyle(Paint.Style.Stroke);
        }
    }
}