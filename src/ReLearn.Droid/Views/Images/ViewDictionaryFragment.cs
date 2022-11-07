﻿using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.API;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.Images;
using ReLearn.Droid.Adapters;
using ReLearn.Droid.Views.Base;
using System;

namespace ReLearn.Droid.Views.Images
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("relearn.droid.views.images.ViewDictionaryFragment")]
    public class ViewDictionaryFragment : MvxBaseFragment<ViewDictionaryViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_menu_view_dictionary;

        protected override int Toolbar => Resource.Id.toolbar_view_dictionary;
        private ListView DictionaryImages { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            DictionaryImages = view.FindViewById<ListView>(Resource.Id.listView_dictionary);
            SortNamesImages();
            DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity,
                ViewModel.HideStudied ? ViewModel.DataBase.FindAll(obj => obj.NumberLearn != 0) : ViewModel.DataBase);
            return view;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.increase:
                    ViewModel.DataBase.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity,
                        ViewModel.HideStudied
                            ? ViewModel.DataBase.FindAll(obj => obj.NumberLearn != 0)
                            : ViewModel.DataBase);
                    return true;
                case Resource.Id.decrease:
                    ViewModel.DataBase.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity,
                        ViewModel.HideStudied
                            ? ViewModel.DataBase.FindAll(obj => obj.NumberLearn != 0)
                            : ViewModel.DataBase);
                    return true;
                case Resource.Id.ABC:
                    SortNamesImages();
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity,
                        ViewModel.HideStudied
                            ? ViewModel.DataBase.FindAll(obj => obj.NumberLearn != 0)
                            : ViewModel.DataBase);
                    return true;
                case Resource.Id.HideStudied:
                    ViewModel.HideStudied = !ViewModel.HideStudied;
                    item.SetChecked(ViewModel.HideStudied);
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity,
                        ViewModel.HideStudied
                            ? ViewModel.DataBase.FindAll(obj => obj.NumberLearn != 0)
                            : ViewModel.DataBase);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search, menu);
            var searchItem = menu.FindItem(Resource.Id.action_search);
            var searchView = searchItem.ActionView.JavaCast<AndroidX.AppCompat.Widget.SearchView>();
            searchView.InputType = Convert.ToInt32(InputTypes.TextFlagCapWords);
            menu.FindItem(Resource.Id.HideStudied).SetChecked(ViewModel.HideStudied);
            menu.FindItem(Resource.Id.ABC).SetTitle(ViewModel.TextSortAlphabetically);
            menu.FindItem(Resource.Id.HideStudied).SetTitle(ViewModel.TextHideStudied);
            menu.FindItem(Resource.Id.increase).SetTitle(ViewModel.TextSortAscending);
            menu.FindItem(Resource.Id.decrease).SetTitle(ViewModel.TextSortDescending);
            searchView.QueryTextChange += (sender, e) =>
            {
                var searchWord = e.NewText.ToLower().Trim();
                if (searchWord == string.Empty)
                {
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity,
                        ViewModel.HideStudied
                            ? ViewModel.DataBase.FindAll(column => column.NumberLearn != 0)
                            : ViewModel.DataBase);
                }
                else
                {
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity,
                        Settings.Currentlanguage == $"{Language.en}" ?
                        ViewModel.DataBase.FindAll(column => column.Name_image_en.ToLower().Contains(searchWord)) :
                        ViewModel.DataBase.FindAll(column => column.Name_image_ru.ToLower().Contains(searchWord)));
                }
            };
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public void SortNamesImages()
        {
            if (Settings.Currentlanguage == $"{Language.en}")
            {
                ViewModel.DataBase.Sort((x, y) => x.Name_image_en.CompareTo(y.Name_image_en));
            }
            else
            {
                ViewModel.DataBase.Sort((x, y) => x.Name_image_ru.CompareTo(y.Name_image_ru));
            }
        }
    }
}