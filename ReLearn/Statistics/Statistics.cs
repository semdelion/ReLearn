using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace ReLearn
{
    public class Statistics
    {  
        public int Position  { get; }                        //Posithoin  in table words,
        public string Word { get; }                          //Word Ebglish or Flag - "name"
        public static int AnswerTrue = 0;                    //Answer ? true or false 
        public static int AnswerFalse = 0;
        public int Learn { get; set; }

        public static void CreateNewStatistics()
        {
            AnswerTrue = 0;                    
            AnswerFalse = 0;
        }

        public Statistics(int position_new, string word_new, int Learn_new)
        {
            Position = position_new;
            Word = word_new;

            if (Learn + Learn_new > Settings.MaxNumberOfRepeats)
                Learn = Settings.MaxNumberOfRepeats;
            else if (Learn + Learn_new < 0)
                Learn = 0;
            else
                Learn = Learn_new;
        }

        public static float GetAverageNumberLearn(List<DBStatistics> Database_NL_and_D)
        {
            float avg_numberLearn_stat = (float)Database_NL_and_D.Sum(
                r => r.NumberLearn > Settings.StandardNumberOfRepeats ?
                Settings.StandardNumberOfRepeats : r.NumberLearn) / (float)Database_NL_and_D.Count;
            return avg_numberLearn_stat;
        }

        public static void Add(List<Statistics> Stats, string identifier, int rand_word, int RepeatLearn, int answer)
        {
            foreach (var s in Stats)
                if (s.Word == identifier)
                {
                    s.Learn += answer;
                    return;
                }
            Stats.Add(new Statistics(rand_word, identifier, RepeatLearn + answer));
        }
    }

    public class DatabaseStatistics                                // class for reading databse
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public int True { get; set; }
        public int False { get; set; }
        public DateTime DateOfTesting { get; set; }
    }
}
