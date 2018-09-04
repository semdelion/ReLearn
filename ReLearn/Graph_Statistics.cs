using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Graphics;
using static Android.Graphics.Shader;

namespace ReLearn
{
    class FrameStatistics
    {
        public float Left { get; }
        public float Right { get; }
        public float Top { get; }
        public float Bottom { get; }
        public float Width { get => Right - Left; }
        public float Height { get => Bottom - Top; }
        Paint P;

        public FrameStatistics(float left, float top, float right, float bottom, Color color)
        {
            Left = left; Bottom = bottom; Right = right; Top = top;
            P = new Paint { Color = color };
        }

        public FrameStatistics(float left, float top, float right, float bottom, Color color1, Color color2)
        {
            Left = left; Bottom = bottom; Right = right; Top = top;
            P = new Paint();
            Shader shader = new LinearGradient(0, Top, 0, Bottom, color1, color2, TileMode.Clamp);
            P.SetShader(shader);
        }

        public void Draw(Canvas canvas) => canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, P);

        public void DrawBorder(Canvas canvas, Paint paint)
        {
            canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, P);
            canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, paint);
        }

        public void Progress_Line(Canvas canvas, float height, float True, float False)
        {
            float padding_left_right = 30f;
            float padding_bottom = 30f;

            Paint PRed = new Paint { Color = Color.Rgb(209, 50, 50) };
            Paint PGreen = new Paint { Color = Color.Rgb(84, 204, 73) };

            float step_width = (Width - padding_left_right * 2) / (True + False);

            canvas.DrawRect(new RectF(Left + padding_left_right, Bottom - padding_bottom - height, Left + padding_left_right + step_width * True, Bottom - padding_bottom), PGreen);
            canvas.DrawRect(new RectF(Left + padding_left_right + step_width * True, Bottom - padding_bottom - height, Right - padding_left_right, Bottom - padding_bottom), PRed);

            Paint paint_border = new Paint { StrokeWidth = 2, Color = Color.Argb(250, 215, 248, 254) };
            paint_border.SetStyle(Paint.Style.Stroke);
            canvas.DrawRect(new RectF(Left + padding_left_right, Bottom - padding_bottom - height, Right - padding_left_right, Bottom - padding_bottom), paint_border);
        }

        public void DrawText(Canvas canvas, float font_size, string text, float left, float top)
        {
            Paint paint = new Paint
            {
                TextSize = font_size,
                Color = Color.Rgb(215, 248, 254)
            };
            canvas.DrawText(Convert.ToString(text), left, top + font_size, paint);
        }
    }

    class Graph_Statistics : View
    {
        public Graph_Statistics(Context context) : base(context) { }
        Canvas The_canvas;
        readonly Color background_color = new Color(Color.Argb(150, 16, 19, 38));
        readonly Paint paint_border = new Paint { StrokeWidth = 4, Color = Color.Argb(250, 215, 248, 254) };
        readonly Paint paint_text = new Paint { TextSize = 25, StrokeWidth = 4, Color = Color.Rgb(215, 248, 254) };
        void Abscissa(float left, float bottom, float width, Paint paint, int amount)
        {
            The_canvas.DrawLine(left, bottom, left + width, bottom, paint);

            float step = width / 10f;
            for (int i = 0; i <= 10; i++)
                The_canvas.DrawLine(left + step * i, bottom, left + step * i, bottom + 10f, paint);

            int count = amount >= 10 ? 10 : amount;
            int stat = amount >= 10 ? amount - 10 : 0;
            for (int i = 1; i <= count; i++)
                The_canvas.DrawText((stat + i).ToString(), left + step * i, bottom + 35f, paint);
        }

        void Ordinate(float left, float bottom, float height, Paint paint)
        {
            The_canvas.DrawLine(left, bottom, left, bottom - height, paint);

            float step = height / 20f;
            for (int i = 0; i <= 20; i += 2)
                The_canvas.DrawLine(left, bottom - step * i, left - 10f, bottom - step * i, paint);

            for (int i = 0; i <= 20; i += 2)
                The_canvas.DrawText((i).ToString(), left - 45f, bottom - step * (i) + 10f, paint);
        }

        void Graph_layout(float left, float bottom, float top, float right)
        {
            float height = bottom - top;
            Paint paint = new Paint { TextSize = 25, Color = Color.Argb(40, 215, 248, 254), StrokeWidth = 1 };
            for (int j = 2; j <= 20; j += 2)
                The_canvas.DrawLine(left, bottom - height * (j), right, bottom - height * (j), paint);
        }

        void Diagram(List<Database_Statistics> Database_Stat, float left, float right, float bottom, float top)
        {
            float step_width = (right - left) / 10f,
                  step_height = (bottom - top) / 20f;
            int i = 0, n_count = Database_Stat.Count - 10, False, True;
            foreach (var s in Database_Stat)
            {
                if (i >= n_count)
                {
                    False = s.False; True = s.True;
                    Shader shader = new LinearGradient(left + 2f, bottom - (step_height * True), left + step_width - 2f, bottom, Color.Rgb(155 - 155 / 20 * True, 100 + 155 / 20 * True, 0), Color.Rgb(255, 0, 0), TileMode.Clamp);
                    Paint paint = new Paint();
                    paint.SetShader(shader);
                    The_canvas.DrawRoundRect(new RectF(left + 2, bottom - (step_height * True), left + step_width - 2f, bottom), 6, 6, paint);
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

            paint_border.SetStyle(Paint.Style.Stroke);

            float left = 70f, right = The_canvas.Width - 70f,
                  top = 50f, bottom = The_canvas.Height - 300f;


            Ordinate(left - 10f, bottom, bottom - top, paint_text);
            Abscissa(left, bottom + 10f, right - left, paint_text, Database_Stat.Count);
            Graph_layout(left, bottom, top, right);
            Diagram(Database_Stat, left, right, bottom, top);

            FrameStatistics LastStat = new FrameStatistics(left, bottom + 70, right, canvas.Height - 50, background_color);
            LastStat.DrawBorder(The_canvas, paint_border);
            if (Database_Stat.Count != 0)
            {
                LastStat.Progress_Line(The_canvas, 20, Database_Stat[Database_Stat.Count - 1].True, Database_Stat[Database_Stat.Count - 1].False);
                LastStat.DrawText(The_canvas, 45, "Last testing", LastStat.Left + 30f, LastStat.Top + 15f);
                LastStat.DrawText(The_canvas, 30, "Correct: " + Convert.ToString(Database_Stat[Database_Stat.Count - 1].True) +
                                  ", Incorrect: " + Convert.ToString(Database_Stat[Database_Stat.Count - 1].False), LastStat.Left + 30f, LastStat.Top + 70f);
            }
        }
    }
}