using Android.Graphics;
using Android.Views;
using Android.Widget;
using ReLearn.API;
using ReLearn.API.Database;
using System;
using System.Collections.Generic;

namespace ReLearn.Droid
{
    public class Dictionaries
    {
        public int Width { get; set; }
        public List<Bitmap> DictionariesBitmap { get; set; }
        public List<ImageView> DictionariesView { get; set; }
        
        public LinearLayout.LayoutParams ParmsImage { get; set; } 

        public Dictionaries(int width)
        {
            DictionariesBitmap = new List<Bitmap>();
            DictionariesView = new List<ImageView>();
            Width = width;
            ParmsImage  = new LinearLayout.LayoutParams(width, width) {
                Gravity = GravityFlags.Center, TopMargin = 10
            };
        }

        public static Bitmap CreateSingleImageFromMultipleImages(Bitmap firstImage, Bitmap secondImage, PointF C)
        {
            Bitmap result = Bitmap.CreateBitmap(firstImage.Width, firstImage.Height, firstImage.GetConfig());
            Canvas canvas = new Canvas(result);
            canvas.DrawBitmap(firstImage, 0f, 0f, null);
            canvas.DrawBitmap(secondImage, C.X, C.Y, null);
            return result;
        }

        public void Selected(string NewTableName, string СurrentTableName)
        {
            int indexCurrent = DictionariesView.FindIndex(s => s.Tag.ToString() == СurrentTableName);
            DictionariesView[indexCurrent].SetImageBitmap(DictionariesBitmap[indexCurrent]);

            using (Bitmap image1 = Bitmap.CreateBitmap(Width, Width, Bitmap.Config.Argb4444))
            {
                Canvas baseCan = new Canvas(image1);
                Paint paint2 = new Paint { Color = Colors.FrameBorder, AntiAlias = true };
                baseCan.DrawCircle(Width / 2, Width / 2, Width / 2.5f, paint2);
                int indexNew = DictionariesView.FindIndex(s => s.Tag.ToString() == NewTableName);
                DictionariesView[indexNew].SetImageBitmap(CreateSingleImageFromMultipleImages(image1, DictionariesBitmap[indexNew], new PointF(0, 0)));
            }
        }

        public Bitmap CreateBitmapWithStats(Bitmap image, List<DBStatistics> Database_NL_and_D, Color Start, Color End)
        {
            try
            {
                using (Bitmap Image1 = Bitmap.CreateBitmap(Width, Width, Bitmap.Config.Argb4444))
                {
                    FrameStatistics FRAME = new FrameStatistics(0, 0, Image1.Width, Image1.Width, Colors.FrameBackground);
                    float WidthLine = FRAME.Width / 10;
                    using (Bitmap Image2 = Bitmap.CreateScaledBitmap(image, (int)((Width / 2.5) * 2 - WidthLine), (int)((Width / 2.5) * 2 - WidthLine), false))
                    {
                        Canvas baseCan = new Canvas(Image1);
                        FRAME.DrawPieChart(baseCan, Statistics.GetAverageNumberLearn(Database_NL_and_D), Settings.StandardNumberOfRepeats,
                                           Start, End, (float)(baseCan.Height / 2.5), WidthLine);
                        Bitmap finalImage = CreateSingleImageFromMultipleImages(Image1, Image2,
                            new PointF(((FRAME.Left + FRAME.Width) / 2) - (float)(baseCan.Height / 2.5) + WidthLine / 2,
                                       ((FRAME.Top + FRAME.Height) / 2) - (float)(baseCan.Height / 2.5) + WidthLine / 2));
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