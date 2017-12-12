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

namespace ReLearn
{
    [Activity(Label = "Repeat 1/20", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags_Repeat : Activity
    {
        
        void Random_Button(Button B1, Button B2, Button B3, Button B4, List<DatabaseOfFlags> dataBase, int i)   //загружаем варианты ответа в текст кнопок
        {
            Random rand = new Random(unchecked((int)(DateTime.Now.Ticks)));
            B1.Text = Repeat_work.word_det(dataBase[i]);
            B2.Text = Repeat_work.word_det(dataBase[rand.Next(dataBase.Count)]);
            B3.Text = Repeat_work.word_det(dataBase[rand.Next(dataBase.Count)]);
            B4.Text = Repeat_work.word_det(dataBase[rand.Next(dataBase.Count)]);
        }

        void Function_Next_Test(Button B1, Button B2, Button B3, Button B4, Button BNext, ImageView imageView, List<DatabaseOfFlags> dataBase, int rand_word, int i_rand) //new test
        {
            imageView.SetImageResource(dataBase[rand_word].image_name);
            GUI.button_ebabled(BNext);
            switch (i_rand)
            {// задаём рандоммную кнопку                            
                case 0:
                    {
                        Random_Button(B1, B2, B3, B4, dataBase, rand_word);
                        break;
                    }
                case 1:
                    {
                        Random_Button(B2, B1, B3, B4, dataBase, rand_word);
                        break;
                    }
                case 2:
                    {
                        Random_Button(B3, B1, B2, B4, dataBase, rand_word);
                        break;
                    }
                case 3:
                    {
                        Random_Button(B4, B1, B2, B3, dataBase, rand_word);
                        break;
                    }
            }
        }

        void Answer(Button B1, Button B2, Button B3, Button B4, Button BNext, List<DatabaseOfFlags> dataBase, List<Statistics_learn> Stats, int rand_word) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            GUI.Button_enable(B1, B2, B3, B4, BNext);
            if (B1.Text == Repeat_work.word_det(dataBase[rand_word]))
            {
                Repeat_work.DeleteRepeat(Stats, Convert.ToString(dataBase[rand_word].image_name), rand_word, dataBase[rand_word].numberLearn -= magic_constants.true_answer);
                Statistics_learn.answerTrue++;
                GUI.button_true(B1);
            }
            else
            {
                Repeat_work.DeleteRepeat(Stats, Convert.ToString(dataBase[rand_word].image_name), rand_word, dataBase[rand_word].numberLearn += magic_constants.false_answer);
                Statistics_learn.answerFalse++;
                GUI.button_false(B1);
                if (B2.Text == Repeat_work.word_det(dataBase[rand_word]))
                    GUI.button_true(B2);
                else if (B3.Text == Repeat_work.word_det(dataBase[rand_word]))
                    GUI.button_true(B3);
                else
                    GUI.button_true(B4);
            }
        }
        public void Update_Database(List<Statistics_learn> listdataBase) // изменение у бвзы данных элемента numberLearn
        {
            var database = DataBase.Connect(NameDatabase.Flags_DB);
            database.CreateTable<Database_Flags>();
            int month = DateTime.Today.Month;
            for (int i = 0; i < listdataBase.Count; i++)
            {
                database.Query<Database_Flags>("UPDATE Database_Flags SET dateRepeat = " + month + " WHERE image_name = ?", listdataBase[i].Word);
                database.Query<Database_Flags>("UPDATE Database_Flags SET numberLearn = " + listdataBase[i].Learn + " WHERE image_name = ?", listdataBase[i].Word);
            }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //setting layout     
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags_Repeat);

            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarFlagsRepeat);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            Window.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.backgroundEnFl));

            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(127, 0, 0, 0));

            GUI.button_default(Flags.button_flags_repeat);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Statistics_learn.answerFalse = 0;
            Statistics_learn.answerTrue = 0;
            int rand_word = 0, i_rand = 0, count = 0;
            List<Statistics_learn> Stats = new List<Statistics_learn>();

            ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView_Flags_repeat); // проверить
            Button button1 = FindViewById<Button>(Resource.Id.button_F_choice1);
            Button button2 = FindViewById<Button>(Resource.Id.button_F_choice2);
            Button button3 = FindViewById<Button>(Resource.Id.button_F_choice3);
            Button button4 = FindViewById<Button>(Resource.Id.button_F_choice4);
            Button button_next = FindViewById<Button>(Resource.Id.button_F_Next);
            button1.Touch += GUI.TouchAdd;
            button2.Touch += GUI.TouchAdd;
            button3.Touch += GUI.TouchAdd;
            button4.Touch += GUI.TouchAdd;
            button_next.Touch += GUI.TouchAdd;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //repeat flags
            try
            {
                var db = DataBase.Connect(NameDatabase.Flags_DB);
                db.CreateTable<Database_Flags>(); //
                List<DatabaseOfFlags> dataBase = new List<DatabaseOfFlags>();
                var table = db.Table<Database_Flags>();
                foreach (var word in table)
                { // создание БД в виде  List<DatabaseOfFlags>
                    DatabaseOfFlags w = new DatabaseOfFlags();
                    if (word.numberLearn != 0) //add all flags with 'numberLearn' > 0
                    {
                        w.Add(word.image_name, word.name_flag_en, word.name_flag_ru, word.numberLearn, word.dateRepeat);
                        dataBase.Add(w);
                    }
                }
                Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                rand_word = rnd.Next(dataBase.Count);
                i_rand = rnd.Next(3);                                                                                                //рандом для 4 кнопок
                Function_Next_Test(button1, button2, button3, button4, button_next, imageView, dataBase, rand_word, i_rand);
                button1.Click += (s, e) => { Answer(button1, button2, button3, button4, button_next, dataBase, Stats, rand_word); }; //лямбда оператор для подсветки ответа // true ? green:red
                button2.Click += (s, e) => { Answer(button2, button1, button3, button4, button_next, dataBase, Stats, rand_word); };
                button3.Click += (s, e) => { Answer(button3, button1, button2, button4, button_next, dataBase, Stats, rand_word); };
                button4.Click += (s, e) => { Answer(button4, button1, button2, button3, button_next, dataBase, Stats, rand_word); };
                button_next.Click += (s, e) =>
                {
                    if (count < magic_constants.repeat_count - 1)
                    {
                        i_rand = rnd.Next(3);
                        rand_word = rnd.Next(dataBase.Count);
                        Function_Next_Test(button1, button2, button3, button4, button_next, imageView, dataBase, rand_word, i_rand);
                        GUI.Button_Refresh(button1, button2, button3, button4, button_next);
                        count++;
                        ActionBar.Title = Convert.ToString("Repeat " + (count + 1) + "/" + magic_constants.repeat_count); // ПЕРЕДЕЛАЙ Костыль счётчик 
                    }
                    else
                    {
                        Repeat_work.AddStatistics(Statistics_learn.answerTrue, Statistics_learn.answerFalse, NameDatabase.Flags_DB_Stat_DB);
                        Update_Database(Stats);
                        Intent intent_flags_stat = new Intent(this, typeof(Flags_Stats));
                        StartActivity(intent_flags_stat);
                        this.Finish();
                    }

                };
            }
            catch{
                Toast.MakeText(this, "Error : can't connect to database of flags", ToastLength.Long).Show();
            }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
}