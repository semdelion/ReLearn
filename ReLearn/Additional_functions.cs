﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calligraphy;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Plugin.Settings;

namespace ReLearn
{
    public static class Magic_constants // Маааагия!
    {
        public static int repeat_count; // количество повторений;
        public static int maxLearn = 12;
        public static int numberLearn = 6;
        public static int false_answer = 3;
        public static int true_answer = 1;
        public const string font = "fonts/Roboto-Regular.ttf";

        public static void Get_repeat_count(string name)
        {
            if (System.String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault(name, null)))
                CrossSettings.Current.AddOrUpdateValue(name, "20");
            Magic_constants.repeat_count = Convert.ToInt32(CrossSettings.Current.GetValueOrDefault(name, null));
        }
        public static void Set_repeat_count(string name,int count)
        {
            CrossSettings.Current.AddOrUpdateValue(name, Convert.ToString(count));
            Magic_constants.repeat_count = Convert.ToInt32(CrossSettings.Current.GetValueOrDefault(name, null));
        }
}

    static class Additional_functions
    {
        public static void Random_4_numbers(int NotI, int count, out List<int> random_numbers)
        {
            System.Random rand = new System.Random(unchecked((int)(DateTime.Now.Ticks)));
            random_numbers = new List<int> { NotI, 0, 0, 0 };
            if (count > 4)
            {
                //(NotI + rand.Next(1,count)) % (count);
                int rnd;
                while (true)
                {
                    rnd = rand.Next(count);
                    if (rnd != random_numbers[0])
                        break;
                }
                random_numbers[1] = rnd;
                while (true)
                {
                    rnd = rand.Next(count);
                    if (rnd != random_numbers[0] && rnd != random_numbers[1])
                        break;
                }
                random_numbers[2] = rnd;
                while (true)
                {
                    rnd = rand.Next(count);
                    if (rnd != random_numbers[0] && rnd != random_numbers[1] && rnd != random_numbers[2])
                        break;
                }
                random_numbers[3] = rnd;
            }
            else
            {
                random_numbers[1] = (NotI + 1) % count;
                random_numbers[2] = (NotI + 2) % count;
                random_numbers[3] = (NotI + 3) % count;
            }
        }

        public static string Name_of_the_flag(Database_images word) => CrossSettings.Current.GetValueOrDefault("Language", null) == "en" ? word.Name_image_en : word.Name_image_ru;
        
        public static void Update_number_learn(List<Statistics> Stats, string identifier, int rand_word, int RepeatLearn)
        {
            for (int i = 0; i < Stats.Count; i++)
                if (Stats[i].Word == identifier)
                {
                    Stats[i] = new Statistics(rand_word, identifier, RepeatLearn);
                    return;
                }
            Stats.Add(new Statistics(rand_word, identifier, RepeatLearn));
        }

        public static string Round(float number)
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
                return "";
            }            
        }

        public static void Font()
        {
            CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder()
               .SetDefaultFontPath(Magic_constants.font)
               .SetFontAttrId(Resource.Attribute.fontPath)
               .Build());
        }
        
    }
}