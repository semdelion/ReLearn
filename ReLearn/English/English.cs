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
        private int selected = 0;

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
                    Toast.MakeText(this, Additional_functions.GetResourceString("databaseEmpty", this.Resources), ToastLength.Short).Show();
            }
            catch
            {
                Toast.MakeText(this, Additional_functions.GetResourceString("databaseNotConnect", this.Resources), ToastLength.Short).Show();
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
                    Toast.MakeText(this, Additional_functions.GetResourceString("repeatedAllWords", this.Resources), ToastLength.Short).Show();
                else if (search_occurrences.Count != 0)
                {
                    Intent intent_english_repeat = new Intent(this, typeof(English_Repeat));
                    StartActivity(intent_english_repeat);
                }
                else
                    Toast.MakeText(this, Additional_functions.GetResourceString("databaseEmpty", this.Resources), ToastLength.Short).Show();              
            }
            catch
            {
                Toast.MakeText(this, Additional_functions.GetResourceString("databaseNotConnect", this.Resources), ToastLength.Short).Show();
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
            SetSelected();
            DataBase.UpdateWordsToRepeat();
        }

        void SetSelected() // TODO to new layout just crutch.
        {
            if (DataBase.TableNameLanguage == TableNames.My_Directly.ToString())
                selected = Resource.Id.menuDatabase_MyDictionary;
            else if (DataBase.TableNameLanguage == TableNames.Home.ToString())
                selected = Resource.Id.menuDatabase_Home;
            else if (DataBase.TableNameLanguage == TableNames.Education.ToString())
                selected = Resource.Id.menuDatabase_Education;
            else if (DataBase.TableNameLanguage == TableNames.Popular_Words.ToString())
                selected = Resource.Id.menuDatabase_PopularWords;
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_english, menu);
            if (selected == Resource.Id.menuDatabase_MyDictionary)
            {
                menu.FindItem(Resource.Id.menuDatabase_MyDictionary).SetChecked(true);
                return true;
            }
            if (selected == Resource.Id.menuDatabase_Home)
            {
                menu.FindItem(Resource.Id.menuDatabase_Home).SetChecked(true);
                return true;
            }
            if (selected == Resource.Id.menuDatabase_Education)
            {
                menu.FindItem(Resource.Id.menuDatabase_Education).SetChecked(true);
                return true;
            }
            if (selected == Resource.Id.menuDatabase_PopularWords)
            {
                menu.FindItem(Resource.Id.menuDatabase_PopularWords).SetChecked(true);
                return true;
            }
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.menuDatabase_MyDictionary)
            {
                DataBase.TableNameLanguage = TableNames.My_Directly.ToString();
                Toast.MakeText(this, Additional_functions.GetResourceString("MyDictionaryIsSelected", this.Resources), ToastLength.Short).Show();
                DataBase.UpdateWordsToRepeat();
                item.SetChecked(true);
                return true;
            }
            if (id == Resource.Id.menuDatabase_PopularWords)
            {
                DataBase.TableNameLanguage = TableNames.Popular_Words.ToString();
                Toast.MakeText(this, Additional_functions.GetResourceString("PopularDictionaryIsSelected", this.Resources), ToastLength.Short).Show();
                DataBase.UpdateWordsToRepeat();
                item.SetChecked(true);
                return true;
            }
            if (id == Resource.Id.menuDatabase_Home)
            {
                DataBase.TableNameLanguage = TableNames.Home.ToString();
                Toast.MakeText(this, Additional_functions.GetResourceString("HomeIsSelected", this.Resources), ToastLength.Short).Show();
                DataBase.UpdateWordsToRepeat();
                item.SetChecked(true);
                return true;
            }
            if (id == Resource.Id.menuDatabase_Education)
            {
                DataBase.TableNameLanguage = TableNames.Education.ToString();
                Toast.MakeText(this, Additional_functions.GetResourceString("EducationIsSelected", this.Resources), ToastLength.Short).Show();
                DataBase.UpdateWordsToRepeat();
                item.SetChecked(true);
                return true;
            }
            if (id == Resource.Id.Stats){
                Intent intent_english_stat = new Intent(this, typeof(English_Stat));
                StartActivity(intent_english_stat);
                return true;
            }
            if(id == Resource.Id.Deleteword){
                Intent intent_english_add = new Intent(this, typeof(English_View_Dictionary));
                StartActivity(intent_english_add);
                return true;
            }
            if (id == Android.Resource.Id.Home)
            {
                this.Finish();
                return true;
            }
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}