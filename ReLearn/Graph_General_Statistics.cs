using Android.Content;
using Android.Graphics;
using Android.Views;
using System;
using System.Collections.Generic;
using static Android.Graphics.Shader;
namespace ReLearn
{
    class Graph_General_Statistics : View
    {
        Canvas The_canvas;
        List<Database_for_stats> Stats_database { get; }
        Color Color_Diagram_1 { get; }
        Color Color_Diagram_2 { get; }
        readonly Color background_color = new Color(Color.Argb(150, 16, 19, 38));
        readonly Paint paint_border = new Paint { StrokeWidth = 4, Color = Color.Argb(250, 215, 248, 254) };
        readonly Paint paint_text = new Paint { TextSize = 25, StrokeWidth = 4, Color = Color.Rgb(215, 248, 254) };

        public Graph_General_Statistics(Context context, Color color_diagram_1, Color color_diagram_2, List<Database_for_stats> stats_databse) : base(context)
        {
            Stats_database  = stats_databse;
            Color_Diagram_1 = color_diagram_1;
            Color_Diagram_2 = color_diagram_2;
        }

        float Average_percent_true(List<Database_Statistics> Database_Stat)
        {
            float sum = 0;
            foreach(var s in Database_Stat)
                sum += s.True;
            return (sum/Database_Stat.Count)*(Magic_constants.repeat_count/100);
        }

        int Number_of_words_learned(List<Database_for_stats> list_stat)
        {
            int sum = 0;
            foreach (var s in list_stat)
                if(s.NumberLearn == 0)
                    sum++;
            return sum;
        }

        public float Average(List<Database_for_stats> list_stat)
        {
            float sum = 0;
            for (int i = 0; i < list_stat.Count; i++)
                sum += list_stat[i].NumberLearn;
            return sum / list_stat.Count;         
        }      

        protected override void OnDraw(Canvas canvas)
        {
            The_canvas = canvas;
            base.OnDraw(The_canvas);

            var database = DataBase.Connect(Database_Name.Statistics);
            var Database_Stat = database.Query<Database_Statistics>("SELECT * FROM " + DataBase.Table_Name + "_Statistics");// количество строк в БД
            float average_percent_true = Average_percent_true(Database_Stat);
            paint_text.TextSize = 2.5f * (The_canvas.Height + The_canvas.Width) / 200;
            paint_border.SetStyle(Paint.Style.Stroke);

            float rate = (The_canvas.Height + The_canvas.Width) / 200f;

            FrameStatistics During_all_time;
            FrameStatistics During_1;
            FrameStatistics During_2;
            FrameStatistics During_3;
            FrameStatistics During_4;

            float avg_numberLearn_stat = Average(Stats_database);
            int numberLearned = Number_of_words_learned(Stats_database);
            float height = (The_canvas.Height - 81f * The_canvas.Width/100f)/3;

            During_all_time   = new FrameStatistics(5f  * The_canvas.Width / 100f, 5f * The_canvas.Width / 100f, 60f * The_canvas.Width / 100f, 70f  * The_canvas.Width / 100f, background_color);
            During_1 = new FrameStatistics(62f * The_canvas.Width / 100f, 5f * The_canvas.Width / 100f, 95f * The_canvas.Width / 100f, 70f * The_canvas.Width / 100f, background_color);
            During_2 = new FrameStatistics(5f * The_canvas.Width / 100f, 2f * The_canvas.Width / 100f + During_1.Bottom, 95f * The_canvas.Width / 100f, 2f * The_canvas.Width / 100f + During_1.Bottom + height, background_color);
            During_3 = new FrameStatistics(5f * The_canvas.Width / 100f, 2f * The_canvas.Width / 100f + During_2.Bottom, 95f * The_canvas.Width / 100f, 2f * The_canvas.Width / 100f + During_2.Bottom + height, background_color);
            During_4 = new FrameStatistics(5f * The_canvas.Width / 100f, 2f * The_canvas.Width / 100f + During_3.Bottom, 95f * The_canvas.Width / 100f, 2f * The_canvas.Width / 100f + During_3.Bottom + height, background_color);

            float text_size_up = 7f * During_all_time.Width / 100, text_size_low = 5f * During_all_time.Width / 100;


            During_all_time.DrawBorder(The_canvas, paint_border);
            During_1.DrawBorder(The_canvas, paint_border);
            During_2.DrawBorder(The_canvas, paint_border);
            During_3.DrawBorder(The_canvas, paint_border);
            During_4.DrawBorder(The_canvas, paint_border);

            During_all_time.DrawPieChart(The_canvas, avg_numberLearn_stat, Magic_constants.maxLearn, Color_Diagram_1, Color_Diagram_2,
                new PointF(During_all_time.Left + During_all_time.Width / 2f, During_all_time.Bottom - 27.5f * The_canvas.Width / 100f), During_all_time.Width / 2.5f);
            During_all_time.DrawText(The_canvas, text_size_up, "Degree of study", During_all_time.Left + During_all_time.Width / 12, During_all_time.Top + During_all_time.Height / 20);

            float one_third = During_all_time.Height - 12.5f * During_all_time.Height / 100;
            During_1.DrawText(The_canvas, text_size_up, "Correct answers", During_1.Left + 7f * During_1.Width / 100, During_1.Top + During_all_time.Height / 20);
            During_1.DrawText(The_canvas, text_size_low, "Today", During_1.Left + 7f * During_1.Width / 100, During_1.Top + 12.5f * During_all_time.Height / 100);
            During_1.DrawText(The_canvas, 2 * text_size_up, "50%", During_1.Left + 7f * During_1.Width / 100, During_1.Top + 17.5f * During_all_time.Height / 100);

            During_1.DrawText(The_canvas, text_size_low, "Month", During_1.Left + 7f * During_1.Width / 100, During_1.Top + one_third / 3 + 12.5f * During_all_time.Height / 100);
            During_1.DrawText(The_canvas, 2 * text_size_up, "45%", During_1.Left + 7f * During_1.Width / 100, During_1.Top + one_third / 3 + 17.5f * During_all_time.Height / 100);

            During_1.DrawText(The_canvas, text_size_low, "Average", During_1.Left + 7f * During_1.Width / 100, During_1.Top + 2f * one_third / 3 + 12.5f * During_all_time.Height / 100);
            During_1.DrawText(The_canvas, 2 * text_size_up, Math.Round(avg_numberLearn_stat, 1) + "%", During_1.Left + 7f * During_1.Width / 100, During_1.Top +  2f * one_third / 3 + 17.5f * During_all_time.Height / 100);


            During_2.ProgressLine(The_canvas, numberLearned, Stats_database.Count - numberLearned, Color_Diagram_1, Color_Diagram_2);
            During_2.DrawText(The_canvas, text_size_up,  "Number of words learned",                     During_2.Left + 7f * During_2.Width / 100, During_2.Top + 7f  * During_2.Height / 100);
            During_2.DrawText(The_canvas, text_size_low, numberLearned + " of " + Stats_database.Count, During_2.Left + 7f * During_2.Width / 100, During_2.Top + 38f * During_2.Height / 100);


            During_3.ProgressLine(The_canvas, average_percent_true, Magic_constants.repeat_count - average_percent_true, Color_Diagram_1, Color_Diagram_2);

            During_4.ProgressLine(The_canvas, avg_numberLearn_stat, Magic_constants.maxLearn - avg_numberLearn_stat,     Color_Diagram_1, Color_Diagram_2);

            
           

        }
    }
}