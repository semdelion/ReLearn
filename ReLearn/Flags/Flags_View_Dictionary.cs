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

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags_View_Dictionary : Activity
    {
        ListView listView_dictionary;
        List<Database_images> dataBase;
        CustomAdapter_ImageText Adapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_View_Dictionary);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglishDelete);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            listView_dictionary = FindViewById<ListView>(Resource.Id.listView_dictionary);

            var db = DataBase.Connect(Database_Name.Flags_DB);
            dataBase = db.Query<Database_images>("SELECT * FROM " + DataBase.Table_Name);
            SortNamesImages();

            Adapter = new CustomAdapter_ImageText(this, dataBase);
            listView_dictionary.Adapter = Adapter;                 
        }

        public override bool OnOptionsItemSelected(IMenuItem item) // button home
        {
            int id = item.ItemId;
            if (id == Resource.Id.increase)
            {
                dataBase.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                Adapter = new CustomAdapter_ImageText(this, dataBase);
                listView_dictionary.Adapter = Adapter;
            }
            if (id == Resource.Id.decrease)
            {
                dataBase.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                Adapter = new CustomAdapter_ImageText(this, dataBase);
                listView_dictionary.Adapter = Adapter;
            }
            if (id == Resource.Id.ABC)
            {
                SortNamesImages();
                Adapter = new CustomAdapter_ImageText(this, dataBase);
                listView_dictionary.Adapter = Adapter;
            }
            if (id == Android.Resource.Id.Home)
            {
                this.Finish();
            }
            return true;
        }       

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.search, menu);
            var searchItem = menu.FindItem(Resource.Id.action_search);   
            
            SearchView _searchView = searchItem.ActionView.JavaCast<Android.Widget.SearchView>();

            _searchView.SetInputType(Android.Text.InputTypes.TextFlagCapWords);////?

            _searchView.QueryTextChange += (sender, e) =>
            {
                if (e.NewText == "")
                    listView_dictionary.Adapter = new CustomAdapter_ImageText(this, dataBase);
                else
                {
                    List<Database_images> FD = new List<Database_images>();
                    if (CrossSettings.Current.GetValueOrDefault("Language", null) == "en")
                        FD = SearchWithGetTypeField("Name_image_en", e);                    
                    else                    
                        FD = SearchWithGetTypeField("Name_image_ru", e);

                    var ad = new CustomAdapter_ImageText(this, FD);
                    listView_dictionary.Adapter = ad;                   
                }
            };           
            return base.OnCreateOptionsMenu(menu);
        }

        public List<Database_images> SearchWithGetTypeField(string nameField, SearchView.QueryTextChangeEventArgs e)
        {
            List<Database_images> FD = new List<Database_images>();
            foreach (var image in dataBase)
                if (image.GetType().GetProperty(nameField).GetValue(image,null).ToString().Substring(0, ((e.NewText.Length > image.GetType().GetProperty(nameField).GetValue(image, null).ToString().Length) ? 0 : e.NewText.Length)) == e.NewText)
                    FD.Add(image);    
            return FD;
        }

        public void SortNamesImages()
        {
            if (CrossSettings.Current.GetValueOrDefault("Language", null) == "en")
                dataBase.Sort((x, y) => x.Name_image_en.CompareTo(y.Name_image_en));
            else
                dataBase.Sort((x, y) => x.Name_image_ru.CompareTo(y.Name_image_ru));
        }

    }
}

   