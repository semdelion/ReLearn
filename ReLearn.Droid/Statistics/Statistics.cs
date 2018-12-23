using System.Collections.Generic;
using System.Linq;
using Plugin.Settings;
namespace ReLearn.Droid
{
    static class Statistics
    {
        public static string Table{set;get;}

        public static int Count
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.Count}{Table}", 0);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.Count}{Table}", value);
        }
        public static int True
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.True}{Table}", 0);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.True}{Table}", value);
        }
        public static int False
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.False}{Table}", 0);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.False}{Table}", value);
        }


        public static float GetAverageNumberLearn(List<DBStatistics> Database_NL_and_D)
        {
            if (Database_NL_and_D.Count == 0)
                return Settings.StandardNumberOfRepeats;
            float avg_numberLearn_stat = (float)Database_NL_and_D.Sum(
                r => r.NumberLearn > Settings.StandardNumberOfRepeats ?
                Settings.StandardNumberOfRepeats : r.NumberLearn) / (float)Database_NL_and_D.Count;
            return avg_numberLearn_stat;
        }

        public static void Add(List<DBWords> WordDatabase, int CurrentWordNumber, int answer)
        {
            WordDatabase[CurrentWordNumber].NumberLearn += answer;

            int value = WordDatabase[CurrentWordNumber].NumberLearn > Settings.MaxNumberOfRepeats ?
                        Settings.MaxNumberOfRepeats : WordDatabase[CurrentWordNumber].NumberLearn < 0 ? 
                        0 : WordDatabase[CurrentWordNumber].NumberLearn;

            DBWords.Update(WordDatabase[CurrentWordNumber].Word, value);
        }
        public static void Add(List<DBImages> ImagesDatabase, int CurrentWordNumber, int answer)
        {
            ImagesDatabase[CurrentWordNumber].NumberLearn += answer;

            int value = ImagesDatabase[CurrentWordNumber].NumberLearn > Settings.MaxNumberOfRepeats ?
                        Settings.MaxNumberOfRepeats : ImagesDatabase[CurrentWordNumber].NumberLearn < 0 ?
                        0 : ImagesDatabase[CurrentWordNumber].NumberLearn;
            DBImages.Update(ImagesDatabase[CurrentWordNumber].Image_name , value);
        }
    }

}
