using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReLearn.Resources;
using Android.Support.V7.App;

namespace ReLearn
{
    [Activity( Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Languages_View_Dictionary : AppCompatActivity
    {
        ListView listViewDel;
        List<DBWords> dataBase;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Languages_ViewDictionary);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguagesDelete);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
   
            listViewDel = FindViewById<ListView>(Resource.Id.listView_dictionary);    
            
            var db = DataBase.Connect(Database_Name.English_DB);
            dataBase = db.Query<DBWords>($"SELECT * FROM {DataBase.TableNameLanguage}");
            dataBase.Sort((x, y) => x.Word.CompareTo(y.Word));
            listViewDel.Adapter = new CustomAdapter(this, dataBase);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.search, menu);
            var searchItem = menu.FindItem(Resource.Id.action_search);

            var _searchView = searchItem.ActionView.JavaCast<Android.Support.V7.Widget.SearchView>();

            _searchView.QueryTextChange += (sender, e) =>
            {
                if (e.NewText == "")
                    listViewDel.Adapter = new CustomAdapter(this, dataBase);
                else
                {
                    List<DBWords> FD = new List<DBWords>();
                    foreach (var word in dataBase)
                        if (word.Word.Substring(0, ((e.NewText.Length > word.Word.Length) ? 0 : e.NewText.Length)) == e.NewText)
                            FD.Add(word);
                    listViewDel.Adapter = new CustomAdapter(this, FD);
                }
            };

            listViewDel.ItemClick += (s, args) =>
            {
                var word = listViewDel.Adapter.GetItem(args.Position);
                DBWords words = new DBWords();
                foreach (var item in dataBase)
                    if (item.Word == word.ToString())
                    {
                        words = item.Find();
                        break;
                    }
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("");
                alert.SetMessage($"To delete : {word.ToString()}? ");
                alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
                alert.SetNeutralButton("ок", delegate
                {
                    dataBase.Remove(words);
                    listViewDel.Adapter = new CustomAdapter(this, dataBase);

                    var database = DataBase.Connect(Database_Name.English_DB);
                    database.CreateTable<DBWords>();
                    int search_occurrences = database.Query<DBWords>($"SELECT * FROM {DataBase.TableNameLanguage}").Count;

                    if (search_occurrences == 0)
                        Toast.MakeText(this, GetString(Resource.String.Word_Not_Exists), ToastLength.Short).Show();
                    else
                    {
                        database.Query<DBWords>($"DELETE FROM {DataBase.TableNameLanguage} WHERE Word = ?", word.ToString());// поиск вхождения слова в БД
                            Toast.MakeText(this, GetString(Resource.String.Word_Delete), ToastLength.Short).Show();
                    }
                });
                alert.Show();
            };
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.increase)
            {
                dataBase.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                listViewDel.Adapter = new CustomAdapter(this, dataBase);
            }
            else if (item.ItemId == Resource.Id.decrease)
            {
                dataBase.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                listViewDel.Adapter = new CustomAdapter(this, dataBase);
            }
            else if (item.ItemId == Resource.Id.ABC)
            {
                dataBase.Sort((x, y) => x.Word.CompareTo(y.Word));
                listViewDel.Adapter = new CustomAdapter(this, dataBase);
            }
            else if (item.ItemId == Android.Resource.Id.Home)
                Finish();
            
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
}

