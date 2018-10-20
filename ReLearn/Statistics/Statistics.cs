using System;
using SQLite;

namespace ReLearn
{
    public class Statistics
    {  
        public int Position  { get; }                        //Posithoin  in table words,
        public string Word { get; }                          //Word Ebglish or Flag - "name"
        public static int AnswerTrue = 0;                    //Answer ? true or false 
        public static int AnswerFalse = 0;
        public int Learn { get; }
        public static void Statistics_update()
        {
            AnswerTrue = 0;                    
            AnswerFalse = 0;
        }
        public Statistics(int position_new, string word_new, int Learn_new)
        {
            Position = position_new;
            Word = word_new;

            if (Learn + Learn_new > Magic_constants.MaxNumberOfRepeats)
                Learn = Magic_constants.MaxNumberOfRepeats;
            else if (Learn + Learn_new < 0)
                Learn = 0;
            else
                Learn = Learn_new;
        }

        public static void Add_Statistics(int True, int False, string TableName)      // add stats to the database
        {
            var database = DataBase.Connect(Database_Name.Statistics);
            database.Query<Database_Statistics>("INSERT INTO " + TableName + "_Statistics" + " (True, False, DateOfTesting) VALUES (" + True + "," + False + ", DATETIME('NOW'))");
        }
    }

    public class Database_Statistics                                // class for reading databse
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public int True { get; set; }
        public int False { get; set; }
        public DateTime DateOfTesting { get; set; }
    }
}
