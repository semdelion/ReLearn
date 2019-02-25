using Android.Graphics;
using Android.Graphics.Drawables;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Statistics;
using static Android.Graphics.Shader;

namespace ReLearn.Droid
{
    public static class Background
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
    }
}