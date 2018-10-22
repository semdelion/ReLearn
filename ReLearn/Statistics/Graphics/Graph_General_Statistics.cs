using Android.Content;
using Android.Graphics;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReLearn
{
    class Graph_General_Statistics : View
    {
        Canvas The_canvas;
        TableNames TableName;
        Color Start { get; }
        Color End { get; }
        string Object_name { get;}
        List<Database_for_stats> Stats_database { get; }
        readonly Color background_color = new Color( Color.Argb(150, 16, 19, 38) );
        readonly Paint paint_border = new Paint { StrokeWidth = 4, Color = Color.Argb(250, 215, 248, 254), AntiAlias = true };
        readonly Paint paint_text = new Paint { TextSize = 25, StrokeWidth = 4, Color = Color.Rgb(215, 248, 254), AntiAlias = true };
        float text_size_up;
        float text_size_low;

        public Graph_General_Statistics(Context context, Color start, Color end, List<Database_for_stats> stats_database, string object_name , string Table_Name ) : base(context)
        {
            Stats_database = stats_database;
            Start = start;
            End = end;
            Object_name = object_name;
            TableName = (TableNames)Enum.Parse(typeof(TableNames), Table_Name);
        }

        float Average_True_Today(SQLite.SQLiteConnection database)
        {
            var Database_Stat = database.Query<Database_Statistics>("SELECT * FROM " + TableName.ToString() + "_Statistics" + " WHERE DateOfTesting >= date('now')");// количество строк в БД
            return Average_percent_true(Database_Stat);
        }

        float Average_True_Month(SQLite.SQLiteConnection database)
        {
            var Database_Stat = database.Query<Database_Statistics>("SELECT * FROM " + TableName.ToString() + "_Statistics" + " WHERE  STRFTIME ( '%Y%m', DateOfTesting) = STRFTIME ( '%Y%m', 'now')");// количество строк в БД
            return Average_percent_true(Database_Stat);
        }

        float Average_percent_true(List<Database_Statistics> Database_Stat) =>
            Database_Stat.Count == 0 ? 0 : (Database_Stat.Sum(r => r.True) * (100 / (Database_Stat.Sum(r => r.True) + Database_Stat.Sum(r => r.False))));

        void DegreeOfStudy_FRAME(FrameStatistics Degree)
        {
            Degree.DrawBorder(The_canvas, paint_border);
            float avg_numberLearn_stat = (float)Stats_database.Sum(
                r => r.NumberLearn > Magic_constants.StandardNumberOfRepeats ? 
                Magic_constants.StandardNumberOfRepeats : r.NumberLearn) / (float)Stats_database.Count;

            Degree.DrawPieChart(The_canvas, avg_numberLearn_stat, Magic_constants.StandardNumberOfRepeats, Start, End,
            new PointF(Degree.Left + Degree.Width / 2f, Degree.Bottom - 27.5f * The_canvas.Width / 100f), Degree.Width / 2.5f);
            Degree.DrawText(The_canvas, text_size_up, Additional_functions.GetResourceString("Degree_Of_Study", this.Resources), 
                Degree.Left + Degree.Width / 12, Degree.Top + Degree.Height / 20);
        }

        void CorrectAnswers_FRAME(FrameStatistics CA,float Width, float Height, float avg_true_today, float avg_true_month, float avg_true)
        {
            CA.DrawBorder(The_canvas, paint_border);
            float one_third     = Height - 12.5f * Height / 100;

            CA.DrawText(The_canvas, text_size_up, 
                Additional_functions.GetResourceString("Correct_answers", this.Resources), CA.Left + 7f * CA.Width / 100, CA.Top + Height / 20);
            CA.DrawText(The_canvas, text_size_low, 
                Additional_functions.GetResourceString("Today", this.Resources), CA.Left + 7f * CA.Width / 100, CA.Top + 12.5f *  Height / 100);
            CA.DrawText(The_canvas, 1.9f * text_size_up, 
                Math.Round(avg_true_today, 1) + "%", CA.Left + 7f * CA.Width / 100, CA.Top + 17.5f * Height / 100);

            if (avg_true_today != 0)
                CA.DrawText(The_canvas, 6f * Width / 100,
                (avg_true_today - avg_true >= 0 ? "+" : "") + Math.Round(avg_true_today - avg_true, 1) + "%",
                CA.Left + 2f * CA.Width / 3, CA.Top + 20f * Height / 100, avg_true_today - avg_true >= 0 ? Color.Green : Color.Red);

            CA.DrawText(The_canvas, text_size_low, 
                Additional_functions.GetResourceString("Month", this.Resources), CA.Left + 7f * CA.Width / 100, CA.Top + one_third / 3 + 12.5f * Height / 100);
            CA.DrawText(The_canvas, 1.9f * text_size_up, 
                Math.Round(avg_true_month, 1) + "%", CA.Left + 7f * CA.Width / 100, CA.Top + one_third / 3 + 17.5f * Height / 100);

            if (avg_true_month != 0)
                CA.DrawText(The_canvas, 6f * Width / 100,
                (avg_true_month - avg_true >= 0 ? "+" : "") + Math.Round(avg_true_month - avg_true, 1) + "%",
                CA.Left + 2f * CA.Width / 3, CA.Top + one_third / 3 + 20f * Height / 100, avg_true_month - avg_true >= 0 ? Color.Green : Color.Red);

            CA.DrawText(The_canvas, text_size_low, 
                Additional_functions.GetResourceString("Average", this.Resources), CA.Left + 7f * CA.Width / 100, CA.Top + 2f * one_third / 3 + 12.5f * Height / 100);
            CA.DrawText(The_canvas, 1.9f * text_size_up, 
                Math.Round(avg_true, 1)       + "%", CA.Left + 7f * CA.Width / 100, CA.Top + 2f * one_third / 3 + 17.5f * Height / 100);
        }

        void LearnedWords_FRAME(FrameStatistics LW)
        {
            int numberLearned = Stats_database.Count(r => r.NumberLearn == 0);
            LW.DrawBorder(The_canvas, paint_border);
            LW.ProgressLine(The_canvas, numberLearned, Stats_database.Count - numberLearned, Start, End);
            LW.DrawText    (The_canvas, text_size_up,  
                Additional_functions.GetResourceString("Number_Learned_" + Object_name, this.Resources), 
                LW.Left + 7f * LW.Width / 100, LW.Top + 7f * LW.Height / 100);
            LW.DrawText    (The_canvas, text_size_low, 
                numberLearned + " " + Additional_functions.GetResourceString("Of", this.Resources) + " " + Stats_database.Count, 
                LW.Left + 7f * LW.Width / 100, LW.Top + 38f * LW.Height / 100);
        }

        void InconvenientWords_FRAME(FrameStatistics IW)
        {
            int numberInconvenient = Stats_database.Count(r => r.NumberLearn == Magic_constants.MaxNumberOfRepeats);
            IW.DrawBorder(The_canvas, paint_border);
            IW.ProgressLine(The_canvas, numberInconvenient, Stats_database.Count - numberInconvenient, Start, End);
            IW.DrawText(The_canvas, text_size_up, 
                Additional_functions.GetResourceString("Number_Inconvenient_" + Object_name, this.Resources), 
                IW.Left + 7f * IW.Width / 100, IW.Top + 7f * IW.Height / 100);
            IW.DrawText(The_canvas, text_size_low, 
                numberInconvenient + " " + Additional_functions.GetResourceString("Of", this.Resources) + " " + Stats_database.Count, 
                IW.Left + 7f * IW.Width / 100, IW.Top + 38f * IW.Height / 100);
        }

        void TotalNumbers_FRAME(FrameStatistics TotalNumbers, List<Database_Statistics> Database_Stat)
        {
            int numberTrue  = Database_Stat.Sum(r => r.True),
                numberFalse = Database_Stat.Sum(r => r.False);
            TotalNumbers.DrawBorder(The_canvas, paint_border);

            TotalNumbers.ProgressLine(The_canvas, numberTrue, numberFalse, Start, End);

            TotalNumbers.DrawText(The_canvas, text_size_up, 
                Additional_functions.GetResourceString("Number_Correct_Answers", this.Resources), 
                TotalNumbers.Left + 7f * TotalNumbers.Width / 100, TotalNumbers.Top + 7f * TotalNumbers.Height / 100);

            TotalNumbers.DrawText(The_canvas, text_size_low, 
                Additional_functions.GetResourceString("Correct", this.Resources)         + " " + numberTrue  + ", " + 
                Additional_functions.GetResourceString("Incorrect", this.Resources)       + " " + numberFalse + ", " + 
                Additional_functions.GetResourceString("Number_Of_Tests", this.Resources) + " " + Database_Stat.Count, 
                TotalNumbers.Left + 7f * TotalNumbers.Width / 100, TotalNumbers.Top + 38f * TotalNumbers.Height / 100);
        }

        protected override void OnDraw(Canvas canvas)
        {
            The_canvas = canvas;
            base.OnDraw(The_canvas);
            paint_border.SetStyle(Paint.Style.Stroke);
            paint_text.TextSize = 2.5f * (The_canvas.Height + The_canvas.Width) / 200f;
            var database = DataBase.Connect(Database_Name.Statistics);
            var Database_Stat = database.Query<Database_Statistics>("SELECT * FROM " + TableName.ToString() + "_Statistics");// количество строк в БД

            float height = (The_canvas.Height - 81f * The_canvas.Width / 100f) / 3f;

            FrameStatistics DegreeOfStudy =       new FrameStatistics(5f  * The_canvas.Width / 100f, 5f  * The_canvas.Width / 100f, 
                                                                      60f * The_canvas.Width / 100f, 70f * The_canvas.Width / 100f,  background_color);
            FrameStatistics CorrectAnswers =      new FrameStatistics(62f * The_canvas.Width / 100f, 5f  * The_canvas.Width / 100f, 
                                                                      95f * The_canvas.Width / 100f, 70f * The_canvas.Width / 100f,  background_color);
            FrameStatistics LearnedWords =        new FrameStatistics(5f  * The_canvas.Width / 100f, 2f  * The_canvas.Width / 100f + CorrectAnswers.Bottom, 
                                                                      95f * The_canvas.Width / 100f, 2f  * The_canvas.Width / 100f + CorrectAnswers.Bottom    + height, background_color);
            FrameStatistics InconvenientWords =   new FrameStatistics(5f  * The_canvas.Width / 100f, 2f  * The_canvas.Width / 100f + LearnedWords.Bottom, 
                                                                      95f * The_canvas.Width / 100f, 2f  * The_canvas.Width / 100f + LearnedWords.Bottom      + height, background_color);
            FrameStatistics TotalNumbers =        new FrameStatistics(5f  * The_canvas.Width / 100f, 2f  * The_canvas.Width / 100f + InconvenientWords.Bottom, 
                                                                      95f * The_canvas.Width / 100f, 2f  * The_canvas.Width / 100f + InconvenientWords.Bottom + height, background_color);
            text_size_up  = 7f * DegreeOfStudy.Width / 100;
            text_size_low = 5f * DegreeOfStudy.Width / 100;

            float avg_true_today = Average_True_Today(database),
                  avg_true_month = Average_True_Month(database),
                  avg_true = Average_percent_true(Database_Stat);
                 
                
            DegreeOfStudy_FRAME (DegreeOfStudy);

            CorrectAnswers_FRAME(CorrectAnswers, DegreeOfStudy.Width, DegreeOfStudy.Height, avg_true_today, avg_true_month, avg_true);

            LearnedWords_FRAME  (LearnedWords);

            InconvenientWords_FRAME(InconvenientWords);

            TotalNumbers_FRAME(TotalNumbers, Database_Stat);
        }
    }
}