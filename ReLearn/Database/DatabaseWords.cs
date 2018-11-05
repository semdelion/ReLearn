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
using SQLite;

namespace ReLearn
{
    public class DBWords //Класс для считывания базы данных English
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Word { get; set; }
        public string TranslationWord { get; set; }
        public int NumberLearn { get; set; }
        public DateTime DateRecurrence { get; set; }

        public DBWords()
        {
            DateRecurrence = DateTime.Today;
            Word = "";
            NumberLearn = Settings.StandardNumberOfRepeats;
            TranslationWord = "";
        }

        public DBWords(DBWords x)
        {
            DateRecurrence = x.DateRecurrence;
            Word = x.Word;
            NumberLearn = x.NumberLearn;
            TranslationWord = x.TranslationWord;
        }

        public DBWords Find() => this;

    }
}