using Android.Content;
using Android.Graphics;
using Android.Views;
using System;
using System.Collections.Generic;

namespace ReLearn
{
    class Graph_General_Statistics : View
    {
        Canvas The_canvas;
        TableNames TableName;
        Color Color_Diagram_1 { get; }
        Color Color_Diagram_2 { get; }
        string Object_name { get;}
        List<Database_for_stats> Stats_database { get; }
        readonly Color background_color = new Color(Color.Argb(150, 16, 19, 38));
        readonly Paint paint_border = new Paint { StrokeWidth = 4, Color = Color.Argb(250, 215, 248, 254), AntiAlias = true };
        readonly Paint paint_text = new Paint { TextSize = 25, StrokeWidth = 4, Color = Color.Rgb(215, 248, 254), AntiAlias = true };

        public Graph_General_Statistics(Context context, Color color_diagram_1, Color color_diagram_2, List<Database_for_stats> stats_database,string object_name , string Table_Name ) : base(context)
        {
            Stats_database = stats_database;
            Color_Diagram_1 = color_diagram_1;
            Color_Diagram_2 = color_diagram_2;
            Object_name = object_name;
            TableName = (TableNames)Enum.Parse(typeof(TableNames), Table_Name);
        }

        float Average(List<Database_for_stats> list_stat)
        {
            float sum = 0;
            for (int i = 0; i < list_stat.Count; i++)
                sum += list_stat[i].NumberLearn;
            return sum / list_stat.Count;
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

        float Average_percent_true(List<Database_Statistics> Database_Stat)
        {
            float sum_True = 0;
            float sum_False = 0;
            foreach (var s in Database_Stat)
            {
                sum_True += s.True;
                sum_False += s.False;
            }
            return Database_Stat.Count == 0 ? 0 : (sum_True * (100 / (sum_True + sum_False)));
        }

        int Number_of_words_learned(List<Database_for_stats> list_stat)
        {
            int sum = 0;
            foreach (var s in list_stat)
                if (s.NumberLearn == 0)
                    sum++;
            return sum;
        }

        int Number_of_words_inconvenient(List<Database_for_stats> list_stat)
        {
            int sum = 0;
            foreach (var s in list_stat)
                if (s.NumberLearn == Magic_constants.MaxNumberOfRepeats)
                    sum++;
            return sum;
        }

        int Number_of_correct_answers(List<Database_Statistics> Database_Stat)
        {
            int sum = 0;
            foreach (var s in Database_Stat)
                sum += s.True;
            return sum;
        }
        int Number_of_incorrect_answers(List<Database_Statistics> Database_Stat)
        {
            int sum = 0;
            foreach (var s in Database_Stat)
                sum += s.False;
            return sum;
        }

        protected override void OnDraw(Canvas canvas)
        {
            The_canvas = canvas;
            base.OnDraw(The_canvas);
            paint_border.SetStyle(Paint.Style.Stroke);
            paint_text.TextSize = 2.5f * (The_canvas.Height + The_canvas.Width) / 200;
            var database = DataBase.Connect(Database_Name.Statistics);
            var Database_Stat = database.Query<Database_Statistics>("SELECT * FROM " + TableName.ToString() + "_Statistics");// количество строк в БД

            float height = (The_canvas.Height - 81f * The_canvas.Width / 100f) / 3;

            FrameStatistics Vocabulary_learning = new FrameStatistics(5f * The_canvas.Width / 100f, 5f * The_canvas.Width / 100f, 60f * The_canvas.Width / 100f, 70f * The_canvas.Width / 100f, background_color);
            FrameStatistics Correct_answers = new FrameStatistics(62f * The_canvas.Width / 100f, 5f * The_canvas.Width / 100f, 95f * The_canvas.Width / 100f, 70f * The_canvas.Width / 100f, background_color);
            FrameStatistics Learned_words = new FrameStatistics(5f * The_canvas.Width / 100f, 2f * The_canvas.Width / 100f + Correct_answers.Bottom, 95f * The_canvas.Width / 100f, 2f * The_canvas.Width / 100f + Correct_answers.Bottom + height, background_color);
            FrameStatistics Inconvenient_words = new FrameStatistics(5f * The_canvas.Width / 100f, 2f * The_canvas.Width / 100f + Learned_words.Bottom, 95f * The_canvas.Width / 100f, 2f * The_canvas.Width / 100f + Learned_words.Bottom + height, background_color);
            FrameStatistics Total_numbers = new FrameStatistics(5f * The_canvas.Width / 100f, 2f * The_canvas.Width / 100f + Inconvenient_words.Bottom, 95f * The_canvas.Width / 100f, 2f * The_canvas.Width / 100f + Inconvenient_words.Bottom + height, background_color);

            float rate = (The_canvas.Height + The_canvas.Width) / 200f,
                  avg_numberLearn_stat = Average(Stats_database),
                  avg_true_today = Average_True_Today(database),
                  avg_true_month = Average_True_Month(database),
                  avg_true = Average_percent_true(Database_Stat),
                  text_size_up = 7f * Vocabulary_learning.Width / 100,
                  text_size_low = 5f * Vocabulary_learning.Width / 100;

            int numberLearned = Number_of_words_learned(Stats_database),
                numberInconvenient = Number_of_words_inconvenient(Stats_database),
                numberTrue = Number_of_correct_answers(Database_Stat),
                numberFalse = Number_of_incorrect_answers(Database_Stat);


            Vocabulary_learning.DrawBorder(The_canvas, paint_border);
            Correct_answers.DrawBorder(The_canvas, paint_border);
            Learned_words.DrawBorder(The_canvas, paint_border);
            Inconvenient_words.DrawBorder(The_canvas, paint_border);
            Total_numbers.DrawBorder(The_canvas, paint_border);

            Vocabulary_learning.DrawPieChart(The_canvas, avg_numberLearn_stat, Magic_constants.MaxNumberOfRepeats, Color_Diagram_1, Color_Diagram_2,
                new PointF(Vocabulary_learning.Left + Vocabulary_learning.Width / 2f, Vocabulary_learning.Bottom - 27.5f * The_canvas.Width / 100f), Vocabulary_learning.Width / 2.5f);
            Vocabulary_learning.DrawText(The_canvas, text_size_up, Additional_functions.GetResourceString("Degree_Of_Study", this.Resources), Vocabulary_learning.Left + Vocabulary_learning.Width / 12, Vocabulary_learning.Top + Vocabulary_learning.Height / 20);

            float one_third = Vocabulary_learning.Height - 12.5f * Vocabulary_learning.Height / 100;
            Correct_answers.DrawText(The_canvas, text_size_up, Additional_functions.GetResourceString("Correct_answers", this.Resources), Correct_answers.Left + 7f * Correct_answers.Width / 100, Correct_answers.Top + Vocabulary_learning.Height / 20);

            Correct_answers.DrawText(The_canvas, text_size_low, Additional_functions.GetResourceString("Today", this.Resources), Correct_answers.Left + 7f * Correct_answers.Width / 100, Correct_answers.Top + 12.5f * Vocabulary_learning.Height / 100);
            Correct_answers.DrawText(The_canvas, 1.9f * text_size_up, Math.Round(avg_true_today, 1) + "%", Correct_answers.Left + 7f * Correct_answers.Width / 100, Correct_answers.Top + 17.5f * Vocabulary_learning.Height / 100);
            if (avg_true_today != 0)
                Correct_answers.DrawText(The_canvas, 6f * Vocabulary_learning.Width / 100,
                (avg_true_today - avg_true >= 0 ? "+" : "") + Math.Round(avg_true_today - avg_true, 1) + "%",
                Correct_answers.Left + 2f * Correct_answers.Width / 3, Correct_answers.Top + 20f * Vocabulary_learning.Height / 100, avg_true_today - avg_true >= 0 ? Color.Green : Color.Red);

            Correct_answers.DrawText(The_canvas, text_size_low, Additional_functions.GetResourceString("Month", this.Resources), Correct_answers.Left + 7f * Correct_answers.Width / 100, Correct_answers.Top + one_third / 3 + 12.5f * Vocabulary_learning.Height / 100);
            Correct_answers.DrawText(The_canvas, 1.9f * text_size_up, Math.Round(avg_true_month, 1) + "%", Correct_answers.Left + 7f * Correct_answers.Width / 100, Correct_answers.Top + one_third / 3 + 17.5f * Vocabulary_learning.Height / 100);
            if (avg_true_month != 0)
                Correct_answers.DrawText(The_canvas, 6f * Vocabulary_learning.Width / 100,
                (avg_true_month - avg_true >= 0 ? "+" : "") + Math.Round(avg_true_month - avg_true, 1) + "%",
                Correct_answers.Left + 2f * Correct_answers.Width / 3, Correct_answers.Top + one_third / 3 + 20f * Vocabulary_learning.Height / 100, avg_true_month - avg_true >= 0 ? Color.Green : Color.Red);

            Correct_answers.DrawText(The_canvas, text_size_low, Additional_functions.GetResourceString("Average", this.Resources), Correct_answers.Left + 7f * Correct_answers.Width / 100, Correct_answers.Top + 2f * one_third / 3 + 12.5f * Vocabulary_learning.Height / 100);
            Correct_answers.DrawText(The_canvas, 1.9f * text_size_up, Math.Round(avg_true, 1) + "%", Correct_answers.Left + 7f * Correct_answers.Width / 100, Correct_answers.Top + 2f * one_third / 3 + 17.5f * Vocabulary_learning.Height / 100);


            Learned_words.ProgressLine(The_canvas, numberLearned, Stats_database.Count - numberLearned, Color_Diagram_1, Color_Diagram_2);
            Learned_words.DrawText(The_canvas, text_size_up, Additional_functions.GetResourceString("Number_Learned_" + Object_name, this.Resources), Learned_words.Left + 7f * Learned_words.Width / 100, Learned_words.Top + 7f * Learned_words.Height / 100);
            Learned_words.DrawText(The_canvas, text_size_low, numberLearned + " " + Additional_functions.GetResourceString("Of", this.Resources) + " " + Stats_database.Count, Learned_words.Left + 7f * Learned_words.Width / 100, Learned_words.Top + 38f * Learned_words.Height / 100);


            Inconvenient_words.ProgressLine(The_canvas, numberInconvenient, Stats_database.Count - numberInconvenient, Color_Diagram_1, Color_Diagram_2);
            Inconvenient_words.DrawText(The_canvas, text_size_up, Additional_functions.GetResourceString("Number_Inconvenient_"+Object_name, this.Resources), Inconvenient_words.Left + 7f * Inconvenient_words.Width / 100, Inconvenient_words.Top + 7f * Inconvenient_words.Height / 100);
            Inconvenient_words.DrawText(The_canvas, text_size_low, numberInconvenient + " " + Additional_functions.GetResourceString("Of", this.Resources) + " " + Stats_database.Count, Inconvenient_words.Left + 7f * Inconvenient_words.Width / 100, Inconvenient_words.Top + 38f * Inconvenient_words.Height / 100);

            Total_numbers.ProgressLine(The_canvas, numberTrue, numberFalse, Color_Diagram_1, Color_Diagram_2);
            Total_numbers.DrawText(The_canvas, text_size_up, Additional_functions.GetResourceString("Number_Correct_Answers", this.Resources), Total_numbers.Left + 7f * Total_numbers.Width / 100, Total_numbers.Top + 7f * Total_numbers.Height / 100);
            Total_numbers.DrawText(The_canvas, text_size_low, Additional_functions.GetResourceString("Correct", this.Resources) + " "
                + numberTrue + ", " + Additional_functions.GetResourceString("Incorrect", this.Resources) + " " + numberFalse + ", " 
                + Additional_functions.GetResourceString("Number_Of_Tests", this.Resources) + " " 
                + Database_Stat.Count, Total_numbers.Left + 7f * Total_numbers.Width / 100, Total_numbers.Top + 38f * Total_numbers.Height / 100);
        }
    }
}