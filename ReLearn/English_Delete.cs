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
         ArrayAdapter adapter;
        //CustomAdapter adapter;
         //  List<string> dataBase;
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
            Window.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.backgroundEnFl));

            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(127, 0, 0, 0));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            listViewDel = FindViewById<ListView>(Resource.Id.listViewDelete);


         


            var db = DataBase.Connect(NameDatabase.English_DB);
            db.CreateTable<Database>();
            //dataBase = new List<string>();
            //var table = db.Table<Database>();
            //foreach (var word in table)
            //    dataBase.Add(word.enWords);

            dataBase = new List<Words>();
            var table = db.Table<Database>();
            foreach (var word in table)
            { // создание БД в виде  List<DatabaseOfWords>
                Words w = new Words()
                {
                    enWords = word.enWords,
                    ruWords = word.ruWords,
                    numberLearn = word.numberLearn,
                    dateRepeat = word.dateRepeat
                };

                dataBase.Add(w);
                
            }
            var adapter = new CustomAdapter(this, dataBase);
            listViewDel.Adapter = adapter;

            
            //adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, dataBase);
            //listViewDel.DrawingCacheBackgroundColor = Android.Graphics.Color.Gold;
            //listViewDel.CacheColorHint = Android.Graphics.Color.Gold;
            //listViewDel.SetAdapter(adapter); // заполнение listView 
            //listViewDel.CacheColorHint = Android.Graphics.Color.Argb(127, 0, 50, 0);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }

        public override bool OnCreateOptionsMenu(IMenu menu) // удаление слова по клику из списка
        {
            this.MenuInflater.Inflate(Resource.Menu.search, menu);
            var searchItem = menu.FindItem(Resource.Id.action_search);
            _searchView = searchItem.ActionView.JavaCast<Android.Widget.SearchView>();

            _searchView.QueryTextChange += (sender, e) => {
                adapter.Filter.InvokeFilter(e.NewText);
            };

            listViewDel.ItemClick += (s, args) => {

                //Words t = listViewDel.Adapter.Get;


                var word = listViewDel.Adapter.GetItem(args.Position);
                //var selectedFromList = listViewDel.GetItemAtPosition(args.Position);


                // var word = listViewDel.GetItemAtPosition(args.Position);
                // var word = listViewDel.GetI[args.Position];
                AlertDialog.Builder alert = new AlertDialog.Builder(this);

                //alert.SetTitle("");
                //alert.SetMessage("To delete : " + word.ToString() + " ? ");
                //alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
                //alert.SetNeutralButton("ок", delegate
                //{
                //    //dataBase.Remove(word.ToString());
                //    //adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, dataBase);
                //    //listViewDel.SetAdapter(adapter);
                //    //var database = DataBase.Connect(NameDatabase.English_DB);
                //    //database.CreateTable<Database>();
                //    //var search_occurrences = database.Query<Database>("SELECT * FROM Database WHERE enWords = ?", word.ToString());// поиск вхождения слова в БД              
                //    //if (search_occurrences.Count == 0)
                //    //    Toast.MakeText(this, "Words do not exist!", ToastLength.Short).Show();
                //    //else
                //    //{
                //    //    database.Query<Database>("DELETE FROM Database WHERE enWords = ?", word.ToString());
                //    //    Toast.MakeText(this, "Word delete!", ToastLength.Short).Show();
                //    //}
                //});
                //alert.Show();
            };
            return base.OnCreateOptionsMenu(menu);
        }

    }
   


}

