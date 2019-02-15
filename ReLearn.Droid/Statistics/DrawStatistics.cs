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
    public class DrawStatistics
    {
        private readonly float _width;
        private readonly float _height;
        public Canvas Canvas { get; set; }

        public DrawStatistics(Canvas canvas)
        {
            _width = canvas.Width;
            _height = canvas.Height;
            Canvas = canvas;
        }

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

        private void Abscissa(float left, float bottom, float width, Paint paint, int amount, int count)
        {
            Canvas.DrawLine(left, bottom, left + width, bottom, paint);

            float step = width / count;
            for (int i = 0; i <= count; i++)
                Canvas.DrawLine(left + step * i, bottom, left + step * i, bottom + 1.5f * (Canvas.Width + Canvas.Height) / 200, paint);

            int countDate = amount >= count ? count : amount,
                stat = amount >= count ? amount - count : 0;
            for (int i = 1; i <= countDate; i++)
                Canvas.DrawText((stat + i).ToString(), 
                    left - (stat + i).ToString().Length * 0.7f * (Canvas.Width + Canvas.Height) / 200 + step * i, 
                    bottom + 4 * (Canvas.Width + Canvas.Height) / 200, paint);
        }

        private void Ordinate(float left, float bottom, float height, Paint paint, int count)
        {
            Canvas.DrawLine(left, bottom, left, bottom - height, paint);

            float step = height / count;
            for (int i = 0; i <= count; i++)
                Canvas.DrawLine(left, bottom - step * i, left - 1.5f * (Canvas.Width + Canvas.Height) / 200, bottom - step * i, paint);

            float percent = 100f / count;

            for (int i = 0; i <= count; i++)
                Canvas.DrawText($"{string.Format("{0:N0}", i * percent)}%", left - 7.5f * (Canvas.Width + Canvas.Height) / 200,
                    bottom - step * (i) +  (Canvas.Width + Canvas.Height) / 200, paint);
        }

        private void GraphLayout(float left, float bottom, float top, float right, int count)
        {
            float height = (bottom - top) / count;
            Paint paint = new Paint {Color = Color.Argb(40, 215, 248, 254), StrokeWidth = 1, AntiAlias = true };
            for (int j = 1; j <= count; j += 1)
                Canvas.DrawLine(left, bottom - height * j, right, bottom - height * j, paint);
        }

        private void Diagram(List<DatabaseStatistics> Database_Stat, float left, float right, float bottom, float top, Color start, Color end, int count)
        {
            float step_width = (right - left) / count,
                  step_height = (bottom - top),
                  padding = 2f; // between columns
            int i = 0, n_count = Database_Stat.Count - count;
            foreach (var s in Database_Stat)
            {
                if (i >= n_count)
                {
                    Shader shader = new LinearGradient(left + 2f, bottom - step_height, left + step_width - 2f, bottom, start, end, TileMode.Clamp);
                    Paint paint = new Paint { AntiAlias = true };
                    paint.SetShader(shader);
                    Canvas.DrawRoundRect(new RectF(left + padding, bottom - ((step_height / (s.True + s.False)) * s.True), left + step_width - padding, bottom), 0, 0, paint);
                    left = left + step_width;
                }
                ++i;
            }
        }

        public void DrawChart(List<DatabaseStatistics> Database, 
            Color start, Color end, 
            int countOrdinate = 10, 
            int countAbscissa = 10)
        {
            float
                left =  0.1f * _width,//pading 10%
                right = _width - 0.05f * _width, // pading 5%
                bottom = _height - 0.12f * _height, // pading 12%
                top = 0.03f * _height;// pading 3%

            Ordinate(left - 0.01f * _width, bottom, bottom - top, Paints.Text, countOrdinate);
            Abscissa(left, bottom + 0.01f * _width, right - left, Paints.Text, Database.Count, countAbscissa);
            GraphLayout(left, bottom, top, right, countOrdinate);
            Diagram(Database, left, right, bottom, top, start, end, countAbscissa);
        }
    }
}