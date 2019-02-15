using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform.Droid.Platform;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Statistics;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Statistics;
using ReLearn.Droid.Views.Fragments;
using System;
using System.Collections.Generic;

namespace ReLearn.Droid.Views.Statistics
{
    public class TabMainFragment : MvxFragment<MainStatisticsViewModel>
    {
        private List<DatabaseStatistics> Database { get;  set; } = null;
        private int? True { get; set; } = null;
        private int? False { get; set; } = null;

        private void CreateLastStat(LinearLayout viewLastStat)
        {
            using (Bitmap bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20), 
                PixelConverter.DpToPX(70), Bitmap.Config.Argb8888))
            {
                Canvas canvasLastStat = new Canvas(bitmapLastStat);
                var lastStat = new DrawStatistics(canvasLastStat);
                lastStat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                lastStat.ProgressLine(True ?? 0, False ?? 1, StatisticsFragment.LightColor, StatisticsFragment.DarkColor, Paints.BackgroundLine);
                viewLastStat.Background = new BitmapDrawable(Resources, bitmapLastStat);
            }
        }

        private void CreateMainChart(LinearLayout viewLastStat)
        {
            using (Bitmap bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20), 
                Resources.DisplayMetrics.HeightPixels - PixelConverter.DpToPX(150), 
                Bitmap.Config.Argb8888))
            {
                Canvas canvasLastStat = new Canvas(bitmapLastStat);
                var mainChart = new DrawStatistics(canvasLastStat);
                mainChart.DrawChart(Database, StatisticsFragment.LightColor, StatisticsFragment.DarkColor, 15,15);
                viewLastStat.Background = new BitmapDrawable(Resources, bitmapLastStat);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //return new GraphStatistics(inflater.Inflate(
            //    Resource.Layout.TESTmainSTATISTICS, container, false).Context,
            //    StatisticsFragment.LightColor, 
            //    StatisticsFragment.DarkColor, 
            //    DataBase.TableName.ToString()
            //    );
            Database = DBStatistics.GetData(DataBase.TableName.ToString());
            if (Database.Count != 0)
            {
                True = Database[Database.Count - 1].True;
                False = Database[Database.Count - 1].False;
            }
            var view = inflater.Inflate(Resource.Layout.TESTmainSTATISTICS, container, false);
            var viewLastStat = view.FindViewById<LinearLayout>(Resource.Id.view_statistics_last_test);
            CreateLastStat(viewLastStat);
            var viewMainChart = view.FindViewById<LinearLayout>(Resource.Id.view_statistics_diagram);
            CreateMainChart(viewMainChart);
            return view;
        }
    }
}