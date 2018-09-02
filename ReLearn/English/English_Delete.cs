using System;
using System.IO;
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
using ReLearn.Resources;

namespace ReLearn
{
    [Activity(Label = "Delete", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class English_Delete : Activity
    {
        ListView listViewDel;
        SearchView _searchView;
        CustomAdapter adapter;
        List<Database_Words> dataBase;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //setting layout 
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_delete);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglishDelete);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            listViewDel = FindViewById<ListView>(Resource.Id.listViewDelete);    
            
            var db = DataBase.Connect(Database_Name.English_DB);
            dataBase = db.Query<Database_Words>("SELECT * FROM " + DataBase.Table_Name);
            dataBase.Sort((x, y) => x.Word.CompareTo(y.Word));
            
            adapter = new CustomAdapter(this, dataBase);
            listViewDel.Adapter = adapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu) // удаление слова по клику из списка
        {
            this.MenuInflater.Inflate(Resource.Menu.search, menu);
            var searchItem = menu.FindItem(Resource.Id.action_search);
            _searchView = searchItem.ActionView.JavaCast<Android.Widget.SearchView>();

            _searchView.QueryTextChange += (sender, e) =>
            {
                if (e.NewText == "")
                    listViewDel.Adapter = new CustomAdapter(this, dataBase);
                else
                {
                    List<Database_Words> FD = new List<Database_Words>();
                    foreach (var word in dataBase)
                        if (word.Word.Substring(0, ((e.NewText.Length > word.Word.Length) ? 0 : e.NewText.Length)) == e.NewText)
                            FD.Add(word);
                    var ad = new CustomAdapter(this, FD);
                    listViewDel.Adapter = ad;
                }
            };

            listViewDel.ItemClick += (s, args) => {
                var word = listViewDel.Adapter.GetItem(args.Position);
                Database_Words words = new Database_Words();
                foreach (var item in dataBase)
                    if (item.Word == word.ToString())
                    {
                        words = item.Find();
                        break;
                    }                        
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                string str_word = word.ToString();
                alert.SetTitle("");
                alert.SetMessage("To delete : " + word.ToString() + " ? ");
                alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
                alert.SetNeutralButton("ок", delegate
                {
                    dataBase.Remove(words);
                    adapter = new CustomAdapter(this, dataBase);
                    listViewDel.Adapter = adapter;

                    var database = DataBase.Connect(Database_Name.English_DB);
                    database.CreateTable<Database_Words>();
                    int search_occurrences = database.Query<Database_Words>("SELECT * FROM " + DataBase.Table_Name).Count;
                    
                    if (search_occurrences == 0)
                        Toast.MakeText(this, "Words do not exist!", ToastLength.Short).Show();
                    else
                    {
                        database.Query<Database_Words>("DELETE FROM " + DataBase.Table_Name + " WHERE Word = ?", word.ToString());// поиск вхождения слова в БД
                        Toast.MakeText(this, "Word delete!", ToastLength.Short).Show();
                    }
                });
                alert.Show();
            };
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.increase)
            {
                dataBase.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                adapter = new CustomAdapter(this, dataBase);
                listViewDel.Adapter = adapter;
            }
            if (id == Resource.Id.decrease)
            {
                dataBase.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                adapter = new CustomAdapter(this, dataBase);
                listViewDel.Adapter = adapter;
            }
            if (id == Resource.Id.ABC)
            {
                dataBase.Sort((x, y) => x.Word.CompareTo(y.Word));
                adapter = new CustomAdapter(this, dataBase);
                listViewDel.Adapter = adapter;
            }
            if (id == Android.Resource.Id.Home)
            {
                this.Finish();
            }
            return true;
        }

    }

}

