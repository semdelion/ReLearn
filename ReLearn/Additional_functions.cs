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

    }
}