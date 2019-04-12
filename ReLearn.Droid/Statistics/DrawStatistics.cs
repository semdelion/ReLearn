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
            float @true, float @false, 
            Color startGradient, Color endGradient, 
            Paint falseLine, 
            float paddingLeftRight = 5f, 
            float heightLine = 18f, 
            float paddingBottom = 15f)
        {
            Shader shader = new LinearGradient(
                paddingLeftRight * _width / 100f, 0, 
                _width - 2f * paddingLeftRight * _width / 100f, 0,
                endGradient, startGradient, TileMode.Clamp);
            Paint trueLine = new Paint { AntiAlias = true };
            trueLine.SetShader(shader);
            ProgressLine(@true, @false, trueLine, falseLine, paddingLeftRight, heightLine, paddingBottom);
        }

        public void ProgressLine(
            float @true, float @false, 
            Paint trueLine, Paint falseLine,
            float paddingLeftRight = 5f, 
            float heightLine = 18f, 
            float paddingBottom = 15f)
        {
            paddingLeftRight *= _width / 100f;
            paddingBottom *= _height / 100f;
            heightLine *= _height / 100f;
            float step = (_width - paddingLeftRight * 2) / (@true + @false);

            float y0 = _height - paddingBottom - heightLine;
            float y1 = _height - paddingBottom;

            Canvas.DrawRect(new RectF(paddingLeftRight, y0, paddingLeftRight + step * @true, y1), trueLine);
            Canvas.DrawRect(new RectF(paddingLeftRight + step * @true, y0, _width - paddingLeftRight, y1), falseLine);
        }

        public void DrawPieChart(float average, float sum, Color start, Color end, float width = 0.13f, float radius = -1, bool text = true)
        {
            float Cx = _width / 2f,
                  Cy = _height / 2f,
                  strokeWidth = width * (_width <= _height ? _width : _height);
                  radius = radius <= 0 ? (_width <= _height ? _width / 2f : _height / 2f) - strokeWidth : radius;

            Shader shader = new SweepGradient(Cx, Cy, end, start);
            Paint paintMain = new Paint { Color = start, StrokeWidth = strokeWidth, AntiAlias = true };
            Paint paintBack = new Paint { Color = Color.Rgb(29, 43, 59), StrokeWidth = strokeWidth, AntiAlias = true };
            paintMain.SetStyle(Paint.Style.Stroke);
            paintMain.SetShader(shader);
            paintBack.SetStyle(Paint.Style.Stroke);

            Canvas.DrawArc(new RectF(Cx - radius, Cy - radius, Cx + radius, Cy + radius), 0f, 360f, false, paintBack);
            Canvas.Rotate(-90f, Cx, Cy);
            Canvas.DrawArc(new RectF(Cx - radius, Cy - radius, Cx + radius, Cy + radius), 0.5f, 360f - average * (360f / sum), false, paintMain);
            Canvas.Rotate(90f, Cx, Cy);
            if(text)
                DrawText(_width * 20f / 100f, $"{RoundOfNumber(100 - average * 100f / sum)}%",  2.4f * _width / 10f, Cy - 33f * radius / 100);
        }

        public void DrawText(float fontSize, string text, float left, float top, Color? c = null)
        {
            Typeface Plain = null;
            Typeface bold = Typeface.Create(Plain, TypefaceStyle.Normal);
            Paint paint = new Paint
            {
                AntiAlias = true,
                TextSize = fontSize,
                Color = c ?? Colors.White
            };
            paint.SetTypeface(bold);
            Canvas.DrawText(text, left, top + fontSize, paint);
        }

        private static string RoundOfNumber(float number)
        {
            var numberChar = Convert.ToString(number);
            if (numberChar.Length > 4)
                numberChar = numberChar.Remove(4);
            else if (numberChar.Contains(","))
                numberChar += "0";
            else
            {
                if (numberChar.Length == 2)
                    numberChar += ".0";
                else if (numberChar.Length == 1)
                    numberChar += ".00";
            }
            return numberChar;
        }
    }
}