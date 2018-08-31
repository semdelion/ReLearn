using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SQLite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using static Android.Graphics.Shader;

namespace ReLearn
{
    class Graph_Statistics : View
    {
        public Graph_Statistics(Context context) : base(context) { }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            Paint text_paint = new Paint {TextSize = 25}; 
            // используется для написания текста и рисования линии
            //left    float: Левая сторона прямоугольника, который нужно нарисовать
            //top float: Верхняя сторона прямоугольника, который нужно нарисовать
            //right   float: Правая сторона прямоугольника, который нужно нарисовать
            //bottom  float: Нижняя сторона прямоугольника, который нужно нарисовать
            float height = (canvas.Height - 450f) / 20, width = (canvas.Width - 100f) / 20, left = 50f, right = left + width, bottom = canvas.Height - 300f;
            text_paint.Color = Color.Black;//цвет для линии
            text_paint.StrokeWidth = 4; // толщина  линии
            canvas.DrawLine(25, bottom + 3f, canvas.Width, bottom + 3f, text_paint); //абсцисса
            canvas.DrawLine(canvas.Width - 15, bottom + 18f, canvas.Width, bottom + 3f, text_paint);
            canvas.DrawLine(canvas.Width - 15, bottom - 12f, canvas.Width, bottom + 3f, text_paint);
            canvas.DrawLine(left - 5f, bottom + 30f, left - 5f, 100, text_paint);    //ордината
            canvas.DrawLine(left - 20f, 120, left - 5f, 100, text_paint);
            canvas.DrawLine(left + 10f, 120, left - 5f, 100, text_paint);
            text_paint.Color = Color.White;
            var database = DataBase.Connect(Database_Name.Statistics);
            var count_Database_Stat = database.Query<Database_Statistics>("SELECT * FROM " + DataBase.Table_Name + "_Statistics" );// количество строк в БД
            int i = 0, n_count = count_Database_Stat.Count - 10, False = 0, True = 0; // n_count - используется для вывода только последних 10 строк БД
            right -= 2;
            foreach (var s in count_Database_Stat)
            {
                if (i >= n_count)
                {                  
                    False = s.False; True = s.True;
                    Shader shader = new LinearGradient(left, bottom - (height * True), right, bottom, Color.Rgb(255 - 255 / 20 * True, 255 / 20 * True, 0), Color.Rgb(255, 0, 0), TileMode.Clamp);
                    Paint paint = new Paint();
                    paint.SetShader(shader);
                    canvas.DrawRect(new RectF(left, bottom - (height * True), right, bottom), paint);
                    canvas.DrawText((i + 1).ToString(), right, bottom + 30, text_paint); //вывод номера теста
                    left = left + width + 2;
                    right = left + width - 2;                  
                }
                ++i;
            }
            i = 0;
            for (int j = 2; i < 10; i++, j += 2)//разметка оси ординат
                canvas.DrawText((j).ToString(), 10, bottom - (height * j), text_paint);
            text_paint.TextSize = 40;
            canvas.DrawText(Convert.ToString("Correct = " + True), 50, bottom + 100, text_paint);
            canvas.DrawText(Convert.ToString("Incorrect = " + False), 50, bottom + 150, text_paint);
        }
    }
}