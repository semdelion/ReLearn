using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Settings;
using Android.Support.V7.App;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags_View_Dictionary : AppCompatActivity
    {
        ListView DictionaryImages { get; set; }
        List<DBImages> dataBase = DBImages.GetData;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Languages_ViewDictionary);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguagesDelete);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); 

            DictionaryImages = FindViewById<ListView>(Resource.Id.listView_dictionary);
            SortNamesImages();
            DictionaryImages.Adapter = new CustomAdapter_ImageText(this, dataBase);
        }

        public override bool OnOptionsItemSelected(IMenuItem item) 
        {
            if (item.ItemId == Resource.Id.increase)
            {
                dataBase.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                DictionaryImages.Adapter = new CustomAdapter_ImageText(this, dataBase);
            }
            else if (item.ItemId == Resource.Id.decrease)
            {
                dataBase.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                DictionaryImages.Adapter = new CustomAdapter_ImageText(this, dataBase);
            }
            else if (item.ItemId == Resource.Id.ABC)
            {
                SortNamesImages();
                DictionaryImages.Adapter = new CustomAdapter_ImageText(this, dataBase);
            }
            else if (item.ItemId == Android.Resource.Id.Home)
                Finish();
            return true;
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.search, menu);
            var searchItem = menu.FindItem(Resource.Id.action_search);
            var _searchView = searchItem.ActionView.JavaCast<Android.Support.V7.Widget.SearchView>();
            _searchView.InputType = Convert.ToInt32(Android.Text.InputTypes.TextFlagCapWords);
            _searchView.QueryTextChange += (sender, e) =>
            {
                if (e.NewText == "")
                    DictionaryImages.Adapter = new CustomAdapter_ImageText(this, dataBase);
                else
                {
                    List<DBImages> FD = new List<DBImages>();
                    if (Settings.Currentlanguage == Language.en.ToString())
                        FD = SearchWithGetTypeField("Name_image_en", e);
                    else
                        FD = SearchWithGetTypeField("Name_image_ru", e);

                    DictionaryImages.Adapter = new CustomAdapter_ImageText(this, FD);
                }
            };
            return true;
        }

        public List<DBImages> SearchWithGetTypeField(string nameField, Android.Support.V7.Widget.SearchView.QueryTextChangeEventArgs e)
        {
            List<DBImages> FD = new List<DBImages>();
            foreach (var image in dataBase)
                if (image.GetType().GetProperty(nameField).GetValue(image,null).ToString().Substring(0, ((e.NewText.Length > image.GetType().GetProperty(nameField).GetValue(image, null).ToString().Length) ? 0 : e.NewText.Length)) == e.NewText)
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

   