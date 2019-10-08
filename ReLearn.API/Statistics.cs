using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Settings;
using ReLearn.API.Database;

namespace ReLearn.API
{
    public static class Statistics
    {
        public static int Count
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.Count}{DataBase.TableName}", 0);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.Count}{DataBase.TableName}", value);
        }

        public static int True
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.True}{DataBase.TableName}", 0);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.True}{DataBase.TableName}", value);
        }

        public static int False
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.False}{DataBase.TableName}", 0);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.False}{DataBase.TableName}", value);
        }

        public static float GetAverageNumberLearn(List<DBStatistics> database)
        {
            if (database.Count == 0)
                return Settings.StandardNumberOfRepeats;
            var avg_numberLearn_stat = database.Sum(
                                           r => r.NumberLearn > Settings.StandardNumberOfRepeats
                                               ? Settings.StandardNumberOfRepeats
                                               : r.NumberLearn) / (float) database.Count;
            return avg_numberLearn_stat;
        }

        public static async Task Add(List<DatabaseWords> wordDatabase, int currentWordNumber, int answer)
        {
            wordDatabase[currentWordNumber].NumberLearn += answer;

            var value = wordDatabase[currentWordNumber].NumberLearn > Settings.MaxNumberOfRepeats
                ?
                Settings.MaxNumberOfRepeats
                : wordDatabase[currentWordNumber].NumberLearn < 0
                    ? 0
                    : wordDatabase[currentWordNumber].NumberLearn;

            await DatabaseWords.Update(wordDatabase[currentWordNumber].Word, value);
        }

        public static async Task Add(List<DatabaseImages> imagesDatabase, int currentWordNumber, int answer)
        {
            imagesDatabase[currentWordNumber].NumberLearn += answer;

            var value = imagesDatabase[currentWordNumber].NumberLearn > Settings.MaxNumberOfRepeats
                ?
                Settings.MaxNumberOfRepeats
                : imagesDatabase[currentWordNumber].NumberLearn < 0
                    ? 0
                    : imagesDatabase[currentWordNumber].NumberLearn;
            await DatabaseImages.Update(imagesDatabase[currentWordNumber].Image_name, value);
        }

        public static void Delete()
        {
            Count = True = False = 0;
        }
    }
}