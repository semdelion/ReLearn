using Android.Content;
using Android.Graphics;
using Android.Views;
using System;
using System.Collections.Generic;
using static Android.Graphics.Shader;

namespace ReLearn
{
    class Graph_Statistics : View
    {
        Canvas The_canvas;
        Color Color_Diagram_1;
        Color Color_Diagram_2;
        readonly Color background_color = new Color(Color.Argb(150, 16, 19, 38));
        readonly Paint paint_border = new Paint { StrokeWidth = 4, Color = Color.Argb(250, 215, 248, 254), AntiAlias = true };
        readonly Paint paint_text = new Paint { TextSize = 25, StrokeWidth = 4, Color = Color.Rgb(215, 248, 254), AntiAlias = true };

        public Graph_Statistics(Context context, Color color_diagram_1, Color color_diagram_2) : base(context)
        {
            Color_Diagram_1 = color_diagram_1;
            Color_Diagram_2 = color_diagram_2;
        }

        void Abscissa(float left, float bottom, float width, Paint paint, int amount)
        {
            The_canvas.DrawLine(left, bottom, left + width, bottom, paint);

            float step = width / 10f;
            for (int i = 0; i <= 10; i++)
                The_canvas.DrawLine(left + step * i, bottom, left + step * i, bottom + 1.5f * (The_canvas.Width + The_canvas.Height) / 200, paint);

            int count = amount >= 10 ? 10 : amount,
                stat = amount >= 10 ? amount - 10 : 0;
            for (int i = 1; i <= count; i++)
                The_canvas.DrawText((stat + i).ToString(), left + step * i, bottom + 4 * (The_canvas.Width + The_canvas.Height) / 200, paint);
        }

        void Ordinate(float left, float bottom, float height, Paint paint)
        {
            The_canvas.DrawLine(left, bottom, left, bottom - height, paint);

            float step = height / 10f;
            for (int i = 0; i <= 10; i++)
                The_canvas.DrawLine(left, bottom - step * i, left - 1.5f * (The_canvas.Width + The_canvas.Height) / 200, bottom - step * i, paint);

            for (int i = 0; i <= 10; i++)
                The_canvas.DrawText((i*10).ToString()+"%", left - 8f * (The_canvas.Width + The_canvas.Height) / 200,
                    bottom - step * (i) + 1.5f * (The_canvas.Width + The_canvas.Height) / 200, paint);
        }

        void Graph_layout(float left, float bottom, float top, float right)
        {
            float height = (bottom - top) / 20f;
            Paint paint = new Paint { TextSize = 25, Color = Color.Argb(40, 215, 248, 254), StrokeWidth = 1, AntiAlias = true };
            for (int j = 2; j <= 20; j += 2)
                The_canvas.DrawLine(left, bottom - height * j, right, bottom - height * j, paint);
        }

        void Diagram(List<Database_Statistics> Database_Stat, float left, float right, float bottom, float top)
        {
            float step_width = (right - left) / 10f,
                  step_height = (bottom - top),
                  padding = 3f; // between columns
            int i = 0, n_count = Database_Stat.Count - 10;
            foreach (var s in Database_Stat)
            {
                if (i >= n_count)
                {
                    Shader shader = new LinearGradient(left + 2f, bottom - step_height, left + step_width - 2f, bottom, Color_Diagram_1, Color_Diagram_2, TileMode.Clamp);
                    Paint paint = new Paint { AntiAlias = true };
                    paint.SetShader(shader);
                    The_canvas.DrawRoundRect(new RectF(left + padding, bottom - ((step_height/(s.True+s.False)) * s.True), left + step_width - padding, bottom), 0, 0, paint);
                    left = left + step_width;
                }
                ++i;
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            The_canvas = canvas;
            var database = DataBase.Connect(Database_Name.Statistics);
            var Database_Stat = database.Query<Database_Statistics>("SELECT * FROM " + DataBase.Table_Name + "_Statistics");// количество строк в БД
            base.OnDraw(The_canvas);

            paint_text.TextSize = 2.5f * (The_canvas.Height + The_canvas.Width) / 200;
            paint_border.SetStyle(Paint.Style.Stroke);

            float h_rate = The_canvas.Height / 100f,
                  w_rate = The_canvas.Width / 100f,
                  left = 14f * w_rate, right = 93f * w_rate,
                  top = 7f * h_rate, bottom = 70f * h_rate;

            Ordinate(left - 10f, bottom, bottom - top, paint_text);
            Abscissa(left, bottom + 10f, right - left, paint_text, Database_Stat.Count);
            Graph_layout(left, bottom, top, right);
            Diagram(Database_Stat, left, right, bottom, top);

            FrameStatistics LastStat = new FrameStatistics(7f * w_rate, bottom + 10f * h_rate, right, 95f * h_rate, background_color);
            LastStat.DrawBorder(The_canvas, paint_border);
            if (Database_Stat.Count != 0)
            {
                LastStat.ProgressLine(The_canvas, Database_Stat[Database_Stat.Count - 1].True, Database_Stat[Database_Stat.Count - 1].False, Color_Diagram_1, Color_Diagram_2);
                if (The_canvas.Height > The_canvas.Width)
                {
                    LastStat.DrawText(The_canvas, 25 * LastStat.Height / 100, Additional_functions.GetResourceString("Last_testing", this.Resources), LastStat.Left + 7 * LastStat.Width / 100, LastStat.Top + LastStat.Height / 14);
                    LastStat.DrawText(The_canvas, 15 * LastStat.Height / 100, Additional_functions.GetResourceString("Correct", this.Resources) + ": " + Convert.ToString(Database_Stat[Database_Stat.Count - 1].True) +
                                      ", "+ Additional_functions.GetResourceString("Incorrect_Up", this.Resources)+ ": " + Convert.ToString(Database_Stat[Database_Stat.Count - 1].False), LastStat.Left + 7 * LastStat.Width / 100, LastStat.Top + 25 * LastStat.Height / 100 + LastStat.Height / 7);
                }
                else 
                    LastStat.DrawText(The_canvas, 40 * LastStat.Height / 100, Additional_functions.GetResourceString("Last_testing", this.Resources) + "." + "   " + Additional_functions.GetResourceString("Correct", this.Resources) + ": " + Convert.ToString(Database_Stat[Database_Stat.Count - 1].True) +
                        ", " + Additional_functions.GetResourceString("Incorrect_Up", this.Resources) + ": " + Convert.ToString(Database_Stat[Database_Stat.Count - 1].False), LastStat.Left + 7 * LastStat.Width / 100, LastStat.Top + LastStat.Height / 14);
            }
        }
    }
}