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

namespace ReLearn
{
    class SelectDictionary
    {
        public List<Bitmap> DictionariesBitmap { get; set; }
        public List<ImageView> DictionariesView { get; set; }
        public int Width { get; set; }
        public LinearLayout.LayoutParams ParmsImage { get; set; } 

        public SelectDictionary(int width)
        {
            DictionariesBitmap = new List<Bitmap>();
            DictionariesView = new List<ImageView>();
            Width = width;
            ParmsImage  = new LinearLayout.LayoutParams(width, width)
            { Gravity = GravityFlags.Center, TopMargin = 10 };
        }

        public static Bitmap CreateSingleImageFromMultipleImages(Bitmap firstImage, Bitmap secondImage, PointF C)
        {
            using (Bitmap result = Bitmap.CreateBitmap(firstImage.Width, firstImage.Height, firstImage.GetConfig()))
            {
                Canvas canvas = new Canvas(result);
                canvas.DrawBitmap(firstImage, 0f, 0f, null);
                canvas.DrawBitmap(secondImage, C.X, C.Y, null);
                return result;
            }
        }

        public void Selected(string NewTableName, string СurrentTableName)
        {
            for (int i = 0; i < DictionariesView.Count; i++)
                if (DictionariesView[i].Tag.ToString() == СurrentTableName)
                {
                    DictionariesView[i].SetImageBitmap(DictionariesBitmap[i]);
                    break;
                }

            using (Bitmap image1 = Bitmap.CreateBitmap(Width, Width, Bitmap.Config.Argb4444))
            {
                Canvas baseCan = new Canvas(image1);
                Paint paint2 = new Paint { Color = Color.Argb(200, 215, 248, 254), AntiAlias = true };
                baseCan.DrawCircle(Width / 2, Width / 2, Width / 2.5f, paint2);
                for (int i = 0; i < DictionariesView.Count; i++)
                    if (DictionariesView[i].Tag.ToString() == NewTableName)
                    {
                        DictionariesView[i].SetImageBitmap(CreateSingleImageFromMultipleImages(image1, DictionariesBitmap[i], new PointF(0, 0)));
                        break;
                    }
            }
        }

        public Bitmap CreateBitmapWithStats(Bitmap image, List<DBStatistics> Database_NL_and_D)
        {
            try
            {
                using (Bitmap Image1 = Bitmap.CreateBitmap(Width, Width, Bitmap.Config.Argb4444))
                {
                    FrameStatistics FRAME = new FrameStatistics(0, 0, Image1.Width, Image1.Width, Color.Argb(150, 16, 19, 38));
                    float WidthLine = FRAME.Width / 10;
                    using (Bitmap Image2 = Bitmap.CreateScaledBitmap(image, (int)((Width / 2.5) * 2 - WidthLine), (int)((Width / 2.5) * 2 - WidthLine), false))
                    {
                        Canvas baseCan = new Canvas(Image1);
                        FRAME.DrawPieChart(baseCan, Statistics.GetAverageNumberLearn(Database_NL_and_D), Settings.StandardNumberOfRepeats,
                                           Color.Rgb(0, 255, 255), Color.Rgb(50, 60, 126), (float)(baseCan.Height / 2.5), WidthLine);
                        using (Bitmap finalImage = CreateSingleImageFromMultipleImages(Image1, Image2,
                            new PointF(((FRAME.Left + FRAME.Width) / 2) - (float)(baseCan.Height / 2.5) + WidthLine / 2,
                                       ((FRAME.Top + FRAME.Height) / 2) - (float)(baseCan.Height / 2.5) + WidthLine / 2)))
                        return finalImage;
                    }
                }
            }
            catch (OutOfMemoryException)
            {
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}