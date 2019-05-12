using System.Collections.Generic;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Statistics;

namespace ReLearn.Droid.Views.SelectDictionary
{
    public class Dictionaries
    {
        public Dictionaries(int width)
        {
            DictionariesBitmap = new List<Bitmap>();
            DictionariesView = new List<ImageView>();
            Width = width;
            ParmsImage = new LinearLayout.LayoutParams(width, width)
            {
                Gravity = GravityFlags.Center, TopMargin = 10
            };
        }

        public int Width { get; set; }
        public List<Bitmap> DictionariesBitmap { get; set; }
        public List<ImageView> DictionariesView { get; set; }

        public LinearLayout.LayoutParams ParmsImage { get; set; }

        public static Bitmap CreateSingleImageFromMultipleImages(Bitmap firstImage, Bitmap secondImage, PointF C)
        {
            var result = Bitmap.CreateBitmap(firstImage.Width, firstImage.Height, firstImage.GetConfig());
            using (var canvas = new Canvas(result))
            {
                canvas.DrawBitmap(firstImage, 0f, 0f, null);
                canvas.DrawBitmap(secondImage, C.X, C.Y, null);
                return result;
            }
        }

        public void Selected(string newTableName, string currentTableName)
        {
            var indexCurrent = DictionariesView.FindIndex(s => $"{s.Tag}" == currentTableName);
            DictionariesView[indexCurrent].SetImageBitmap(DictionariesBitmap[indexCurrent]);

            using (var image1 = Bitmap.CreateBitmap(Width, Width, Bitmap.Config.Argb4444))
            using (var baseCan = new Canvas(image1))
            {
                var paint2 = new Paint {Color = Colors.FrameBorder, AntiAlias = true};
                baseCan.DrawCircle(Width / 2, Width / 2, Width / 2.5f, paint2);
                var indexNew = DictionariesView.FindIndex(s => $"{s.Tag}" == newTableName);
                DictionariesView[indexNew]
                    .SetImageBitmap(CreateSingleImageFromMultipleImages(image1, DictionariesBitmap[indexNew],
                        new PointF(0, 0)));
            }
        }

        public Bitmap CreateBitmapWithStats(Bitmap image, List<DBStatistics> database, Color start, Color end)
        {
            using (var image1 = Bitmap.CreateBitmap(Width, Width, Bitmap.Config.Argb4444))
            {
                float WidthLine = image1.Width / 10;
                using (var image2 = Bitmap.CreateScaledBitmap(image, (int) (Width / 2.5 * 2 - WidthLine),
                    (int) (Width / 2.5 * 2 - WidthLine), false))
                using (var baseCan = new Canvas(image1))
                {
                    var pieChart = new DrawStatistics(baseCan);
                    pieChart.DrawPieChart(API.Statistics.GetAverageNumberLearn(database),
                        Settings.StandardNumberOfRepeats, start, end, 0.13f, (float) (baseCan.Height / 2.5), false);
                    return CreateSingleImageFromMultipleImages(image1, image2,
                        new PointF(image1.Width / 2 - (float) (baseCan.Height / 2.5) + WidthLine / 2,
                            image1.Width / 2 - (float) (baseCan.Height / 2.5) + WidthLine / 2));
                }
            }
        }
    }
}