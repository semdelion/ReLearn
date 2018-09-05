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
            canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, P);
            canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, paint);
        }

        public void Progress_Line(Canvas canvas, float True, float False, Color Color_Diagram_1, Color Color_Diagram_2)
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