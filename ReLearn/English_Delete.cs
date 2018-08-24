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
        // ArrayAdapter adapter;
        CustomAdapter adapter;
        List<Words> dataBase;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //setting layout 
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_delete);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglishDelete);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundEnFl));

            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(127, 0, 0, 0));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            listViewDel = FindViewById<ListView>(Resource.Id.listViewDelete);        
            var db = DataBase.Connect(NameDatabase.English_DB);
            db.CreateTable<Database>();
            dataBase = new List<Words>();
            var table = db.Table<Database>();
            foreach (var word in table)
            { // создание БД в виде  List<Words>
                Words w = new Words()
                {
                    enWords = word.enWords,
                    ruWords = word.ruWords,
                    numberLearn = word.numberLearn,
                    dateRepeat = word.dateRepeat
                };
                dataBase.Add(w);                
            }

            dataBase.Sort((x, y) => x.enWords.CompareTo(y.enWords));
            //dataBase = dataBase.OrderBy(o => o.enWords).ToList();         
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
                    List<Words> FD = new List<Words>();
                    foreach (var word in dataBase)
                        if (word.enWords.Substring(0, ((e.NewText.Length > word.enWords.Length) ? 0 : e.NewText.Length)) == e.NewText)
                            FD.Add(word);
                    var ad = new CustomAdapter(this, FD);
                    listViewDel.Adapter = ad;
                }
            };

            listViewDel.ItemClick += (s, args) => {
                var word = listViewDel.Adapter.GetItem(args.Position);
                Words words = new Words();
                foreach (var item in dataBase)
                    if (item.enWords == word.ToString())
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
                    adapter = new CustomAdapter(this, /*Android.Resource.Layout.SimpleListItem1,*/ dataBase);
                    listViewDel.Adapter = adapter;
                    var database = DataBase.Connect(NameDatabase.English_DB);
                    database.CreateTable<Database>();
                    var search_occurrences = database.Query<Database>("SELECT * FROM Database WHERE enWords = ?", word.ToString());// поиск вхождения слова в БД              
                    if (search_occurrences.Count == 0)
                        Toast.MakeText(this, "Words do not exist!", ToastLength.Short).Show();
                    else
                    {
                        database.Query<Database>("DELETE FROM Database WHERE enWords = ?", word.ToString());
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
            //возрастание
            if (id == Resource.Id.increase)
            {
                dataBase.Sort((x, y) => x.numberLearn.CompareTo(y.numberLearn));
                adapter = new CustomAdapter(this, dataBase);
                listViewDel.Adapter = adapter;
            }
            //убывание
            if (id == Resource.Id.decrease)
            {
                dataBase.Sort((x, y) => y.numberLearn.CompareTo(x.numberLearn));
                adapter = new CustomAdapter(this, dataBase);
                listViewDel.Adapter = adapter;
            }
            if (id == Resource.Id.ABC)
            {
                dataBase.Sort((x, y) => x.enWords.CompareTo(y.enWords));
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

