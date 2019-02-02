using Android.Graphics;
using static Android.Graphics.Shader;

namespace ReLearn.Droid
{
    public static class Background
    {
        public static Bitmap GetBackgroung(float width, float height)
        {
            float rounding = PixelConverter.DpToPX(10);
            float stepGrad = PixelConverter.DpToPX(4);
            float strokeWidth = PixelConverter.DpToPX(2);

            var backgroundPaint = new Paint { Color = new Color(Color.Argb(100, 16, 19, 38)), AntiAlias = true };
            var borderPaint = new Paint { StrokeWidth = strokeWidth, Color = Colors.White, AntiAlias = true };
            borderPaint.SetStyle(Paint.Style.Stroke);
            var bitmap = Bitmap.CreateBitmap((int)width, (int)height, Bitmap.Config.Argb4444);
            var canvas = new Canvas(bitmap);
            LinearGradient backlg = new LinearGradient(stepGrad, 0, 0, stepGrad,
                Color.Argb(80, 60, 90, 125), Color.Transparent, TileMode.Repeat);
            Paint gradientBackgroundPaint = new Paint { AntiAlias = true };
            gradientBackgroundPaint.SetShader(backlg);
            canvas.DrawRoundRect(new RectF(strokeWidth / 2, strokeWidth / 2, width - strokeWidth, height - strokeWidth), rounding, rounding, gradientBackgroundPaint);
            canvas.DrawRoundRect(new RectF(strokeWidth / 2, strokeWidth / 2, width - strokeWidth, height - strokeWidth), rounding, rounding, backgroundPaint);
            canvas.DrawRoundRect(new RectF(strokeWidth / 2, strokeWidth / 2, width - strokeWidth, height - strokeWidth), rounding, rounding, borderPaint);
            return bitmap;
        }
    }
}