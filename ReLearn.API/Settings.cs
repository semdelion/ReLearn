using System;
using System.Threading;
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
        BlitzEnable,
        QuizEnable,
        DictionaryName,
        AmountOfStatistics
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
        public const int MaxNumberOfStatistics = 50;
        public const int MinNumberOfStatistics = 10;
        public const int MaxNumberOfRepeats = 12;
        public const int StandardNumberOfRepeats = 6;
        public const int FalseAnswer = 3;
        public const int NeutralAnswer = 1;
        public const int TrueAnswer = 1;
        public const string font = "fonts/MyriadProSemibold.ttf";

        public static int AmountOfStatistics
        {
            get => Convert.ToInt32(CrossSettings.Current.GetValueOrDefault($"{DBSettings.AmountOfStatistics}", 10));
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.AmountOfStatistics}", value);
        }

        public static int NumberOfRepeatsImage
        {
            get => Convert.ToInt32(CrossSettings.Current.GetValueOrDefault($"{DBSettings.Images_repeat_count}", "20"));
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.Images_repeat_count}", $"{value}");
        }

        public static int NumberOfRepeatsLanguage
        {
            get =>
                Convert.ToInt32(CrossSettings.Current.GetValueOrDefault($"{DBSettings.Language_repeat_count}", "20"));
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.Language_repeat_count}", $"{value}");
        }

        public static int TimeToBlitz
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.TimeToBlitz}", 45);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.TimeToBlitz}", value);
        }

        public static string CurrentPronunciation
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.Pronunciation}", $"{Pronunciation.uk}");
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.Pronunciation}", $"{value}");
        }

        public static string Currentlanguage
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.Language}",
                Thread.CurrentThread.CurrentCulture.Name == "ru-RU" ? $"{Language.ru}" : $"{Language.en}");
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.Language}", $"{value}");
        }

        public static bool BlitzEnable
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.BlitzEnable}", true);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.BlitzEnable}", value);
        }

        public static bool QuizEnable
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.QuizEnable}", true);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.QuizEnable}", value);
        }

        public static TypeOfRepetitions TypeOfRepetition
        {
            get
            {
                Enum.TryParse(
                    CrossSettings.Current.GetValueOrDefault($"{DBSettings.TypeOfRepetition}",
                        $"{TypeOfRepetitions.FourOptions}"), out TypeOfRepetitions name);
                return name;
            }
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.TypeOfRepetition}", $"{value}");
        }
    }
}