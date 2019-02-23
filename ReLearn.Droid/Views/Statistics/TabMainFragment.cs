using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Animation;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Statistics;
using ReLearn.Droid.Views.Fragments;
using System;

namespace ReLearn.Droid.Views.Statistics
{
    public class TabMainFragment : MvxFragment<MainStatisticsViewModel>
    {
        private static LinearLayout ViewMainChart;

        private void CreateLastStat(LinearLayout viewLastStat)
        {
            using (Bitmap bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20), 
                PixelConverter.DpToPX(70), Bitmap.Config.Argb4444))
            {
                Canvas canvasLastStat = new Canvas(bitmapLastStat);
                var lastStat = new DrawStatistics(canvasLastStat);
                lastStat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                lastStat.ProgressLine(ViewModel.True ?? 0, ViewModel.False ?? 1, StatisticsFragment.LightColor, StatisticsFragment.DarkColor, Paints.BackgroundLine);
                viewLastStat.Background = new BitmapDrawable(Resources, bitmapLastStat);
            }
        }

        private void CreateMainChart(int abscissa = 10, int count = 10)
        {
            using (Bitmap bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20),
                Resources.DisplayMetrics.HeightPixels - PixelConverter.DpToPX(150),
                Bitmap.Config.Argb4444))
            {
                Canvas canvasLastStat = new Canvas(bitmapLastStat);
                var mainChart = new DrawChart(canvasLastStat);
                mainChart.StepAbscissa = count;
                mainChart.CountAbscissa = abscissa;
                mainChart.DoDrawChart(ViewModel.Database, StatisticsFragment.LightColor, StatisticsFragment.DarkColor, Paints.Text);
                ViewMainChart.Background = new BitmapDrawable(Resources, bitmapLastStat);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_tab_statistics_main, container, false);
            var viewLastStat = view.FindViewById<LinearLayout>(Resource.Id.view_statistics_last_test);
            CreateLastStat(viewLastStat);
            ViewMainChart = view.FindViewById<LinearLayout>(Resource.Id.view_statistics_diagram);
            CreateMainChart();
            ViewMainChart.Touch += ChartClick;


            return view;
        }

        public void ChartClick(object sender, EventArgs e)
        {
            var flingAnimationX = new SpringAnimation(ViewMainChart, DynamicAnimation.ScaleX, 0);
            var flingAnimationY = new SpringAnimation(ViewMainChart, DynamicAnimation.ScaleY, 0);
            flingAnimationX.AnimateToFinalPosition(0.9f);
            flingAnimationX.Start();
            flingAnimationY.AnimateToFinalPosition(0.9f);
            flingAnimationY.Start();
            CreateMainChart(20,10);
        }
    }
}

