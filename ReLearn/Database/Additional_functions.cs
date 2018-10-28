using System;
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
using Java.Util;
using Android.Content.Res;

namespace ReLearn
{
    enum StateButton
    {
        Next,
        Unknown
    }

    enum Settings
    {
        Language,
        Language_repeat_count,
        Images_repeat_count,
        DictionaryNameLanguages,
        DictionaryNameImage
    }

    class ButtonNext
    {
        public StateButton State { get; set; }
        public Button button = null;
    }

    static class Magic_constants // Маааагия!
    {
        public const int MaxNumberOfRepeats = 12;
        public const int StandardNumberOfRepeats = 6;
        public const int FalseAnswer = 3;
        public const int NeutralAnswer = 1;
        public const int TrueAnswer = 1;
        public const string font = "fonts/Roboto-Regular.ttf";

        public static int NumberOfRepeatsImage {
            get{
                if (System.String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault(Settings.Images_repeat_count.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(Settings.Images_repeat_count.ToString(), "20");
                return Convert.ToInt32(CrossSettings.Current.GetValueOrDefault(Settings.Images_repeat_count.ToString(), null));
            }
            set{
                CrossSettings.Current.AddOrUpdateValue(Settings.Images_repeat_count.ToString(), Convert.ToString(value));
            }
        }

        public static int NumberOfRepeatsLanguage 
        {
            get{ 
                if (System.String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault(Settings.Language_repeat_count.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(Settings.Language_repeat_count.ToString(), "20");
                return Convert.ToInt32(CrossSettings.Current.GetValueOrDefault(Settings.Language_repeat_count.ToString(), null));
            }
            set { 
                CrossSettings.Current.AddOrUpdateValue(Settings.Language_repeat_count.ToString(), Convert.ToString(value));
            }
        }
    }

    static class Additional_functions
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
            if (degreeOfStudy == Magic_constants.StandardNumberOfRepeats)
                Color_TextView(TView, new Color(238, 252, 255));
            else if (degreeOfStudy > Magic_constants.StandardNumberOfRepeats) 
                Color_TextView(TView, new Color(230, 
                200 - ((degreeOfStudy - Magic_constants.StandardNumberOfRepeats) * 180 / Magic_constants.StandardNumberOfRepeats), 20));     //  230, 20, 20   to   230, 200, 20
            else
                Color_TextView(TView, 
                    new Color(20 + (degreeOfStudy * 180 / (Magic_constants.StandardNumberOfRepeats - 1)), 230, 20));                        //  180, 230, 20   to   20,  230, 20
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
                for(int i = 1; i < four; i++)
                    random_numbers[i] = (NotI + i) % count;
            
        }

        public static string NameOfTheFlag(Database_images image) => 
            CrossSettings.Current.GetValueOrDefault(Settings.Language.ToString(), null) == Language.en.ToString() ? image.Name_image_en : image.Name_image_ru;
        
        public static void UpdateNumberLearn(List<Statistics> Stats, string identifier, int rand_word, int RepeatLearn)
        {
            for (int i = 0; i < Stats.Count; i++)
                if (Stats[i].Word == identifier)
                {
                    Stats[i] = new Statistics(rand_word, identifier, RepeatLearn);
                    return;
                }
            Stats.Add(new Statistics(rand_word, identifier, RepeatLearn));
        }

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
               .SetDefaultFontPath(Magic_constants.font)
               .SetFontAttrId(Resource.Attribute.fontPath)
               .Build());
        }

        public static void Update_Configuration_Locale(Android.Content.Res.Resources resource) //TODO
        {
            if (System.String.IsNullOrEmpty(Plugin.Settings.CrossSettings.Current.GetValueOrDefault(Settings.Language.ToString(), null)))
                Plugin.Settings.CrossSettings.Current.AddOrUpdateValue(Settings.Language.ToString(), Language.en.ToString());
            Locale locale = new Locale(Plugin.Settings.CrossSettings.Current.GetValueOrDefault(Settings.Language.ToString(), null));
            Configuration conf = new Configuration { Locale = locale };
            resource.UpdateConfiguration(conf, resource.DisplayMetrics);
            //this.CreateConfigurationContext(conf);
        }
    }
}