using System;
using Android.Content;
using Calligraphy;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Plugin.Settings;
using Android.Support.V7.App;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class English : AppCompatActivity
    {
        [Java.Interop.Export("Button_English_Add_Click")]
        public void Button_English_Add_Click(View v)
        {
            Intent intent_english_add = new Intent(this, typeof(English_Add));
            StartActivity(intent_english_add);
        }

        [Java.Interop.Export("Button_English_Learn_Click")]
        public void Button_English_Learn_Click(View v)
        {
            try
            {
                var database = DataBase.Connect(Database_Name.English_DB);
                database.CreateTable<Database_Words>();
                int search_occurrences = database.Query<Database_Words>("SELECT * FROM " + DataBase.TableNameLanguage).Count;
                if (search_occurrences != 0)
                {
                    Intent intent_english_learn = new Intent(this, typeof(English_Learn));
                    StartActivity(intent_english_learn);
                }
                else
                    Toast.MakeText(this, GetString(Resource.String.DatabaseEmpty), ToastLength.Short).Show();
            }
            catch
            {
                Toast.MakeText(this, GetString(Resource.String.DatabaseNotConnect), ToastLength.Short).Show();
            }
        }

        [Java.Interop.Export("Button_English_Repeat_Click")]
        public void Button_English_Repeat_Click(View v)
        {
            try
            {
                var database = DataBase.Connect(Database_Name.English_DB);
                database.CreateTable<Database_Words>();
                var search_occurrences = database.Query<Database_Words>("SELECT * FROM  " + DataBase.TableNameLanguage);// поиск вхождения слова в БД
                var search_numberlearn_null = database.Query<Database_Words>("SELECT * FROM  " + DataBase.TableNameLanguage + " WHERE NumberLearn = 0").Count;
                if (search_occurrences.Count == search_numberlearn_null)
                    Toast.MakeText(this, GetString(Resource.String.RepeatedAllWords), ToastLength.Short).Show();
                else if (search_occurrences.Count != 0)
                {
                    Intent intent_english_repeat = new Intent(this, typeof(English_Repeat));
                    StartActivity(intent_english_repeat);
                }
                else
                    Toast.MakeText(this, GetString(Resource.String.DatabaseEmpty), ToastLength.Short).Show();              
            }
            catch
            {
                Toast.MakeText(this, GetString(Resource.String.DatabaseNotConnect), ToastLength.Short).Show();
            }
        }    

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEnglish);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            DataBase.UpdateWordsToRepeat();
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_english, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.Stats){
                Intent intent_english_stat = new Intent(this, typeof(English_Stat));
                StartActivity(intent_english_stat);
                return true;
            }
            if(item.ItemId == Resource.Id.Deleteword){
                Intent intent_english_add = new Intent(this, typeof(English_View_Dictionary));
                StartActivity(intent_english_add);
                return true;
            }
            if (item.ItemId == Resource.Id.MenuSelectDictionary)
            {
                Intent intent_SelectDictionary = new Intent(this, typeof(SelectDictionary));
                StartActivity(intent_SelectDictionary);
                return true;
            }
            if (item.ItemId == Android.Resource.Id.Home)
            {
                this.Finish();
                return true;
            }
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}