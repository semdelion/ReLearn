using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Plugin.Settings;
using ReLearn.Droid.Resources;

namespace ReLearn.Droid.Languages
{
    [Activity( Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class ViewDictionary : AppCompatActivity
    {
        ListView DictionaryWords { get; set; }
        List<DBWords> WordDatabase = DBWords.GetData;
        public static bool HideStudied
        {
            get => CrossSettings.Current.GetValueOrDefault(DBSettings.HideStudied.ToString(), true);
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.HideStudied.ToString(), value);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ViewDictionary_Languages);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguagesDelete);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            DictionaryWords = FindViewById<ListView>(Resource.Id.listView_dictionary);    
            WordDatabase.Sort((x, y) => x.Word.CompareTo(y.Word));
            DictionaryWords.Adapter = new CustomAdapterWord(this, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn !=0 ) : WordDatabase);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.search, menu);
            var _searchView = menu.FindItem(Resource.Id.action_search).ActionView.JavaCast<Android.Support.V7.Widget.SearchView>();
            menu.FindItem(Resource.Id.HideStudied).SetChecked(HideStudied);

            _searchView.QueryTextChange += (sender, e) =>
            {
                if (e.NewText == "")
                    DictionaryWords.Adapter = new CustomAdapterWord(this, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                else
                {
                    List<DBWords> FD = new List<DBWords>();
                    foreach (var word in WordDatabase)
                        if (word.Word.Substring(0, ((e.NewText.Length > word.Word.Length) ? 0 : e.NewText.Length)) == e.NewText)
                            FD.Add(word);
                    DictionaryWords.Adapter = new CustomAdapterWord(this, FD);
                }
            };

            DictionaryWords.ItemClick += (s, args) =>
            {
                var word = DictionaryWords.Adapter.GetItem(args.Position);
                DBWords words = new DBWords();
                words = WordDatabase[WordDatabase.FindIndex(obj => obj.Word == word.ToString())];
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
               
                alert.SetTitle("");
                alert.SetMessage($"To delete : {word.ToString()}? ");
                alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
                alert.SetNeutralButton("ок", delegate
                {
                    WordDatabase.Remove(words);
                    DictionaryWords.Adapter = new CustomAdapterWord(this, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    DBWords.Delete(word.ToString());
                    Toast.MakeText(this, GetString(Resource.String.Word_Delete), ToastLength.Short).Show();
                    
                });
                alert.Show();
            };
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.increase:
                    WordDatabase.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                    DictionaryWords.Adapter = new CustomAdapterWord(this, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    return true;
                case Resource.Id.decrease:
                    WordDatabase.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                    DictionaryWords.Adapter = new CustomAdapterWord(this, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    return true;
                case Resource.Id.ABC:
                    WordDatabase.Sort((x, y) => x.Word.CompareTo(y.Word));
                    DictionaryWords.Adapter = new CustomAdapterWord(this, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    return true;
                case Resource.Id.HideStudied:
                    HideStudied = !HideStudied;
                    item.SetChecked(HideStudied);
                    DictionaryWords.Adapter = new CustomAdapterWord(this, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    return true;
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
}

