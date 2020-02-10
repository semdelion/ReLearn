using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.Languages;
using ReLearn.Droid.Adapters;
using ReLearn.Droid.Views.Base;
using SearchView = Android.Support.V7.Widget.SearchView;

namespace ReLearn.Droid.Views.Languages
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("relearn.droid.views.languages.ViewDictionaryFragment")]
    public class ViewDictionaryFragment : MvxBaseFragment<ViewDictionaryViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_menu_view_dictionary;

        protected override int Toolbar => Resource.Id.toolbar_view_dictionary;

        private ListView DictionaryWords { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            DictionaryWords = view.FindViewById<ListView>(Resource.Id.listView_dictionary);
            ViewModel.Database.Sort((x, y) => x.Word.CompareTo(y.Word));
            DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity,
                ViewModel.HideStudied ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0) : ViewModel.Database);
            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search, menu);
            var _searchView = menu.FindItem(Resource.Id.action_search).ActionView.JavaCast<SearchView>();
            menu.FindItem(Resource.Id.HideStudied).SetChecked(ViewModel.HideStudied);

            menu.FindItem(Resource.Id.ABC).SetTitle(ViewModel.TextSortAlphabetically);
            menu.FindItem(Resource.Id.HideStudied).SetTitle(ViewModel.TextHideStudied);
            menu.FindItem(Resource.Id.increase).SetTitle(ViewModel.TextSortAscending);
            menu.FindItem(Resource.Id.decrease).SetTitle(ViewModel.TextSortDescending);

            _searchView.QueryTextChange += (sender, e) =>
            {
                var searchWord = e.NewText.ToLower().Trim();
                if (searchWord == "")
                {
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity,
                        ViewModel.HideStudied
                            ? ViewModel.Database.FindAll(column => column.NumberLearn != 0)
                            : ViewModel.Database);
                }
                else
                {
                    var date = ViewModel.Database.FindAll(column =>
                        column.Word.ToLower().Contains(searchWord) ||
                        column.TranslationWord.ToLower().Contains(searchWord));

                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, date);
                }
            };

            DictionaryWords.ItemClick += (s, args) =>
            {
                var word = DictionaryWords.Adapter.GetItem(args.Position);
                var words = new DatabaseWords();
                words = ViewModel.Database[ViewModel.Database.FindIndex(obj => obj.Word == $"{word}")];
                var alert = new AlertDialog.Builder(ParentActivity);

                alert.SetTitle("");
                alert.SetMessage($"{ViewModel.TextDelete} {$"{word}"}?");
                alert.SetPositiveButton(ViewModel.TextCancel, delegate { alert.Dispose(); });
                alert.SetNeutralButton(ViewModel.TextOk, delegate
                {
                    ViewModel.Database.Remove(words);
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity,
                        ViewModel.HideStudied
                            ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0)
                            : ViewModel.Database);
                    DatabaseWords.Delete($"{word}").ConfigureAwait(false);
                    Toast.MakeText(ParentActivity, ViewModel.ErrorWordDelete, ToastLength.Short).Show();
                });
                alert.Show();
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.increase:
                    ViewModel.Database.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity,
                        ViewModel.HideStudied
                            ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0)
                            : ViewModel.Database);
                    return true;
                case Resource.Id.decrease:
                    ViewModel.Database.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity,
                        ViewModel.HideStudied
                            ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0)
                            : ViewModel.Database);
                    return true;
                case Resource.Id.ABC:
                    ViewModel.Database.Sort((x, y) => x.Word.CompareTo(y.Word));
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity,
                        ViewModel.HideStudied
                            ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0)
                            : ViewModel.Database);
                    return true;
                case Resource.Id.HideStudied:
                    ViewModel.HideStudied = !ViewModel.HideStudied;
                    item.SetChecked(ViewModel.HideStudied);
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity,
                        ViewModel.HideStudied
                            ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0)
                            : ViewModel.Database);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}