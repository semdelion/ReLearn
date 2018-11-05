﻿using System;
using Plugin.Settings;

namespace ReLearn
{
    enum DBSettings
    {
        Language,
        Language_repeat_count,
        Images_repeat_count,
        DictionaryNameLanguages,
        DictionaryNameImage,
        Pronunciation
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
            get
            {
                if (System.String.IsNullOrEmpty(
                       CrossSettings.Current.GetValueOrDefault(DBSettings.Images_repeat_count.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(DBSettings.Images_repeat_count.ToString(), "20");
                return Convert.ToInt32(
                       CrossSettings.Current.GetValueOrDefault(DBSettings.Images_repeat_count.ToString(), null));
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(DBSettings.Images_repeat_count.ToString(), Convert.ToString(value));
            }
        }

        public static int NumberOfRepeatsLanguage
        {
            get
            {
                if (System.String.IsNullOrEmpty(
                       CrossSettings.Current.GetValueOrDefault(DBSettings.Language_repeat_count.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(DBSettings.Language_repeat_count.ToString(), "20");
                return Convert.ToInt32(
                       CrossSettings.Current.GetValueOrDefault(DBSettings.Language_repeat_count.ToString(), null));
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(DBSettings.Language_repeat_count.ToString(), Convert.ToString(value));
            }
        }

        public static string CurrentPronunciation
        {
            get
            {
                if (System.String.IsNullOrEmpty(
                       CrossSettings.Current.GetValueOrDefault(DBSettings.Pronunciation.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(DBSettings.Pronunciation.ToString(), Pronunciation.en.ToString());
                return CrossSettings.Current.GetValueOrDefault(DBSettings.Pronunciation.ToString(), null);
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(DBSettings.Pronunciation.ToString(), Convert.ToString(value));
            }
        }
        public static string Currentlanguage
        {
            get
            {
                if (System.String.IsNullOrEmpty(
                       CrossSettings.Current.GetValueOrDefault(DBSettings.Language.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(DBSettings.Language.ToString(), Language.en.ToString());
                return CrossSettings.Current.GetValueOrDefault(DBSettings.Language.ToString(), null);
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(DBSettings.Language.ToString(), Convert.ToString(value));
            }
        }
    }
}