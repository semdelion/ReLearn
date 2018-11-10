using System;
using Plugin.Settings;

namespace ReLearn
{
    enum DBSettings
    {
        Language,
        Pronunciation,
        Language_repeat_count,
        Images_repeat_count,
        DictionaryNameLanguages,
        DictionaryNameImage,
        Count,
        True,
        False
    }
    enum Language
    {
        en,
        ru
    }

    enum Pronunciation
    {
        en,
        uk
    }

    static class Settings // Маааагия!
    {
        public const int MaxNumberOfRepeats = 12;
        public const int StandardNumberOfRepeats = 6;
        public const int FalseAnswer = 3;
        public const int NeutralAnswer = 1;
        public const int TrueAnswer = 1;
        public const string font = "fonts/Roboto-Regular.ttf";

        public static int NumberOfRepeatsImage
        {
            get => Convert.ToInt32(CrossSettings.Current.GetValueOrDefault(DBSettings.Images_repeat_count.ToString(), "20"));
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.Images_repeat_count.ToString(), Convert.ToString(value));
        }

        public static int NumberOfRepeatsLanguage
        {
            get => Convert.ToInt32(CrossSettings.Current.GetValueOrDefault(DBSettings.Language_repeat_count.ToString(), "20"));
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.Language_repeat_count.ToString(), Convert.ToString(value));
        }

        public static string CurrentPronunciation
        {
            get => CrossSettings.Current.GetValueOrDefault(DBSettings.Pronunciation.ToString(), Pronunciation.en.ToString());
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.Pronunciation.ToString(), Convert.ToString(value));
        }

        public static string Currentlanguage
        {
            get => CrossSettings.Current.GetValueOrDefault(DBSettings.Language.ToString(), Language.en.ToString());
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.Language.ToString(), Convert.ToString(value));
        }
    }
}