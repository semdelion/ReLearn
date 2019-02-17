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
using ReLearn.API.Database;
using ReLearn.Droid.Helpers;
using static Android.Graphics.Shader;

namespace ReLearn.Droid.Statistics
{
    public class DrawStatistics: BaseStatistics
    {
        public DrawStatistics(Canvas Canvas) : base(Canvas) { }

        public void DrawBackground(
            float roundXDP = 0, 
            float roundYDP = 0, 
            Paint background = null, 
            Paint border = null,
            Paint gradient = null)
        {
            if (background != null)
                Canvas.DrawRoundRect(
                    new RectF(0, 0, _width, _height), 
                    PixelConverter.DpToPX(roundXDP), 
                    PixelConverter.DpToPX(roundYDP), 
                    background );
            if (gradient != null)
                Canvas.DrawRoundRect(
                    new RectF(0, 0, _width, _height),
                    PixelConverter.DpToPX(roundXDP), 
                    PixelConverter.DpToPX(roundYDP), 
                    gradient );
            if (border != null)
                Canvas.DrawRoundRect(
                    new RectF(0 + border.StrokeWidth/2f, 
                    0 + border.StrokeWidth / 2f, 
                    _width - border.StrokeWidth / 2f, 
                    _height - border.StrokeWidth / 2f), 
                    PixelConverter.DpToPX(roundXDP), 
                    PixelConverter.DpToPX(roundYDP), 
                    border );
        }

        public void ProgressLine(
            float True, float False, 
            Color StartGradient, Color EndGradient, 
            Paint FalseLine, 
            float paddingLeftRight = 5f, 
            float heightLine = 18f, 
            float paddingBottom = 15f)
        {
            Shader shader = new LinearGradient(
                paddingLeftRight * _width / 100f, 0, 
                _width - 2f * paddingLeftRight * _width / 100f, 0,
                EndGradient, StartGradient, TileMode.Clamp);
            Paint TrueLine = new Paint { AntiAlias = true };
            TrueLine.SetShader(shader);
            ProgressLine(True, False, TrueLine, FalseLine, paddingLeftRight, heightLine, paddingBottom);
        }

        public void ProgressLine(
            float True, float False, 
            Paint TrueLine, Paint FalseLine,
            float paddingLeftRight = 5f, 
            float heightLine = 18f, 
            float paddingBottom = 15f)
        {
            paddingLeftRight *= _width / 100f;
            paddingBottom *= _height / 100f;
            heightLine *= _height / 100f;
            float step = (_width - paddingLeftRight * 2) / (True + False);

            float y0 = _height - paddingBottom - heightLine;
            float y1 = _height - paddingBottom;

            Canvas.DrawRect(new RectF(paddingLeftRight, y0, paddingLeftRight + step * True, y1), TrueLine);
            Canvas.DrawRect(new RectF(paddingLeftRight + step * True, y0, _width - paddingLeftRight, y1), FalseLine);
        }
    }
}