using System;
using System.Collections.Generic;
using System.Linq;
using Calligraphy;
using Android.Widget;
using Android.Graphics;
using Plugin.Settings;
using Java.Util;
using Android.Content.Res;
using Android.Content;

namespace ReLearn
{
    enum StateButton
    {
        Next,
        Unknown
    }

    class ButtonNext
    {
        public StateButton State { get; set; }
        public Button button = null;
    }

    static class AdditionalFunctions
    {
        static void Color_TextView(TextView TextV, Color color)
        {
            int TextTransparence = 225,
                BackgroundTransparence = 10;
            TextV.SetTextColor(Color.Argb(TextTransparence, color.R, color.G, color.B));
            TextV.SetBackgroundColor(Color.Argb(BackgroundTransparence, color.R, color.G, color.B));
        }

        public static void SetColorForItems(int degreeOfStudy, TextView TView)
        {
            if (degreeOfStudy == Settings.StandardNumberOfRepeats)
                Color_TextView(TView, new Color(238, 252, 255));
            else if (degreeOfStudy > Settings.StandardNumberOfRepeats)
                Color_TextView(TView, new Color(230,
                200 - ((degreeOfStudy - Settings.StandardNumberOfRepeats) * 180 / Settings.StandardNumberOfRepeats), 20));           //  230, 20, 20   to   230, 200, 20
            else
                Color_TextView(TView,
                    new Color(20 + (degreeOfStudy * 180 / (Settings.StandardNumberOfRepeats - 1)), 230, 20));                        //  180, 230, 20   to   20,  230, 20
        }

        public static void RandomFourNumbers(int NotI, int count, out List<int> random_numbers)
        {
            const int four = 4;
            System.Random rand = new System.Random(unchecked((int)(DateTime.Now.Ticks)));
            random_numbers = new List<int> { NotI, 0, 0, 0 };
            if (count > four)
            {
                var array = Enumerable.Range(0, count).ToList();
                for (int i = 1; i < four; i++)
                {
                    array.Remove(random_numbers[i - 1]);
                    random_numbers[i] = array[rand.Next(array.Count)];
                }
            }
            else
                for (int i = 1; i < four; i++)
                    random_numbers[i] = (NotI + i) % count;
        }

        public static string NameOfTheFlag(DBImages image) =>
            Settings.Currentlanguage == Language.en.ToString() ? image.Name_image_en : image.Name_image_ru;

        public static string RoundOfNumber(float number)
        {
            var numberChar = Convert.ToString(number);
            if (numberChar.Length > 4)
                numberChar = numberChar.Remove(4);
            else if (numberChar.Contains(","))
                numberChar += "0";
            else
            {
                if (numberChar.Length == 2)
                    numberChar += ".0";
                else if (numberChar.Length == 1)
                    numberChar += ".00";
            }
            return numberChar;
        }

        public static string GetResourceString(string str, Android.Content.Res.Resources resource)
        {
            try
            {
                var resourceId = (int)typeof(Resource.String).GetField(str).GetValue(null);
                return resource.GetString(resourceId);
            }
            catch
            {
                return "Error: Error can't find string  - " + str;
            }
        }

        public static void Font()
        {
            CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder()
               .SetDefaultFontPath(Settings.font)
               .SetFontAttrId(Resource.Attribute.fontPath)
               .Build());
        }

        public static void Update_Configuration_Locale(Android.Content.Res.Resources resource) //TODO
        {
            Locale locale = new Locale(Settings.Currentlanguage);
            Configuration conf = new Configuration { Locale = locale };
            resource.UpdateConfiguration(conf, resource.DisplayMetrics);
            //this.CreateConfigurationContext(conf);
        }

        //public static int GetColor(Context context, int id)
        //{
        //    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.Build.VERSION_CODES.M)
        //    {
        //        return context.GetColor(id);
        //    }
        //    else
        //    {
        //        //noinspection deprecation
        //        return context.Resources.GetColor(id);
        //    }
        //}

        //db.Query<Database_Words>("UPDATE " + TableNameLanguage + " SET DateRecurrence = DATETIME('NOW') WHERE Word = ?", s.Word);
        //db.Query<Database_Words>("UPDATE " + TableNameLanguage + " SET NumberLearn = " + s.NumberLearn + 1 + " WHERE Word = ?", s.Word);
    }
}