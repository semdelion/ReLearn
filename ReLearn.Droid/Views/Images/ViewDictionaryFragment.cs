using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Plugin.Settings;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.Images;
using ReLearn.Droid.Views.Menu;
using System;
using System.Collections.Generic;

namespace ReLearn.Droid.Views.Images
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    [Register("relearn.droid.views.images.ViewDictionaryFragment")]
    public class ViewDictionaryFragment : BaseFragment<ViewDictionaryViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_menu_view_dictionary;

        protected override int Toolbar => Resource.Id.toolbar_view_dictionary;
        ListView DictionaryImages { get; set; }
        
        public static bool HideStudied
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.HideStudied}", true);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.HideStudied}", value);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            DictionaryImages = view.FindViewById<ListView>(Resource.Id.listView_dictionary);
            SortNamesImages();
            DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, HideStudied ? ViewModel.DataBase.FindAll(obj => obj.NumberLearn != 0) : ViewModel.DataBase);
            return view;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.increase:
                    ViewModel.DataBase.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, HideStudied ? ViewModel.DataBase.FindAll(obj => obj.NumberLearn != 0) : ViewModel.DataBase);
                    return true;
                case Resource.Id.decrease:
                    ViewModel.DataBase.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, HideStudied ? ViewModel.DataBase.FindAll(obj => obj.NumberLearn != 0) : ViewModel.DataBase);
                    return true;
                case Resource.Id.ABC:
                    SortNamesImages();
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, HideStudied ? ViewModel.DataBase.FindAll(obj => obj.NumberLearn != 0) : ViewModel.DataBase);
                    return true;
                case Resource.Id.HideStudied:
                    HideStudied = !HideStudied;
                    item.SetChecked(HideStudied);
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, HideStudied ? ViewModel.DataBase.FindAll(obj => obj.NumberLearn != 0) : ViewModel.DataBase);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search, menu);
            var searchItem = menu.FindItem(Resource.Id.action_search);
            var _searchView = searchItem.ActionView.JavaCast<Android.Support.V7.Widget.SearchView>();
            _searchView.InputType = Convert.ToInt32(Android.Text.InputTypes.TextFlagCapWords);
            menu.FindItem(Resource.Id.HideStudied).SetChecked(HideStudied);
            _searchView.QueryTextChange += (sender, e) =>
            {
                if (e.NewText == "")
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, HideStudied ? ViewModel.DataBase.FindAll(obj => obj.NumberLearn != 0) : ViewModel.DataBase);
                else
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, Settings.Currentlanguage == Language.en.ToString() ?
                         SearchWithGetTypeField("Name_image_en", e) :
                         SearchWithGetTypeField("Name_image_ru", e));
            };
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public List<DBImages> SearchWithGetTypeField(string nameField, Android.Support.V7.Widget.SearchView.QueryTextChangeEventArgs e)
        {
            List<DBImages> FD = new List<DBImages>();
            foreach (var image in ViewModel.DataBase)
                if (image.GetType().GetProperty(nameField).GetValue(image, null).ToString().Substring(0, ((e.NewText.Length > image.GetType().GetProperty(nameField).GetValue(image, null).ToString().Length) ? 0 : e.NewText.Length)) == e.NewText)
                    FD.Add(image);
            return FD;
        }

        public void SortNamesImages()
        {
            if (Settings.Currentlanguage == Language.en.ToString())
                ViewModel.DataBase.Sort((x, y) => x.Name_image_en.CompareTo(y.Name_image_en));
            else
                ViewModel.DataBase.Sort((x, y) => x.Name_image_ru.CompareTo(y.Name_image_ru));
        }
    }
}