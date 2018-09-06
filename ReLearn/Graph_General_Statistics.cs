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
        List<Database_for_stats> Stats_databse { get; }
        Color Color_Diagram_1 { get; }
        Color Color_Diagram_2 { get; }
        readonly Color background_color = new Color(Color.Argb(150, 16, 19, 38));
        readonly Paint paint_border = new Paint { StrokeWidth = 4, Color = Color.Argb(250, 215, 248, 254) };
        readonly Paint paint_text = new Paint { TextSize = 25, StrokeWidth = 4, Color = Color.Rgb(215, 248, 254) };

        public Graph_General_Statistics(Context context, Color color_diagram_1, Color color_diagram_2, List<Database_for_stats> stats_databse) : base(context)
        {
            Stats_databse = stats_databse;
            Color_Diagram_1 = color_diagram_1;
            Color_Diagram_2 = color_diagram_2;
        }
        float Average_percent_true(List<Database_Statistics> Database_Stat)
        {
            float sum = 0;
            foreach(var s in Database_Stat)
                sum+=s.True;
            return sum/Database_Stat.Count;
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
            FrameStatistics During_last_month;
            //FrameStatistics During_last_day;
            //FrameStatistics Degree_of_study;
            //FrameStatistics Average_degree;

           




            if (The_canvas.Height > The_canvas.Width)
            {
                During_all_time   = new FrameStatistics(10f * The_canvas.Width / 100f, 10f * The_canvas.Width / 100f, 90f * The_canvas.Width / 100f, 90f * The_canvas.Width / 100f, background_color);
                During_last_month = new FrameStatistics(10f * The_canvas.Width / 100f, The_canvas.Width, 90f * The_canvas.Width / 100f, 120f * The_canvas.Width / 100f, background_color);
            }
            else
            {
                During_all_time   = new FrameStatistics(10f * The_canvas.Height / 100f, 10 * The_canvas.Height / 100f, 90 * The_canvas.Height / 100f, 90 * The_canvas.Height / 100f, background_color);
                During_last_month = new FrameStatistics(The_canvas.Height , 10 * The_canvas.Height / 100f, The_canvas.Width - 10f * The_canvas.Height / 100f, 30f * The_canvas.Height / 100f, background_color);
            }
            During_all_time.DrawBorder(The_canvas, paint_border);
            During_last_month.DrawBorder(The_canvas, paint_border);
            During_last_month.ProgressLine(The_canvas, average_percent_true, 20 - average_percent_true, Color_Diagram_1, Color_Diagram_2);

            During_all_time.DrawPieChart(The_canvas, average_percent_true, 20, Color_Diagram_1, Color_Diagram_2);
           

        }
    }
}