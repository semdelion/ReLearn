﻿using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.DynamicAnimation;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using ReLearn.API;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Statistics;
using System;
using static Android.Views.View;

namespace ReLearn.Droid.Views.Statistics
{
    public class TabMainFragment : MvxFragment<MainStatisticsViewModel>
    {
        private static LinearLayout ViewMainChart;
        private float HistoricalX;
        private float HistoricalY;

        private void CreateLastStat(LinearLayout viewLastStat)
        {
            using var bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20),
                PixelConverter.DpToPX(70), Bitmap.Config.Argb4444);
            using var canvasLastStat = new Canvas(bitmapLastStat);
            var lastStat = new DrawStatistics(canvasLastStat);
            lastStat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
            lastStat.ProgressLine(ViewModel.True ?? 0, ViewModel.False ?? 1, StatisticsFragment.LightColor,
                StatisticsFragment.DarkColor, Paints.BackgroundLine);
            viewLastStat.Background = new BitmapDrawable(Resources, bitmapLastStat);
        }

        private void CreateMainChart(int abscissa = 10, int count = 10)
        {
            using var bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20),
                Resources.DisplayMetrics.HeightPixels - PixelConverter.DpToPX(150),
                Bitmap.Config.Argb4444);
            using var canvasLastStat = new Canvas(bitmapLastStat);
            var mainChart = new DrawChart(canvasLastStat)
            {
                StepAbscissa = count,
                CountAbscissa = abscissa
            };
            mainChart.DoDrawChart(ViewModel.Database, StatisticsFragment.LightColor,
                StatisticsFragment.DarkColor, Paints.Text);
            using var back = new BitmapDrawable(Resources, bitmapLastStat);
            ViewMainChart.Background = back;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_tab_statistics_main, container, false);
            var viewLastStat = view.FindViewById<LinearLayout>(Resource.Id.view_statistics_last_test);
            CreateLastStat(viewLastStat);
            ViewMainChart = view.FindViewById<LinearLayout>(Resource.Id.view_statistics_diagram);
            CreateMainChart(ViewModel.AmountOfStatistics,
                10 + ViewModel.AmountOfStatistics % (ViewModel.AmountOfStatistics / 10 * 10) /
                (ViewModel.AmountOfStatistics / 10));
            ViewMainChart.Touch += ChartClick;
            return view;
        }

        public void ChartClick(object sender, TouchEventArgs e)
        {
            var flingAnimationX = new SpringAnimation(ViewMainChart, DynamicAnimation.ScaleX, 0);
            var flingAnimationY = new SpringAnimation(ViewMainChart, DynamicAnimation.ScaleY, 0);

            if (e.Event.Action == MotionEventActions.Down)
            {
                flingAnimationX.AnimateToFinalPosition(0.98f);
                flingAnimationY.AnimateToFinalPosition(0.98f);
                flingAnimationX.Start();
                flingAnimationY.Start();
                return;
            }

            flingAnimationX.AnimateToFinalPosition(1f);
            flingAnimationY.AnimateToFinalPosition(1f);
            flingAnimationX.Start();
            flingAnimationY.Start();

            if (e.Event.Action == MotionEventActions.Move)
            {
                if (e.Event.PointerCount == 2)
                {
                    var x = e.Event.GetX(0) - e.Event.GetX(1);
                    var y = e.Event.GetY(0) - e.Event.GetY(1);

                    var distanceOld = Math.Sqrt(Math.Pow(HistoricalX, 2) + Math.Pow(HistoricalY, 2));
                    var distanceNew = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

                    var min = Settings.MinNumberOfStatistics;
                    var max = Settings.MaxNumberOfStatistics;
                    var count = ViewModel.AmountOfStatistics;

                    if (distanceOld > distanceNew && count < max && count < ViewModel.Database.Count)
                    {
                        CreateMainChart(ViewModel.AmountOfStatistics += 1,
                            min + ViewModel.AmountOfStatistics % (ViewModel.AmountOfStatistics / min * min) /
                            (ViewModel.AmountOfStatistics / min));
                    }

                    if (distanceOld < distanceNew && count > min)
                    {
                        CreateMainChart(ViewModel.AmountOfStatistics -= 1,
                            min + ViewModel.AmountOfStatistics % (ViewModel.AmountOfStatistics / min * min) /
                            (ViewModel.AmountOfStatistics / min));
                    }

                    HistoricalX = x;
                    HistoricalY = y;
                }
            }
        }
    }
}