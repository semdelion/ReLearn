using Android.Graphics;
using Android.Graphics.Drawables;
using ReLearn.Droid.Statistics;

namespace ReLearn.Droid.Helpers
{
    public static class BitmapHelper
    {
        public static BitmapDrawable GetBackgroung(Android.Content.Res.Resources resources, float width, float height)
        {
            using (var bitmap = Bitmap.CreateBitmap((int)width, (int)height, Bitmap.Config.Argb4444))
            using (var canvas = new Canvas(bitmap))
            {
                var background = new DrawStatistics(canvas);
                background.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                return new BitmapDrawable(resources, bitmap);
            }
        }

        public static Bitmap GetRoundedCornerBitmap(Bitmap bitmap, int pixels)
        {
            var output = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, Bitmap.Config.Argb4444);
            using (var canvas = new Canvas(output))
            using (var rect = new Rect(0, 0, bitmap.Width, bitmap.Height))
            using (var rectF = new RectF(rect))
            {
                var paint = new Paint
                {
                    AntiAlias = true,
                    Color = Color.AliceBlue
                };
                canvas.DrawARGB(0, 0, 0, 0);
                canvas.DrawRoundRect(rectF, pixels, pixels, paint);
                paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
                canvas.DrawBitmap(bitmap, rect, rect, paint);
                return output;
            }
        }
    }
}