using System;
using Plugin.Settings;

namespace ReLearn.API
{
    public enum DBSettings
    {
        Language,
        Pronunciation,
        Language_repeat_count,
        Images_repeat_count,
        TimeToBlitz,
        DictionaryNameLanguages,
        DictionaryNameImage,
        Count,
        True,
        False,
        HideStudied,
        TypeOfRepetition,
        BlitzEnable
    }

    public enum Language
    {
        en,
        ru
    }

    public enum Pronunciation
    {
        en,
        uk
    }

    public enum TypeOfRepetitions
    {
        FourOptions,
        Blitz
    }

    public static class Settings // Маааагия!
    {
        public const int MaxNumberOfRepeats = 12;
        public const int StandardNumberOfRepeats = 6;
        public const int FalseAnswer = 3;
        public const int NeutralAnswer = 1;
        public const int TrueAnswer = 1;
        public const string font = "fonts/MyriadProSemibold.ttf";

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

        public static int TimeToBlitz
        {
            get => CrossSettings.Current.GetValueOrDefault(DBSettings.TimeToBlitz.ToString(), 45);
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.TimeToBlitz.ToString(), value);
        }

        public static string CurrentPronunciation
        {
            get => CrossSettings.Current.GetValueOrDefault(DBSettings.Pronunciation.ToString(), Pronunciation.uk.ToString());
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.Pronunciation.ToString(), Convert.ToString(value));
        }

        public static string Currentlanguage
        {
            get => CrossSettings.Current.GetValueOrDefault(DBSettings.Language.ToString(), Language.en.ToString());
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.Language.ToString(), Convert.ToString(value));
        }

        public static bool BlitzEnable
        {
            get => CrossSettings.Current.GetValueOrDefault(DBSettings.BlitzEnable.ToString(), true);
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.BlitzEnable.ToString(), value);
        }

        public static TypeOfRepetitions TypeOfRepetition
        {
            get
            {
                Enum.TryParse(CrossSettings.Current.GetValueOrDefault(DBSettings.TypeOfRepetition.ToString(), TypeOfRepetitions.FourOptions.ToString()), out TypeOfRepetitions name);
                return name;
            }
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.TypeOfRepetition.ToString(), Convert.ToString(value));
        }
    }
}