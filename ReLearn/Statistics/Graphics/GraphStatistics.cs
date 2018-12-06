using Android.Content;
using Android.Graphics;
using Android.Views;
using System.Collections.Generic;
using static Android.Graphics.Shader;

namespace ReLearn
{
    class GraphStatistics : View
    {
        Canvas TheCanvas;
        readonly Color Color_Diagram_1;
        readonly Color Color_Diagram_2;
        readonly string TabelName;
        readonly Color background_color = new Color(Color.Argb(150, 16, 19, 38));
        readonly Paint paint_border = new Paint { StrokeWidth = 4, Color = Colors.White, AntiAlias = true };
        readonly Paint paint_text = new Paint { TextSize = 25, StrokeWidth = 4, Color = Colors.White, AntiAlias = true };

        public GraphStatistics(Context context, Color color_diagram_1, Color color_diagram_2, string Table_Name) : base(context)
        {
            Color_Diagram_1 = color_diagram_1;
            Color_Diagram_2 = color_diagram_2;
            TabelName = Table_Name;
        }

        void Abscissa(float left, float bottom, float width, Paint paint, int amount)
        {
            TheCanvas.DrawLine(left, bottom, left + width, bottom, paint);

            float step = width / 10f;
            for (int i = 0; i <= 10; i++)
                TheCanvas.DrawLine(left + step * i, bottom, left + step * i, bottom + 1.5f * (TheCanvas.Width + TheCanvas.Height) / 200, paint);

            int count = amount >= 10 ? 10 : amount,
                stat  = amount >= 10 ? amount - 10 : 0;
            for (int i = 1; i <= count; i++)
                TheCanvas.DrawText((stat + i).ToString(), left + step * i, bottom + 4 * (TheCanvas.Width + TheCanvas.Height) / 200, paint);
        }

        void Ordinate(float left, float bottom, float height, Paint paint)
        {
            TheCanvas.DrawLine(left, bottom, left, bottom - height, paint);

            float step = height / 10f;
            for (int i = 0; i <= 10; i++)
                TheCanvas.DrawLine(left, bottom - step * i, left - 1.5f * (TheCanvas.Width + TheCanvas.Height) / 200, bottom - step * i, paint);

            for (int i = 0; i <= 10; i++)
                TheCanvas.DrawText((i*10).ToString()+"%", left - 8f * (TheCanvas.Width + TheCanvas.Height) / 200,
                    bottom - step * (i) + 1.5f * (TheCanvas.Width + TheCanvas.Height) / 200, paint);
        }

        void Graph_layout(float left, float bottom, float top, float right)
        {
            float height = (bottom - top) / 20f;
            Paint paint = new Paint { TextSize = 25, Color = Color.Argb(40, 215, 248, 254), StrokeWidth = 1, AntiAlias = true };
            for (int j = 2; j <= 20; j += 2)
                TheCanvas.DrawLine(left, bottom - height * j, right, bottom - height * j, paint);
        }

        void Diagram(List<DatabaseStatistics> Database_Stat, float left, float right, float bottom, float top)
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
                    TheCanvas.DrawRoundRect(new RectF(left + padding, bottom - ((step_height/(s.True+s.False)) * s.True), left + step_width - padding, bottom), 0, 0, paint);
                    left = left + step_width;
                }
                ++i;
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            TheCanvas = canvas;
            var Database_Stat = DBStatistics.GetData(TabelName);
            base.OnDraw(TheCanvas);

            paint_text.TextSize = 2.5f * (TheCanvas.Height + TheCanvas.Width) / 200;
            paint_border.SetStyle(Paint.Style.Stroke);

            float h_rate = TheCanvas.Height / 100f,
                  w_rate = TheCanvas.Width / 100f,
                  left = 14f * w_rate, right = 93f * w_rate,
                  top = 7f * h_rate, bottom = 70f * h_rate;

            Ordinate(left - 10f, bottom, bottom - top, paint_text);
            Abscissa(left, bottom + 10f, right - left, paint_text, Database_Stat.Count);
            Graph_layout(left, bottom, top, right);
            Diagram(Database_Stat, left, right, bottom, top);

            FrameStatistics LastStat = new FrameStatistics(7f * w_rate, bottom + 10f * h_rate, right, 95f * h_rate, background_color);
            LastStat.DrawBorder(TheCanvas, paint_border);
            int? True = null, False = null;
            if (Database_Stat.Count != 0)
            {
                True = Database_Stat[Database_Stat.Count - 1].True;
                False = Database_Stat[Database_Stat.Count - 1].False;
            }
            LastStat.ProgressLine(TheCanvas, True??0, False??1, Color_Diagram_1, Color_Diagram_2);
            string TextLastStat = $"{Context.GetString(Resource.String.Correct)}: {True??0}, {Context.GetString(Resource.String.Incorrect_Up)}: {False??0}";
            if (TheCanvas.Height > TheCanvas.Width)
            {
                LastStat.DrawText(TheCanvas, 25 * LastStat.Height / 100, Context.GetString(Resource.String.Last_testing),
                    LastStat.Left + 7 * LastStat.Width / 100, LastStat.Top + LastStat.Height / 14);
                LastStat.DrawText(TheCanvas, 15 * LastStat.Height / 100, TextLastStat, LastStat.Left + 7 * LastStat.Width / 100,
                    LastStat.Top + 25 * LastStat.Height / 100 + LastStat.Height / 7);
            }
            else
                LastStat.DrawText(TheCanvas, 40 * LastStat.Height / 100, $"{Context.GetString(Resource.String.Last_testing)}.   {TextLastStat}",
                    LastStat.Left + 7 * LastStat.Width / 100, LastStat.Top + LastStat.Height / 14);
        }
    }
}