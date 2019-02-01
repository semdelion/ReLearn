using Android.Content;
using Android.Graphics;
using Android.Views;
using ReLearn.API;
using ReLearn.API.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReLearn.Droid
{
    class GraphGeneralStatistics : View
    {
        Canvas TheCanvas = null;
        string TableName { get; set; }
        Color Start { get; }
        Color End { get; }
        string Object_name { get;}
        List<DBStatistics> Stats_database { get; }
        readonly Color background_color = Colors.FrameBackground;
        readonly Paint paint_border = new Paint { StrokeWidth = 4, Color = Colors.White, AntiAlias = true };
        readonly Paint paint_text = new Paint { TextSize = 25, StrokeWidth = 4, Color = Colors.White, AntiAlias = true };
        float font_up;
        float font_low;

        public GraphGeneralStatistics(Context context, Color start, Color end, List<DBStatistics> stats_database, string object_name , string Table_Name ) : base(context)
        {
            Stats_database = stats_database;
            Start = start;
            End = end;
            Object_name = object_name;
            TableName = Table_Name;
        }

        void DegreeOfStudy_FRAME(FrameStatistics Degree)
        {
            Degree.DrawBorder(TheCanvas, paint_border);
            float avg_numberLearn_stat = Statistics.GetAverageNumberLearn(Stats_database);
            Degree.DrawPieChartWithText(TheCanvas, avg_numberLearn_stat, Settings.StandardNumberOfRepeats, Start, End,
            new PointF(Degree.Left + Degree.Width / 2f, Degree.Bottom - 27.5f * TheCanvas.Width / 100f), Degree.Width / 2.5f);
            Degree.DrawText(TheCanvas, font_up, Context.GetString(Resource.String.Degree_Of_Study), 
                Degree.Left + Degree.Width / 12, Degree.Top + Degree.Height / 20);
        }

        void CorrectAnswers_FRAME(FrameStatistics CA,float Width, float Height, float avg_true_today, float avg_true_month, float avg_true)
        {
            CA.DrawBorder(TheCanvas, paint_border);
            float one_third = Height - 12.5f * Height / 100;

            CA.DrawText(TheCanvas, font_up,
                Context.GetString(Resource.String.Correct_answers), CA.Left + 7f * CA.Width / 100, CA.Top + Height / 20);
            CA.DrawText(TheCanvas, font_low,
                Context.GetString(Resource.String.Today), CA.Left + 7f * CA.Width / 100, CA.Top + 12.5f *  Height / 100);
            CA.DrawText(TheCanvas, 1.9f * font_up, 
                $"{Math.Round(avg_true_today, 1)}%", CA.Left + 7f * CA.Width / 100, CA.Top + 17.5f * Height / 100);

            if (avg_true_today != 0 && avg_true_today != avg_true_month)
                CA.DrawText(TheCanvas, 6f * Width / 100,
                $"{(avg_true_today - avg_true >= 0 ? "+" : "")}{Math.Round(avg_true_today - avg_true, 1)}%",
                CA.Left + 2f * CA.Width / 3, CA.Top + 20f * Height / 100, avg_true_today - avg_true >= 0 ? Color.Green : Color.Red);

            CA.DrawText(TheCanvas, font_low,
                 Context.GetString(Resource.String.Month), CA.Left + 7f * CA.Width / 100, CA.Top + one_third / 3 + 12.5f * Height / 100);
            CA.DrawText(TheCanvas, 1.9f * font_up,
                $"{Math.Round(avg_true_month, 1)}%", CA.Left + 7f * CA.Width / 100, CA.Top + one_third / 3 + 17.5f * Height / 100);

            if (avg_true_month != 0 && avg_true_month != avg_true)
                CA.DrawText(TheCanvas, 6f * Width / 100,
                $"{(avg_true_month - avg_true >= 0 ? "+" : "")}{Math.Round(avg_true_month - avg_true, 1)}%",
                CA.Left + 2f * CA.Width / 3, CA.Top + one_third / 3 + 20f * Height / 100, avg_true_month - avg_true >= 0 ? Color.Green : Color.Red);

            CA.DrawText(TheCanvas, font_low,                 
                Context.GetString(Resource.String.Average), CA.Left + 7f * CA.Width / 100, CA.Top + 2f * one_third / 3 + 12.5f * Height / 100);
            CA.DrawText(TheCanvas, 1.9f * font_up,
                $"{Math.Round(avg_true, 1)}%", CA.Left + 7f * CA.Width / 100, CA.Top + 2f * one_third / 3 + 17.5f * Height / 100);
        }

        void LearnedWords_FRAME(FrameStatistics LW)
        {
            int numberLearned = Stats_database.Count(r => r.NumberLearn == 0);
            LW.DrawBorder(TheCanvas, paint_border);
            LW.ProgressLine(TheCanvas, numberLearned, Stats_database.Count - numberLearned, Start, End);
            LW.DrawText(TheCanvas, font_up,
                GetString.GetResourceString($"Number_Learned_{Object_name}", this.Resources),
                LW.Left + 7f * LW.Width / 100, LW.Top + 7f * LW.Height / 100);
            LW.DrawText    (TheCanvas, font_low, 
                $"{numberLearned} {Context.GetString(Resource.String.Of)} {Stats_database.Count}", 
                LW.Left + 7f * LW.Width / 100, LW.Top + 38f * LW.Height / 100);
        }

        void InconvenientWords_FRAME(FrameStatistics IW)
        {
            int numberInconvenient = Stats_database.Count(r => r.NumberLearn == Settings.MaxNumberOfRepeats);
            IW.DrawBorder(TheCanvas, paint_border);
            IW.ProgressLine(TheCanvas, numberInconvenient, Stats_database.Count - numberInconvenient, Start, End);
            IW.DrawText(TheCanvas, font_up,
                GetString.GetResourceString($"Number_Inconvenient_{Object_name}", this.Resources),
                IW.Left + 7f * IW.Width / 100, IW.Top + 7f * IW.Height / 100);
            IW.DrawText(TheCanvas, font_low, 
                $"{numberInconvenient} {Context.GetString(Resource.String.Of)} {Stats_database.Count}", 
                IW.Left + 7f * IW.Width / 100, IW.Top + 38f * IW.Height / 100);
        }

        void TotalNumbers_FRAME(FrameStatistics TotalNumbers, List<DatabaseStatistics> Database_Stat)
        {
            int numberTrue  = Database_Stat.Sum(r => r.True),
                numberFalse = Database_Stat.Sum(r => r.False);
            TotalNumbers.DrawBorder(TheCanvas, paint_border);

            TotalNumbers.ProgressLine(TheCanvas, numberTrue, (numberFalse + numberTrue) == 0 ? 1 : numberFalse, Start, End);

            TotalNumbers.DrawText(TheCanvas, font_up,
                Context.GetString(Resource.String.Number_Correct_Answers), 
                TotalNumbers.Left + 7f * TotalNumbers.Width / 100, TotalNumbers.Top + 7f * TotalNumbers.Height / 100);

            TotalNumbers.DrawText(TheCanvas, font_low,
                $"{Context.GetString(Resource.String.Correct)} {numberTrue}, " +
                $"{Context.GetString(Resource.String.Incorrect)} {numberFalse}, " +
                $"{Context.GetString(Resource.String.Number_Of_Tests)} {Database_Stat.Count}", 
                TotalNumbers.Left + 7f * TotalNumbers.Width / 100, TotalNumbers.Top + 38f * TotalNumbers.Height / 100);
        }

        protected override void OnDraw(Canvas canvas)
        {
            TheCanvas = canvas;
            base.OnDraw(TheCanvas);
            paint_border.SetStyle(Paint.Style.Stroke);
            paint_text.TextSize = 2.5f * (TheCanvas.Height + TheCanvas.Width) / 200f;
            var Database_Stat = DBStatistics.GetData(TableName);
            float height = (TheCanvas.Height - 81f * TheCanvas.Width / 100f) / 3f, pct = TheCanvas.Width / 100f;

            FrameStatistics DegreeOfStudy = new FrameStatistics(5f * pct, 5f * pct, 60f * pct, 70f * pct, background_color);
            FrameStatistics CorrectAnswers = new FrameStatistics(62f * pct, 5f * pct, 95f * pct, 70f * pct, background_color);
            FrameStatistics LearnedWords = new FrameStatistics(5f * pct, 2f * pct + CorrectAnswers.Bottom, 95f * pct, 2f * pct + CorrectAnswers.Bottom + height, background_color);
            FrameStatistics InconvenientWords = new FrameStatistics(5f * pct, 2f * pct + LearnedWords.Bottom, 95f * pct, 2f * pct + LearnedWords.Bottom + height, background_color);
            FrameStatistics TotalNumbers = new FrameStatistics(5f * pct, 2f * pct + InconvenientWords.Bottom, 95f * pct, 2f * pct + InconvenientWords.Bottom + height, background_color);
            font_up = 7f * DegreeOfStudy.Width / 100;
            font_low = 5f * DegreeOfStudy.Width / 100;

            float avg_true_today = DBStatistics.AverageTrueToday(TableName),
                  avg_true_month = DBStatistics.AverageTrueMonth(TableName),
                  avg_true = DBStatistics.AveragePercentTrue(Database_Stat);

            DegreeOfStudy_FRAME(DegreeOfStudy);

            CorrectAnswers_FRAME(CorrectAnswers, DegreeOfStudy.Width, DegreeOfStudy.Height, avg_true_today, avg_true_month, avg_true);

            LearnedWords_FRAME(LearnedWords);

            InconvenientWords_FRAME(InconvenientWords);

            TotalNumbers_FRAME(TotalNumbers, Database_Stat);
        }
    }
}