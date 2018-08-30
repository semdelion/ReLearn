using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReLearn
{
    public static class Magic_constants // Маааагия!
    {
        public static int repeat_count = 20; // количество повторений;
        public static int maxLearn = 12;
        public static int numberLearn = 6;
        public static int false_answer = 3;
        public static int true_answer = 1;
        public static int language = 0; // 0 - eng, 1 - rus ...
    }

    static class Additional_functions
    {
        public static void Random_4_numbers(int NotI, int count, out List<int> random_numbers)
        {
            System.Random rand = new System.Random(unchecked((int)(DateTime.Now.Ticks)));
            random_numbers = new List<int> { NotI, 0, 0, 0 };
            if (count > 4)
            {
                int rnd;
                random_numbers[1] = (NotI + rand.Next(count)) % count;
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
                random_numbers[1] = (NotI + 1) % 4;
                random_numbers[2] = (NotI + 2) % 4;
                random_numbers[3] = (NotI + 3) % 4;
            }
        }

        public static string Name_of_the_flag(Database_Flags word) // возвращает имя флага на языке  // 0 - eng / 1 - rus
        {
            if (Magic_constants.language == 0)
                return word.Name_flag_en;
            if (Magic_constants.language == 1)
                return word.Name_flag_ru;
            return "";
        }

        public static void Update_number_learn(List<Statistics> Stats, String identifier, int rand_word, int RepeatLearn)
        {
            for (int i = 0; i < Stats.Count; i++)
                if (Stats[i].Word == identifier)
                {
                    Stats[i] = new Statistics(rand_word, identifier, RepeatLearn);
                    return;
                }
            Stats.Add(new Statistics(rand_word, identifier, RepeatLearn));
        }
    }
}