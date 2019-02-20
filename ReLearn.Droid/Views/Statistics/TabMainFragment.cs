using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platforms.Android.Binding.BindingContext;
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
        private void CreateLastStat(LinearLayout viewLastStat)
        {
            using (Bitmap bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20), 
                PixelConverter.DpToPX(70), Bitmap.Config.Argb8888))
            {
                Canvas canvasLastStat = new Canvas(bitmapLastStat);
                var lastStat = new DrawStatistics(canvasLastStat);
                lastStat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                lastStat.ProgressLine(ViewModel.True ?? 0, ViewModel.False ?? 1, StatisticsFragment.LightColor, StatisticsFragment.DarkColor, Paints.BackgroundLine);
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
                var mainChart = new DrawChart(canvasLastStat);
                mainChart.DoDrawChart(ViewModel.Database, StatisticsFragment.LightColor, StatisticsFragment.DarkColor, Paints.Text);
                viewLastStat.Background = new BitmapDrawable(Resources, bitmapLastStat);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_tab_statistics_main, container, false);
            var viewLastStat = view.FindViewById<LinearLayout>(Resource.Id.view_statistics_last_test);
            CreateLastStat(viewLastStat);
            var viewMainChart = view.FindViewById<LinearLayout>(Resource.Id.view_statistics_diagram);
            CreateMainChart(viewMainChart);
            return view;
        }
    }
}