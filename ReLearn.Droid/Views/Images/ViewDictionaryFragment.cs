using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Plugin.Settings;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.Images;
using ReLearn.Droid.Services;
using ReLearn.Droid.Views.Menu;
using System;
using System.Collections.Generic;

namespace ReLearn.Droid.Views.Images
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("relearn.droid.views.images.ViewDictionaryFragment")]
    public class ViewDictionaryFragment : BaseFragment<ViewDictionaryViewModel>
    {
        protected override int FragmentId => Resource.Layout.languages_view_dictionary_activity;

        protected override int Toolbar => Resource.Id.toolbarLanguagesDelete;
        ListView DictionaryImages { get; set; }
        List<DBImages> dataBase = DBImages.GetData;
        public static bool HideStudied
        {
            get => CrossSettings.Current.GetValueOrDefault(DBSettings.HideStudied.ToString(), true);
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.HideStudied.ToString(), value);
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(FragmentId, null);

            _toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Toolbar);
            if (_toolbar != null)
            {
                ParentActivity.SetSupportActionBar(_toolbar);
                ParentActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);

                _drawerToggle = new MvxActionBarDrawerToggle(
                    Activity,                               // host Activity
                    (ParentActivity as INavigationActivity).DrawerLayout,  // DrawerLayout object
                    _toolbar,                               // nav drawer icon to replace 'Up' caret
                    Resource.String.navigation_drawer_open,
                    Resource.String.navigation_drawer_close
                );
                _drawerToggle.DrawerOpened += (object sender, ActionBarDrawerEventArgs e) => (Activity as MainActivity)?.HideSoftKeyboard();
                (ParentActivity as INavigationActivity).DrawerLayout.AddDrawerListener(_drawerToggle);
            }
            DictionaryImages = view.FindViewById<ListView>(Resource.Id.listView_dictionary);
            SortNamesImages();
            DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, HideStudied ? dataBase.FindAll(obj => obj.NumberLearn != 0) : dataBase);
            return view;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.increase:
                    dataBase.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, HideStudied ? dataBase.FindAll(obj => obj.NumberLearn != 0) : dataBase);
                    return true;
                case Resource.Id.decrease:
                    dataBase.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, HideStudied ? dataBase.FindAll(obj => obj.NumberLearn != 0) : dataBase);
                    return true;
                case Resource.Id.ABC:
                    SortNamesImages();
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, HideStudied ? dataBase.FindAll(obj => obj.NumberLearn != 0) : dataBase);
                    return true;
                case Resource.Id.HideStudied:
                    HideStudied = !HideStudied;
                    item.SetChecked(HideStudied);
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, HideStudied ? dataBase.FindAll(obj => obj.NumberLearn != 0) : dataBase);
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
                    DictionaryImages.Adapter = new CustomAdapterImage(ParentActivity, HideStudied ? dataBase.FindAll(obj => obj.NumberLearn != 0) : dataBase);
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
            foreach (var image in dataBase)
                if (image.GetType().GetProperty(nameField).GetValue(image, null).ToString().Substring(0, ((e.NewText.Length > image.GetType().GetProperty(nameField).GetValue(image, null).ToString().Length) ? 0 : e.NewText.Length)) == e.NewText)
                    FD.Add(image);
            return FD;
        }

        public void SortNamesImages()
        {
            if (Settings.Currentlanguage == Language.en.ToString())
                dataBase.Sort((x, y) => x.Name_image_en.CompareTo(y.Name_image_en));
            else
                dataBase.Sort((x, y) => x.Name_image_ru.CompareTo(y.Name_image_ru));
        }
    }
}