using System;
using System.IO;
using Plugin.Settings;
using SQLite;

namespace ReLearn.API.Database
{
    public enum TableNames
    {
        Flags,
        Films,

        ///////////////////////////////////
        My_Directly,
        Home,
        Education,
        Popular_Words,
        ThreeFormsOfVerb,
        ComputerScience,
        Nature
    }

    public static class DataBase
    {
        private const string _statistics = "database_statistics.db3";
        private const string _english = "database_words.db3";
        private const string _flags = "database_image.db3";

        public static SQLiteAsyncConnection Languages { get; private set; }
        public static SQLiteAsyncConnection Images { get; private set; }
        public static SQLiteAsyncConnection Statistics { get; private set; }

        public static TableNames TableName
        {
            get
            {
                Enum.TryParse(
                    CrossSettings.Current.GetValueOrDefault($"{DBSettings.DictionaryName}",
                        $"{TableNames.Popular_Words}"), out TableNames name);
                return name;
            }
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.DictionaryName}", $"{value}");
        }

        public static void SetupConnection()
        {
            Languages = Connect(_english);
            Images = Connect(_flags);
            Statistics = Connect(_statistics);
        }

        private static SQLiteAsyncConnection Connect(string nameDB)
        {
            return new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                nameDB));
        }
    }
}