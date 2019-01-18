using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReLearn.API
{
    public static class RandomNumbers
    {
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
                for (int i = 1; i < four; i++)
                    random_numbers[i] = (NotI + i) % count;
        }
    }
}
