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

namespace ReLearn
{
    class Graph_Statistics : View
    {
        public Graph_Statistics(Context context) : base(context) { }
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            Paint green = new Paint
            { // цвет для прямоугольников
                AntiAlias = true,
                Color = Color.Rgb(0x99, 0xcc, 0),
            };
            green.SetStyle(Paint.Style.FillAndStroke);

            Paint red = new Paint
            {
                AntiAlias = true,
                Color = Color.Rgb(0xff, 0x44, 0x44)
            };
            red.SetStyle(Paint.Style.FillAndStroke);

            string databasePath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), NameDatabase.Statistics);
            var database = new SQLiteConnection(databasePath); // подключение к БД
            database.CreateTable<Database_Stat>();

            Paint text_paint = new Paint(); // используется для написания текста и рисования линии
            text_paint.TextSize = 25;
            //left    float: Левая сторона прямоугольника, который нужно нарисовать
            //top float: Верхняя сторона прямоугольника, который нужно нарисовать
            //right   float: Правая сторона прямоугольника, который нужно нарисовать
            //bottom  float: Нижняя сторона прямоугольника, который нужно нарисовать
            float height = (canvas.Height - 450f) / 20, width = (canvas.Width - 100f) / 20, left = 50f, right = left + width, bottom = canvas.Height - 300f;
            text_paint.Color = Color.Black;//цвет для линии
            text_paint.StrokeWidth = 4; // толщина черной линии
            canvas.DrawLine(25, bottom + 3f, canvas.Width, bottom + 3f, text_paint); //абсцисса
            canvas.DrawLine(canvas.Width - 15, bottom + 18f, canvas.Width, bottom + 3f, text_paint);
            canvas.DrawLine(canvas.Width - 15, bottom - 12f, canvas.Width, bottom + 3f, text_paint);
            canvas.DrawLine(left - 5f, bottom + 30f, left - 5f, 100, text_paint);    //ордината
            canvas.DrawLine(left - 20f, 120, left - 5f, 100, text_paint);
            canvas.DrawLine(left + 10f, 120, left - 5f, 100, text_paint);
            text_paint.Color = Color.White;
            var count_Database_Stat = database.Query<Database_Stat>("SELECT * FROM Database_Stat");// количество строк в БД
            int i = 0, n_count = count_Database_Stat.Count - 10, False = 0, True = 0; // n_count - используется для вывода только последних 10 строк БД
            var table = database.Table<Database_Stat>();  //ПЕРЕДЕЛАТЬ!!!! оставить только 10 статистик
            foreach (var s in table)
            {
                if (i >= n_count)
                {
                    False = s.False; True = s.True;
                    canvas.DrawRect(left, bottom - (height * False), right, bottom, red);
                    canvas.DrawText((i + 1).ToString(), right, bottom + 30, text_paint); //вывод номера теста
                    left = left + width; right = left + width;
                    canvas.DrawRect(left, bottom - (height * True), right, bottom, green);
                    left = left + width; right = left + width;
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