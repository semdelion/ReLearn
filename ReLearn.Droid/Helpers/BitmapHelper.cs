using Android.Graphics;
using Android.Graphics.Drawables;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Statistics;
using static Android.Graphics.PorterDuff;
using static Android.Graphics.Shader;

namespace ReLearn.Droid
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
            Bitmap output = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, Bitmap.Config.Argb4444);
            using (Canvas canvas = new Canvas(output))
            using (Rect rect = new Rect(0, 0, bitmap.Width, bitmap.Height))
            using (RectF rectF = new RectF(rect))
            {
                Paint paint = new Paint
                {
                    AntiAlias = true,
                    Color = Color.AliceBlue
                };
                canvas.DrawARGB(0, 0, 0, 0);
                canvas.DrawRoundRect(rectF, pixels, pixels, paint);
                paint.SetXfermode(new PorterDuffXfermode(Mode.SrcIn));
                canvas.DrawBitmap(bitmap, rect, rect, paint);
                return output;
            }
        }
    }
}