using System;
using System.Collections.Generic;
using System.Linq;

namespace ReLearn.Core.Helpers
{
    public static class RandomNumbers
    {
        public static void RandomFourNumbers(int notI, int count, out List<int> randomNumbers)
        {
            const int four = 4;
            var rand = new Random(unchecked((int) DateTime.Now.Ticks));
            randomNumbers = new List<int> {notI, 0, 0, 0};
            if (count > four)
            {
                var array = Enumerable.Range(0, count).ToList();
                for (var i = 1; i < four; i++)
                {
                    array.Remove(randomNumbers[i - 1]);
                    randomNumbers[i] = array[rand.Next(array.Count)];
                }
            }
            else
            {
                for (var i = 1; i < four; i++)
                    randomNumbers[i] = (notI + i) % count;
            }
        }
    }
}