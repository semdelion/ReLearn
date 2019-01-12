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
using static Android.Graphics.PorterDuff;

namespace ReLearn.Droid
{
    public static class BitmapHandler
    {
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