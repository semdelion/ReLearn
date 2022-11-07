﻿using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using ReLearn.API;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Statistics;
using System.Linq;

namespace ReLearn.Droid.Views.Statistics
{
    public class TabGeneralFragment : MvxFragment<GeneralStatisticsViewModel>, ViewTreeObserver.IOnPreDrawListener
    {
        public LinearLayout ViewDegreeOfStudy;
        public LinearLayout ViewPieChart;

        public bool OnPreDraw()
        {
            CreateViewDegreeOfStudy(ViewDegreeOfStudy, ViewPieChart);
            return true;
        }

        private void CreateViewAnswersRatio(LinearLayout viewLastStat)
        {
            using (var bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20),
                PixelConverter.DpToPX(65), Bitmap.Config.Argb4444))
            {
                using (var canvas = new Canvas(bitmapLastStat))
                {
                    var stat = new DrawStatistics(canvas);
                    stat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                    int numberTrue = ViewModel.Database.Sum(r => r.True),
                        numberFalse = ViewModel.Database.Sum(r => r.False);
                    stat.ProgressLine(numberTrue, numberFalse + numberTrue == 0 ? 1 : numberFalse,
                        StatisticsFragment.LightColor, StatisticsFragment.DarkColor, Paints.BackgroundLine);
                    using (var background = new BitmapDrawable(Resources, bitmapLastStat))
                    {
                        viewLastStat.Background = background;
                    }
                }
            }
        }

        private void CreateViewAkwardWord(LinearLayout viewLastStat)
        {
            using (var bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20),
                PixelConverter.DpToPX(65), Bitmap.Config.Argb4444))
            {
                using (var canvas = new Canvas(bitmapLastStat))
                {
                    var stat = new DrawStatistics(canvas);
                    stat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                    var numberInconvenient =
                        ViewModel.DatabaseStats.Count(r => r.NumberLearn == Settings.MaxNumberOfRepeats);
                    stat.ProgressLine(numberInconvenient, ViewModel.DatabaseStats.Count - numberInconvenient,
                        StatisticsFragment.LightColor, StatisticsFragment.DarkColor, Paints.BackgroundLine);
                    using (var background = new BitmapDrawable(Resources, bitmapLastStat))
                    {
                        viewLastStat.Background = background;
                    }
                }
            }
        }

        private void CreateViewLearnedWords(LinearLayout viewLastStat)
        {
            using (var bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20),
                PixelConverter.DpToPX(65), Bitmap.Config.Argb4444))
            {
                using (var canvas = new Canvas(bitmapLastStat))
                {
                    var stat = new DrawStatistics(canvas);
                    stat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                    var numberLearned = ViewModel.DatabaseStats.Count(r => r.NumberLearn == 0);
                    stat.ProgressLine(numberLearned, ViewModel.DatabaseStats.Count - numberLearned,
                        StatisticsFragment.LightColor, StatisticsFragment.DarkColor, Paints.BackgroundLine);
                    using (var background = new BitmapDrawable(Resources, bitmapLastStat))
                    {
                        viewLastStat.Background = background;
                    }
                }
            }
        }

        private void CreateViewDegreeOfStudy(LinearLayout viewLastStat, LinearLayout viewPieChart)
        {
            using (var bitmapLastStat =
                Bitmap.CreateBitmap(viewLastStat.Width, viewLastStat.Height, Bitmap.Config.Argb4444))
            {
                using (var canvas = new Canvas(bitmapLastStat))
                {
                    var stat = new DrawStatistics(canvas);
                    stat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                    using (var background = new BitmapDrawable(Resources, bitmapLastStat))
                    {
                        viewLastStat.Background = background;
                    }

                    using (var bitmapPieChart =
                        Bitmap.CreateBitmap(viewPieChart.Width, viewPieChart.Height, Bitmap.Config.Argb4444))
                    {
                        using (var canvasChart = new Canvas(bitmapPieChart))
                        {
                            var statChart = new DrawStatistics(canvasChart);
                            var avgNumberLearnStat = API.Statistics.GetAverageNumberLearn(ViewModel.DatabaseStats);
                            statChart.DrawPieChart(avgNumberLearnStat, Settings.StandardNumberOfRepeats,
                                StatisticsFragment.LightColor, StatisticsFragment.DarkColor);
                            using (var background = new BitmapDrawable(Resources, bitmapPieChart))
                            {
                                viewPieChart.Background = background;
                            }
                        }
                    }
                }
            }

            viewPieChart.ViewTreeObserver.RemoveOnPreDrawListener(this);
        }

        private void CreateViewPercentage(LinearLayout viewLastStat)
        {
            using (var bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20) -
                (int)(0.61f * Resources.DisplayMetrics.WidthPixels),
                PixelConverter.DpToPX(265), Bitmap.Config.Argb4444))
            {
                using (var canvas = new Canvas(bitmapLastStat))
                {
                    var stat = new DrawStatistics(canvas);
                    stat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                    using (var background = new BitmapDrawable(Resources, bitmapLastStat))
                    {
                        viewLastStat.Background = background;
                    }
                }
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_tab_statistics_general, container, false);
            var viewAnswersRatio = view.FindViewById<LinearLayout>(Resource.Id.answers_ratio);
            CreateViewAnswersRatio(viewAnswersRatio);
            var viewAkwardWord = view.FindViewById<LinearLayout>(Resource.Id.view_akward_word);
            CreateViewAkwardWord(viewAkwardWord);
            var viewLearnedWords = view.FindViewById<LinearLayout>(Resource.Id.view_learned_words);
            CreateViewLearnedWords(viewLearnedWords);

            ViewDegreeOfStudy = view.FindViewById<LinearLayout>(Resource.Id.view_degree_of_study);
            ViewPieChart = view.FindViewById<LinearLayout>(Resource.Id.view_degree_of_study_pie_chart);
            ViewPieChart.ViewTreeObserver.AddOnPreDrawListener(this);
            var viewPercentage = view.FindViewById<LinearLayout>(Resource.Id.view_percentage_of_correct_answers);
            CreateViewPercentage(viewPercentage);
            return view;
        }
    }
}