using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Statistics;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Statistics;
using ReLearn.Droid.Views.Fragments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReLearn.Droid.Views.Statistics
{
    public class TabGeneralFragment : MvxFragment<GeneralStatisticsViewModel>, ViewTreeObserver.IOnPreDrawListener
    {
        public LinearLayout viewPieChart;
        public LinearLayout viewDegreeOfStudy;

        private void CreateViewAnswersRatio(LinearLayout viewLastStat)
        {
            using (Bitmap bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20),
                PixelConverter.DpToPX(65), Bitmap.Config.Argb8888))
            {
                Canvas canvas = new Canvas(bitmapLastStat);
                var Stat = new DrawStatistics(canvas);
                Stat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                int numberTrue = ViewModel.Database.Sum(r => r.True),
                numberFalse = ViewModel.Database.Sum(r => r.False);
                Stat.ProgressLine(numberTrue, (numberFalse + numberTrue) == 0 ? 1 : numberFalse, 
                    StatisticsFragment.LightColor, StatisticsFragment.DarkColor, Paints.BackgroundLine);
                viewLastStat.Background = new BitmapDrawable(Resources, bitmapLastStat);
            }
        }

        private void CreateViewAkwardWord(LinearLayout viewLastStat)
        {
            using (Bitmap bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20),
                PixelConverter.DpToPX(65), Bitmap.Config.Argb8888))
            {
                Canvas canvas= new Canvas(bitmapLastStat);
                var Stat = new DrawStatistics(canvas);
                Stat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                int numberInconvenient = ViewModel.DatabaseStats.Count(r => r.NumberLearn == API.Settings.MaxNumberOfRepeats);
                Stat.ProgressLine(numberInconvenient, ViewModel.DatabaseStats.Count - numberInconvenient,
                    StatisticsFragment.LightColor, StatisticsFragment.DarkColor, Paints.BackgroundLine);
                viewLastStat.Background = new BitmapDrawable(Resources, bitmapLastStat);
            }
        }

        private void CreateViewLearnedWords(LinearLayout viewLastStat)
        {
            using (Bitmap bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20),
                PixelConverter.DpToPX(65), Bitmap.Config.Argb8888))
            {
                Canvas canvas = new Canvas(bitmapLastStat);
                var Stat = new DrawStatistics(canvas);
                Stat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                int numberLearned = ViewModel.DatabaseStats.Count(r => r.NumberLearn == 0);
                Stat.ProgressLine(numberLearned, ViewModel.DatabaseStats.Count - numberLearned,
                   StatisticsFragment.LightColor, StatisticsFragment.DarkColor, Paints.BackgroundLine);
                viewLastStat.Background = new BitmapDrawable(Resources, bitmapLastStat);
            }
        }

        private void CreateViewDegreeOfStudy(LinearLayout viewLastStat, LinearLayout viewPieChart)
        {
            using (Bitmap bitmapLastStat = Bitmap.CreateBitmap(viewLastStat.Width, viewLastStat.Height, Bitmap.Config.Argb8888))
            {
                Canvas canvas = new Canvas(bitmapLastStat);
                var Stat = new DrawStatistics(canvas);
                Stat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                viewLastStat.Background = new BitmapDrawable(Resources, bitmapLastStat);

                using (Bitmap bitmapPieChart = Bitmap.CreateBitmap(viewPieChart.Width, viewPieChart.Height, Bitmap.Config.Argb8888))
                {
                    Canvas canvasChart = new Canvas(bitmapPieChart);
                    var StatChart = new DrawStatistics(canvasChart);
                    float avgNumberLearnStat = API.Statistics.GetAverageNumberLearn(ViewModel.DatabaseStats);
                    StatChart.DrawPieChartWithText(avgNumberLearnStat, API.Settings.StandardNumberOfRepeats, StatisticsFragment.LightColor, StatisticsFragment.DarkColor);
                    viewPieChart.Background = new BitmapDrawable(Resources, bitmapPieChart);
                }
            }
            viewPieChart.ViewTreeObserver.RemoveOnPreDrawListener(this);
        }

        private void CreateViewPercentage(LinearLayout viewLastStat)
        {
            using (Bitmap bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20) - (int)(0.61f * Resources.DisplayMetrics.WidthPixels),
                PixelConverter.DpToPX(265), Bitmap.Config.Argb8888))
            {
                Canvas canvas = new Canvas(bitmapLastStat);
                var Stat = new DrawStatistics(canvas);
                Stat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                viewLastStat.Background = new BitmapDrawable(Resources, bitmapLastStat);
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

            viewDegreeOfStudy = view.FindViewById<LinearLayout>(Resource.Id.view_degree_of_study);
            viewPieChart = view.FindViewById<LinearLayout>(Resource.Id.view_degree_of_study_pie_chart);
            viewPieChart.ViewTreeObserver.AddOnPreDrawListener(this);
            var viewPercentage = view.FindViewById<LinearLayout>(Resource.Id.view_percentage_of_correct_answers);
            CreateViewPercentage(viewPercentage);
            return view;
        }

        public bool OnPreDraw()
        {
            CreateViewDegreeOfStudy(viewDegreeOfStudy, viewPieChart);
            return true;
        }
    }
}

  

   
