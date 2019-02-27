﻿using Android.OS;
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
        
        public static bool HideStudied
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.HideStudied}", true);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.HideStudied}", value);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            DictionaryWords = view.FindViewById<ListView>(Resource.Id.listView_dictionary);
            ViewModel.Database.Sort((x, y) => x.Word.CompareTo(y.Word));
            DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0) : ViewModel.Database);
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
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0) : ViewModel.Database);
                else
                {
                    List<DBWords> FD = new List<DBWords>();
                    foreach (var word in ViewModel.Database)
                        if (word.Word.Substring(0, ((e.NewText.Length > word.Word.Length) ? 0 : e.NewText.Length)) == e.NewText)
                            FD.Add(word);
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, FD);
                }
            };

            DictionaryWords.ItemClick += (s, args) =>
            {
                var word = DictionaryWords.Adapter.GetItem(args.Position);
                DBWords words = new DBWords();
                words = ViewModel.Database[ViewModel.Database.FindIndex(obj => obj.Word == $"{word}")];
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(ParentActivity);

                alert.SetTitle("");
                alert.SetMessage($"To delete : {word.ToString()}? ");
                alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
                alert.SetNeutralButton("ок", delegate
                {
                    ViewModel.Database.Remove(words);
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0) : ViewModel.Database);
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
                    ViewModel.Database.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0) : ViewModel.Database);
                    return true;
                case Resource.Id.decrease:
                    ViewModel.Database.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0) : ViewModel.Database);
                    return true;
                case Resource.Id.ABC:
                    ViewModel.Database.Sort((x, y) => x.Word.CompareTo(y.Word));
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0) : ViewModel.Database);
                    return true;
                case Resource.Id.HideStudied:
                    HideStudied = !HideStudied;
                    item.SetChecked(HideStudied);
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? ViewModel.Database.FindAll(obj => obj.NumberLearn != 0) : ViewModel.Database);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}