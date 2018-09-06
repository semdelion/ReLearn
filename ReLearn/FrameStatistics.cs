using Android.Graphics;
using System;
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
            LinearGradient backlg = new LinearGradient((canvas.Width+ canvas.Height) / 200, 0, 0, (canvas.Width + canvas.Height) / 200, Color.Argb(40, 60, 90, 125), Color.Transparent , TileMode.Repeat);
            Paint paint1 = new Paint();
            paint1.SetShader(backlg);
            canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, paint1);
            canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, P);
            canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, paint);
        }

        public void ProgressLine(Canvas canvas, float True, float False, Color Color_Diagram_1, Color Color_Diagram_2)
        {
            float padding_left_right = 7f * Width / 100f;
            float padding_bottom = 14f * Height / 100f;
            float height = 15f * ((Bottom - Top) / 100f);
            float step_width = (Width - padding_left_right * 2) / (True + False);

            Shader shader = new LinearGradient(0, Bottom - padding_bottom - height, 0, Bottom - padding_bottom, Color_Diagram_1, Color_Diagram_2, TileMode.Clamp);
            Paint Pen_F = new Paint { Color = Color.Rgb(29, 43, 59) };
            Paint Pen_T = new Paint();
            Pen_T.SetShader(shader);

            canvas.DrawRect(new RectF(Left + padding_left_right, Bottom - padding_bottom - height, Left + padding_left_right + step_width * True, Bottom - padding_bottom), Pen_T);
            canvas.DrawRect(new RectF(Left + padding_left_right + step_width * True, Bottom - padding_bottom - height, Right - padding_left_right, Bottom - padding_bottom), Pen_F);

            Paint paint_border = new Paint { StrokeWidth = 2, Color = Color.Argb(250, 215, 248, 254) };
            paint_border.SetStyle(Paint.Style.Stroke);
            canvas.DrawRect(new RectF(Left + padding_left_right, Bottom - padding_bottom - height, Right - padding_left_right, Bottom - padding_bottom), paint_border);
        }

        public void DrawPieChart(Canvas canvas, float average, float sum,Color Color_Diagram_1, Color Color_Diagram_2)
        {
            float rate = (canvas.Width+canvas.Height)/ 200;
            Shader shader1 = new SweepGradient(Left + Width / 2, Top + Height / 2, Color_Diagram_2, Color_Diagram_1);
            Paint paint1 = new Paint { Color = Color_Diagram_1, StrokeWidth = 10f * Width / 100f };
            Paint paint2 = new Paint { Color = Color.Rgb(29, 43, 59), StrokeWidth = 10f * Width / 100f };
            paint1.SetStyle(Paint.Style.Stroke);
            paint1.SetShader(shader1);
            paint2.SetStyle(Paint.Style.Stroke);
            canvas.DrawArc(new RectF(Left + rate * 7f, Top + rate * 7f, Right - rate * 7f, Bottom - rate * 7f), 0, 360, false, paint2);
            canvas.Rotate(-90f, Left + Width / 2, Top + Height / 2);
            canvas.DrawArc(new RectF(Left + rate * 7f, Top + rate * 7f, Right - rate * 7f, Bottom - rate * 7f), 0, average * (360f / sum), false, paint1);
            canvas.Rotate(90f, Left + Width / 2, Top + Height / 2);
            DrawText(canvas, Width * 25 / 100f, Convert.ToString((int)(average * 100f/sum)) + "%", Left + 3f * Width / 10f, Top + 3.2f * Width / 10f);
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
}