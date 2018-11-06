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
        ListView DictionaryWords { get; set; }
        List<DBWords> WordDatabase = DBWords.GetData;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Languages_ViewDictionary);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguagesDelete);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
   
            DictionaryWords = FindViewById<ListView>(Resource.Id.listView_dictionary);    
            WordDatabase.Sort((x, y) => x.Word.CompareTo(y.Word));
            DictionaryWords.Adapter = new CustomAdapter(this, WordDatabase);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.search, menu);
            var _searchView = menu.FindItem(Resource.Id.action_search).ActionView.JavaCast<Android.Support.V7.Widget.SearchView>();

            _searchView.QueryTextChange += (sender, e) =>
            {
                if (e.NewText == "")
                    DictionaryWords.Adapter = new CustomAdapter(this, WordDatabase);
                else
                {
                    List<DBWords> FD = new List<DBWords>();
                    foreach (var word in WordDatabase)
                        if (word.Word.Substring(0, ((e.NewText.Length > word.Word.Length) ? 0 : e.NewText.Length)) == e.NewText)
                            FD.Add(word);
                    DictionaryWords.Adapter = new CustomAdapter(this, FD);
                }
            };

            DictionaryWords.ItemClick += (s, args) =>
            {
                var word = DictionaryWords.Adapter.GetItem(args.Position);
                DBWords words = new DBWords();
                foreach (var item in WordDatabase)
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
                    WordDatabase.Remove(words);
                    DictionaryWords.Adapter = new CustomAdapter(this, WordDatabase);
                    DBWords.Delete(word.ToString());
                    Toast.MakeText(this, GetString(Resource.String.Word_Delete), ToastLength.Short).Show();
                    
                });
                alert.Show();
            };
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.increase)
            {
                WordDatabase.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                DictionaryWords.Adapter = new CustomAdapter(this, WordDatabase);
            }
            else if (item.ItemId == Resource.Id.decrease)
            {
                WordDatabase.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                DictionaryWords.Adapter = new CustomAdapter(this, WordDatabase);
            }
            else if (item.ItemId == Resource.Id.ABC)
            {
                WordDatabase.Sort((x, y) => x.Word.CompareTo(y.Word));
                DictionaryWords.Adapter = new CustomAdapter(this, WordDatabase);
            }
            else if (item.ItemId == Android.Resource.Id.Home)
                Finish();
            
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
}

