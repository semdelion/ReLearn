﻿using System;
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
    public enum TypeDate
    {
        Percent,
        MaxNumber
    }

    public class DrawChart: BaseStatistics
    {
        private int _countOrdinate = 10;
        public int CountOrdinate
        {   get => _countOrdinate;
            set
            {
                if (value > 0)
                    _countOrdinate = value;
                else
                    throw new IndexOutOfRangeException("value must be greater than 0");
            }
        }

        private int _countAbscissa = 10;
        public int CountAbscissa
        { 
            get => _countAbscissa;
            set
            {
                if (value > 0)
                    _countAbscissa = value;
                else
                    throw new IndexOutOfRangeException("value must be greater than 0");
            }
        }

        public TypeDate OrdinateType { get; set; } = TypeDate.Percent;
        public float MaxNumber { get; set; } = 1;
        public int RoundMaxNumber { get; set; } = 0;

        Paint GraphLayoutPaint { get; set; } = new Paint {
            Color = Color.Argb(40, 215, 248, 254),
            StrokeWidth = 1,
            AntiAlias = true };

        public DrawChart(Canvas Canvas) : base(Canvas){ }

        protected virtual void Abscissa(float left, float bottom, float width, Paint paint, int amount)
        {
            Canvas.DrawLine(left, bottom, left + width, bottom, paint);

            float step = width / CountAbscissa;
            for (int i = 0; i <= CountAbscissa; i++)
                Canvas.DrawLine(left + step * i, bottom, left + step * i, bottom + 1.5f * (_width + _height) / 200, paint);

            int countDate = amount >= CountAbscissa ? CountAbscissa : amount,
                stat = amount >= CountAbscissa ? amount - CountAbscissa : 0;
            for (int i = 1; i <= countDate; i++)
                Canvas.DrawText((stat + i).ToString(),
                    left - (stat + i).ToString().Length * 0.7f * (_width + _height) / 200 + step * i,
                    bottom + 4 * (_width + _height) / 200, paint);
        }

        protected virtual void Ordinate(float left, float bottom, float height, Paint paint)
        {
            Canvas.DrawLine(left, bottom, left, bottom - height, paint);

            float step = height / CountOrdinate;
            for (int i = 0; i <= CountOrdinate; i++)
                Canvas.DrawLine(
                    left, 
                    bottom - step * i, 
                    left - 1.5f * (_width + _height) / 200, 
                    bottom - step * i, 
                    paint);
            float date = TypeDate.MaxNumber == OrdinateType ? MaxNumber: 100f / CountOrdinate;
            for (int i = 0; i <= CountOrdinate; i++)
            {
                var text = $"{string.Format("{0:N" + $"{RoundMaxNumber}" + "}", i * date)}"
                    + (TypeDate.MaxNumber == OrdinateType ? "" : "%");
                Canvas.DrawText(text, left - 7.5f * (_width + _height) / 200,
                    bottom - step * (i) + (_width + _height) / 200, paint);
            }
        }

        protected virtual void GraphLayout(float left, float bottom, float top, float right)
        {
            float height = (bottom - top) / CountOrdinate;
            for (int j = 1; j <= CountOrdinate; j += 1)
                Canvas.DrawLine(left, bottom - height * j, right, bottom - height * j, GraphLayoutPaint);
        }

        protected virtual void Diagram(List<DatabaseStatistics> Database_Stat, 
            float left, float right, float bottom, float top, 
            Color start, Color end)
        {
            float step_width = (right - left) / CountAbscissa,
                  step_height = (bottom - top),
                  padding = 2f; // between columns
            int i = 0, n_count = Database_Stat.Count - CountAbscissa;
            foreach (var s in Database_Stat)
            {
                if (i >= n_count)
                {
                    Shader shader = new LinearGradient(
                        left + 2f, bottom - step_height, 
                        left + step_width - 2f, bottom, 
                        start, end, TileMode.Clamp);
                    Paint paint = new Paint { AntiAlias = true };
                    paint.SetShader(shader);
                    Canvas.DrawRoundRect(
                        new RectF(
                            left + padding, 
                            bottom - ((step_height / (s.True + s.False)) * s.True), 
                            left + step_width - padding, 
                            bottom), 
                        0, 0, paint);
                    left = left + step_width;
                }
                ++i;
            }
        }

        public virtual void DoDrawChart(List<DatabaseStatistics> Database, Color start, Color end, Paint color)
        {
            float
                left = 0.1f * _width,               // pading 10%
                right = _width - 0.05f * _width,    // pading 5%
                bottom = _height - 0.12f * _height, // pading 12%
                top = 0.03f * _height;              // pading 3%

            Ordinate(left - 0.01f * _width, bottom, bottom - top, color);
            Abscissa(left, bottom + 0.01f * _width, right - left, color, Database.Count);
            GraphLayout(left, bottom, top, right);
            Diagram(Database, left, right, bottom, top, start, end);
        }
    }
}