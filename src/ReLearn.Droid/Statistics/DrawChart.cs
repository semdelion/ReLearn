using Android.Graphics;
using ReLearn.API.Database;
using System;
using System.Collections.Generic;
using static Android.Graphics.Shader;

namespace ReLearn.Droid.Statistics
{
    public enum TypeDate
    {
        Percent,
        MaxNumber
    }

    public class DrawChart : BaseStatistics
    {
        private int _countAbscissa = 10;
        private int _countOrdinate = 10;

        private int? _stepAbscissa;

        public DrawChart(Canvas Canvas) : base(Canvas)
        {
        }

        public int CountOrdinate
        {
            get => _countOrdinate;
            set => _countOrdinate =
                value > 0 ? value : throw new IndexOutOfRangeException("value must be greater than 0");
        }

        public int CountAbscissa
        {
            get => _countAbscissa;
            set => _countAbscissa =
                value > 0 ? value : throw new IndexOutOfRangeException("value must be greater than 0");
        }

        public int? StepAbscissa
        {
            get => _stepAbscissa;
            set => _stepAbscissa =
                value > 0 ? value : throw new IndexOutOfRangeException("value must be greater than 0");
        }

        public TypeDate OrdinateType { get; set; } = TypeDate.Percent;
        public float MaxNumber { get; set; } = 1;
        public int RoundMaxNumber { get; set; } = 0;

        private Paint GraphLayoutPaint { get; } = new Paint
        {
            Color = Color.Argb(40, 215, 248, 254),
            StrokeWidth = 1,
            AntiAlias = true
        };

        protected virtual void Abscissa(float left, float bottom, float width, Paint paint, int amount)
        {
            Canvas.DrawLine(left, bottom, left + width, bottom, paint);

            var step = width / (StepAbscissa ?? CountAbscissa);
            for (var i = 0; i <= (StepAbscissa ?? CountAbscissa); i++)
            {
                Canvas.DrawLine(left + step * i, bottom, left + step * i, bottom + 1.5f * (_width + _height) / 200,
                    paint);
            }

            var stepNumber = CountAbscissa / (StepAbscissa ?? CountAbscissa);
            if (stepNumber < 1)
            {
                stepNumber = 1;
            }

            int countDate = amount / stepNumber >= (StepAbscissa ?? CountAbscissa)
                    ? StepAbscissa ?? CountAbscissa
                    : amount / stepNumber,
                stat = amount >= CountAbscissa ? amount - CountAbscissa : 0;

            for (var i = 1; i <= countDate + (amount % stepNumber > 0 ? 1 : 0); i++)
            {
                Canvas.DrawText($"{stat + i * stepNumber}",
                    left - $"{stat + i}".Length * 0.7f * (_width + _height) / 200 + step * i,
                    bottom + 4 * (_width + _height) / 200, paint);
            }
        }

        protected virtual void Ordinate(float left, float bottom, float height, Paint paint)
        {
            Canvas.DrawLine(left, bottom, left, bottom - height, paint);

            var step = height / CountOrdinate;
            for (var i = 0; i <= CountOrdinate; i++)
            {
                Canvas.DrawLine(
                    left,
                    bottom - step * i,
                    left - 1.5f * (_width + _height) / 200,
                    bottom - step * i,
                    paint);
            }

            var date = TypeDate.MaxNumber == OrdinateType ? MaxNumber : 100f / CountOrdinate;
            for (var i = 0; i <= CountOrdinate; i++)
            {
                var text = $"{string.Format("{0:N" + $"{RoundMaxNumber}" + "}", i * date)}"
                           + (TypeDate.MaxNumber == OrdinateType ? "" : "%");
                Canvas.DrawText(text, left - 7.5f * (_width + _height) / 200,
                    bottom - step * i + (_width + _height) / 200, paint);
            }
        }

        protected virtual void GraphLayout(float left, float bottom, float top, float right)
        {
            var height = (bottom - top) / CountOrdinate;
            for (var j = 1; j <= CountOrdinate; j += 1)
            {
                Canvas.DrawLine(left, bottom - height * j, right, bottom - height * j, GraphLayoutPaint);
            }
        }

        protected virtual void Diagram(List<DatabaseStatistics> DatabaseStat,
            float left, float right, float bottom, float top,
            Color start, Color end)
        {
            float stepWidth = (right - left) / CountAbscissa,
                stepHeight = bottom - top,
                padding = 1f; // between columns
            int i = 0, count = DatabaseStat.Count - CountAbscissa;
            foreach (var s in DatabaseStat)
            {
                if (i >= count)
                {
                    Shader shader = new LinearGradient(
                        left + 2f, bottom - stepHeight,
                        left + stepWidth - 2f, bottom,
                        start, end, TileMode.Clamp);
                    var paint = new Paint { AntiAlias = true };
                    paint.SetShader(shader);
                    Canvas.DrawRoundRect(
                        new RectF(
                            left + padding,
                            bottom - stepHeight / (s.True + s.False) * s.True,
                            left + stepWidth - padding,
                            bottom),
                        0, 0, paint);
                    left += stepWidth;
                }

                ++i;
            }
        }

        public virtual void DoDrawChart(List<DatabaseStatistics> Database, Color start, Color end, Paint color)
        {
            float
                left = 0.12f * _width, // pading 12%
                right = _width - 0.05f * _width, // pading 5%
                bottom = _height - 0.12f * _height, // pading 12%
                top = 0.03f * _height; // pading 3%

            Ordinate(left - 0.01f * _width, bottom, bottom - top, color);
            Abscissa(left, bottom + 0.01f * _width, right - left, color, Database.Count);
            GraphLayout(left, bottom, top, right);
            Diagram(Database, left, right, bottom, top, start, end);
        }
    }
}