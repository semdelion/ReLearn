using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Plugin.Settings;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.Languages;
using ReLearn.Droid.Resources;
using ReLearn.Droid.Views.Menu;
using System.Collections.Generic;

namespace ReLearn.Droid.Views.Languages
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    [Register("relearn.droid.views.languages.ViewDictionaryFragment")]
    public class ViewDictionaryFragment : BaseFragment<ViewDictionaryViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_menu_view_dictionary;

        protected override int Toolbar => Resource.Id.toolbar_view_dictionary;
        ListView DictionaryWords { get; set; }
        List<DBWords> WordDatabase = DBWords.GetData;
        public static bool HideStudied
        {
            get => CrossSettings.Current.GetValueOrDefault(DBSettings.HideStudied.ToString(), true);
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.HideStudied.ToString(), value);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            DictionaryWords = view.FindViewById<ListView>(Resource.Id.listView_dictionary);
            WordDatabase.Sort((x, y) => x.Word.CompareTo(y.Word));
            DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search, menu);
            var _searchView = menu.FindItem(Resource.Id.action_search).ActionView.JavaCast<Android.Support.V7.Widget.SearchView>();
            menu.FindItem(Resource.Id.HideStudied).SetChecked(HideStudied);

            _searchView.QueryTextChange += (sender, e) =>
            {
                if (e.NewText == "")
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                else
                {
                    List<DBWords> FD = new List<DBWords>();
                    foreach (var word in WordDatabase)
                        if (word.Word.Substring(0, ((e.NewText.Length > word.Word.Length) ? 0 : e.NewText.Length)) == e.NewText)
                            FD.Add(word);
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, FD);
                }
            };

            DictionaryWords.ItemClick += (s, args) =>
            {
                var word = DictionaryWords.Adapter.GetItem(args.Position);
                DBWords words = new DBWords();
                words = WordDatabase[WordDatabase.FindIndex(obj => obj.Word == word.ToString())];
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(ParentActivity);

                alert.SetTitle("");
                alert.SetMessage($"To delete : {word.ToString()}? ");
                alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
                alert.SetNeutralButton("ок", delegate
                {
                    WordDatabase.Remove(words);
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    DBWords.Delete(word.ToString());
                    Toast.MakeText(ParentActivity, GetString(Resource.String.Word_Delete), ToastLength.Short).Show();

                });
                alert.Show();
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.increase:
                    WordDatabase.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    return true;
                case Resource.Id.decrease:
                    WordDatabase.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    return true;
                case Resource.Id.ABC:
                    WordDatabase.Sort((x, y) => x.Word.CompareTo(y.Word));
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    return true;
                case Resource.Id.HideStudied:
                    HideStudied = !HideStudied;
                    item.SetChecked(HideStudied);
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}