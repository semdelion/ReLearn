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
        bool Type_databse {get; }
        Color Color_Diagram_1 { get; }
        Color Color_Diagram_2 { get; }
        readonly Color background_color = new Color(Color.Argb(150, 16, 19, 38));
        readonly Paint paint_border = new Paint { StrokeWidth = 4, Color = Color.Argb(250, 215, 248, 254) };
        readonly Paint paint_text = new Paint { TextSize = 25, StrokeWidth = 4, Color = Color.Rgb(215, 248, 254) };

        public Graph_General_Statistics(Context context, Color color_diagram_1, Color color_diagram_2, bool type_database) : base(context)
        {
            Color_Diagram_1 = color_diagram_1;
            Color_Diagram_2 = color_diagram_2;
            Type_databse = type_database;
        }

        //void Diagram_Circle(List<Database_Statistics> Database_Stat, float left, float right, float bottom, float top)
        //{
        //    float step_width = (right - left) / 10f,
        //          step_height = (bottom - top) / 20f,
        //          padding = 3f; // between columns
        //    int i = 0, n_count = Database_Stat.Count - 10;
        //    foreach (var s in Database_Stat)
        //    {
        //        if (i >= n_count)
        //        {
        //            Shader shader = new LinearGradient(left + 2f, bottom - step_height * 20, left + step_width - 2f, bottom, Color_Diagram_1, Color_Diagram_2, TileMode.Clamp);
        //            Paint paint = new Paint();
        //            paint.SetShader(shader);
        //            The_canvas.DrawRoundRect(new RectF(left + padding, bottom - (step_height * s.True), left + step_width - padding, bottom), 0, 0, paint);

        //            left = left + step_width;
        //        }
        //        ++i;
        //    }
        //}
        //void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        //{
        //    SKImageInfo info = args.Info;
        //    SKSurface surface = args.Surface;
        //    SKCanvas canvas = surface.Canvas;

        //    canvas.Clear();

        //    int totalValues = 0;

        //    foreach (ChartData item in chartData)
        //    {
        //        totalValues += item.Value;
        //    }

        //    SKPoint center = new SKPoint(info.Width / 2, info.Height / 2);
        //    float explodeOffset = 50;
        //    float radius = Math.Min(info.Width / 2, info.Height / 2) - 2 * explodeOffset;
        //    SKRect rect = new SKRect(center.X - radius, center.Y - radius,
        //                             center.X + radius, center.Y + radius);

        //    float startAngle = 0;

        //    foreach (ChartData item in chartData)
        //    {
        //        float sweepAngle = 360f * item.Value / totalValues;

        //        using (SKPath path = new SKPath())
        //        using (SKPaint fillPaint = new SKPaint())
        //        using (SKPaint outlinePaint = new SKPaint())
        //        {
        //            path.MoveTo(center);
        //            path.ArcTo(rect, startAngle, sweepAngle, false);
        //            path.Close();

        //            fillPaint.Style = SKPaintStyle.Fill;
        //            fillPaint.Color = item.Color;

        //            outlinePaint.Style = SKPaintStyle.Stroke;
        //            outlinePaint.StrokeWidth = 5;
        //            outlinePaint.Color = SKColors.Black;

        //            // Calculate "explode" transform
        //            float angle = startAngle + 0.5f * sweepAngle;
        //            float x = explodeOffset * (float)Math.Cos(Math.PI * angle / 180);
        //            float y = explodeOffset * (float)Math.Sin(Math.PI * angle / 180);

        //            canvas.Save();
        //            canvas.Translate(x, y);

        //            // Fill and stroke the path
        //            canvas.DrawPath(path, fillPaint);
        //            canvas.DrawPath(path, outlinePaint);
        //            canvas.Restore();
        //        }

        //        startAngle += sweepAngle;
        //    }
        //}




        int Average_percent_true(List<Database_Statistics> Database_Stat)
        {
            float sum = 0;
            foreach(var s in Database_Stat)
                sum+=s.True;
            return (int )((sum/Database_Stat.Count) * 5);
        }

        protected override void OnDraw(Canvas canvas)
        {
            The_canvas = canvas;
            base.OnDraw(The_canvas);

            var database = DataBase.Connect(Database_Name.Statistics);
            var Database_Stat = database.Query<Database_Statistics>("SELECT * FROM " + DataBase.Table_Name + "_Statistics");// количество строк в БД
            int average_percent_true = Average_percent_true(Database_Stat);
            paint_text.TextSize = 2.5f * (The_canvas.Height + The_canvas.Width) / 200;
            paint_border.SetStyle(Paint.Style.Stroke);

            //float h_rate = The_canvas.Height / 100f,
            //      w_rate = The_canvas.Width / 100f,
            //      left = 10f * w_rate, right = 90f * w_rate,
            //      top = 7f * h_rate, bottom = 70f * h_rate;

            //Ordinate(left - 10f, bottom, bottom - top, paint_text);
            //Abscissa(left, bottom + 10f, right - left, paint_text, Database_Stat.Count);
            //Graph_layout(left, bottom, top, right);
            //Diagram(Database_Stat, left, right, bottom, top);


          

            //new FrameStatistics(left, bottom + 10f * h_rate, right, 95f * h_rate, background_color);
            //LastStat.DrawBorder(The_canvas, paint_border);

            //LastStat.DrawBorder(The_canvas, paint_border);
            //LastStat.Progress_Line(The_canvas, Database_Stat[Database_Stat.Count - 1].True, Database_Stat[Database_Stat.Count - 1].False, Color_Diagram_1, Color_Diagram_2);
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
                During_last_month = new FrameStatistics(The_canvas.Height , 10 * The_canvas.Height / 100f, The_canvas.Width - 10f * The_canvas.Height / 100f, 30 * The_canvas.Height / 100f, background_color);
            }
            During_all_time.DrawBorder(The_canvas, paint_border);
            During_last_month.DrawBorder(The_canvas, paint_border);
            During_last_month.Progress_Line(The_canvas, average_percent_true, 100 - average_percent_true, Color_Diagram_1, Color_Diagram_2);


            Shader shader1 = new LinearGradient(0, 0, 50 ,50, Color_Diagram_2, Color_Diagram_1, TileMode.Clamp);
            Paint paint1 = new Paint { Color = Color_Diagram_1, StrokeWidth = 50 };
            paint1.SetStyle(Paint.Style.Stroke);
            paint1.SetShader(shader1);
           
            Paint paint2 = new Paint { Color = Color.Rgb(29, 43, 59), StrokeWidth = 50};
            paint2.SetStyle(Paint.Style.Stroke);


            The_canvas.DrawArc(new RectF(During_all_time.Left + 50, During_all_time.Top + 50, During_all_time.Right - 50, During_all_time.Bottom - 50),   0, 360, false, paint2);
            The_canvas.DrawArc(new RectF(During_all_time.Left + 50, During_all_time.Top + 50, During_all_time.Right - 50, During_all_time.Bottom - 50),   0,355, false, paint1);
           

        }
    }
}