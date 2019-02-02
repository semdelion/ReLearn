using Plugin.Settings;
using SQLite;
using System;
using System.IO;

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
        const string _statistics  = "database_statistics.db3"; 
        const string _english     = "database_words.db3"; 
        const string _flags       = "database_image.db3";

        public static SQLiteConnection Languages;
        public static SQLiteConnection Images;
        public static SQLiteConnection Statistics;

        public static TableNames TableName
        {
            get
            {
                Enum.TryParse(CrossSettings.Current.GetValueOrDefault($"{DBSettings.DictionaryName}", $"{TableNames.Popular_Words}"), out TableNames name);
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

        static SQLiteConnection Connect(string nameDB) => new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), nameDB));
    }
}