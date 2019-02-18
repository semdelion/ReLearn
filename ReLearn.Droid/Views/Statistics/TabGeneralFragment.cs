using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Statistics;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Statistics;
using ReLearn.Droid.Views.Fragments;
using System.Collections.Generic;
using System.Linq;

namespace ReLearn.Droid.Views.Statistics
{
    public class TabGeneralFragment : MvxFragment<GeneralStatisticsViewModel>
    {
        private List<DatabaseStatistics> Database { get; set; } = null;
        private List<DBStatistics> DatabaseStats { get; set; } = null;

        private void CreateViewAnswersRatio(LinearLayout viewLastStat)
        {
            using (Bitmap bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20),
                PixelConverter.DpToPX(65), Bitmap.Config.Argb8888))
            {
                Canvas canvas = new Canvas(bitmapLastStat);
                var Stat = new DrawStatistics(canvas);
                Stat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                int numberTrue = Database.Sum(r => r.True),
                numberFalse = Database.Sum(r => r.False);
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
                int numberInconvenient = DatabaseStats.Count(r => r.NumberLearn == API.Settings.MaxNumberOfRepeats);
                Stat.ProgressLine(numberInconvenient, DatabaseStats.Count - numberInconvenient,
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
                int numberLearned = DatabaseStats.Count(r => r.NumberLearn == 0);
                Stat.ProgressLine(numberLearned, DatabaseStats.Count - numberLearned,
                   StatisticsFragment.LightColor, StatisticsFragment.DarkColor, Paints.BackgroundLine);
                viewLastStat.Background = new BitmapDrawable(Resources, bitmapLastStat);
            }
        }

        private void CreateViewDegreeOfStudy(LinearLayout viewLastStat, LinearLayout viewPieChart)
        {
            using (Bitmap bitmapLastStat = Bitmap.CreateBitmap(
                Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20) - (int)(0.39f * Resources.DisplayMetrics.WidthPixels),
                PixelConverter.DpToPX(265), Bitmap.Config.Argb8888))
            {
                Canvas canvas = new Canvas(bitmapLastStat);
                var Stat = new DrawStatistics(canvas);
                Stat.DrawBackground(6, 6, Paints.Background, Paints.Border, Paints.Gradient);
                viewLastStat.Background = new BitmapDrawable(Resources, bitmapLastStat);

                using (Bitmap bitmapPieChart = Bitmap.CreateBitmap(
                    Resources.DisplayMetrics.WidthPixels - PixelConverter.DpToPX(20) - (int)(0.39f * Resources.DisplayMetrics.WidthPixels),
                    PixelConverter.DpToPX(240), Bitmap.Config.Argb8888))
                {
                    Canvas canvasChart = new Canvas(bitmapPieChart);
                    var StatChart = new DrawStatistics(canvasChart);
                    float avgNumberLearnStat = API.Statistics.GetAverageNumberLearn(DatabaseStats);
                    StatChart.DrawPieChartWithText(avgNumberLearnStat, API.Settings.StandardNumberOfRepeats, StatisticsFragment.LightColor, StatisticsFragment.DarkColor);
                    viewPieChart.Background = new BitmapDrawable(Resources, bitmapPieChart);
                }
            }
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
            Database = DBStatistics.GetData($"{DataBase.TableName}");
            bool isContain = DBImages.DatabaseIsContain(DataBase.TableName.ToString());
            DatabaseStats = isContain ? DBStatistics.GetImages(DataBase.TableName.ToString()) :
                                      DBStatistics.GetWords(DataBase.TableName.ToString());

            var view = inflater.Inflate(Resource.Layout.TESTGeneralSTATISTICS, container, false);
            var viewAnswersRatio = view.FindViewById<LinearLayout>(Resource.Id.answers_ratio);
            CreateViewAnswersRatio(viewAnswersRatio);
            var viewAkwardWord = view.FindViewById<LinearLayout>(Resource.Id.view_akward_word);
            CreateViewAkwardWord(viewAkwardWord);
            var viewLearnedWords = view.FindViewById<LinearLayout>(Resource.Id.view_learned_words);
            CreateViewLearnedWords(viewLearnedWords);

            var viewDegreeOfStudy = view.FindViewById<LinearLayout>(Resource.Id.view_degree_of_study);
            var viewPieChart = view.FindViewById<LinearLayout>(Resource.Id.view_degree_of_study_pie_chart);
            CreateViewDegreeOfStudy(viewDegreeOfStudy, viewPieChart);

            var viewPercentage = view.FindViewById<LinearLayout>(Resource.Id.view_percentage_of_correct_answers);
            CreateViewPercentage(viewPercentage);


            return view;
        }


    }
}

  

   
