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
        ListView listView_dictionary;
        List<Database_images> dataBase;
        CustomAdapter_ImageText Adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Languages_ViewDictionary);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguagesDelete);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            listView_dictionary = FindViewById<ListView>(Resource.Id.listView_dictionary);

            var db = DataBase.Connect(Database_Name.Flags_DB);
            dataBase = db.Query<Database_images>($"SELECT * FROM {DataBase.TableNameImage}");
            SortNamesImages();

            Adapter = new CustomAdapter_ImageText(this, dataBase);
            listView_dictionary.Adapter = Adapter;                 
        }

        public override bool OnOptionsItemSelected(IMenuItem item) // button home
        {
            if (item.ItemId == Resource.Id.increase)
            {
                dataBase.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                Adapter = new CustomAdapter_ImageText(this, dataBase);
                listView_dictionary.Adapter = Adapter;
            }
            else if (item.ItemId == Resource.Id.decrease)
            {
                dataBase.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                Adapter = new CustomAdapter_ImageText(this, dataBase);
                listView_dictionary.Adapter = Adapter;
            }
            else if (item.ItemId == Resource.Id.ABC)
            {
                SortNamesImages();
                Adapter = new CustomAdapter_ImageText(this, dataBase);
                listView_dictionary.Adapter = Adapter;
            }
            else if (item.ItemId == Android.Resource.Id.Home)
                this.Finish();
            
            return true;
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.search, menu);
            var searchItem = menu.FindItem(Resource.Id.action_search);

            var _searchView = searchItem.ActionView.JavaCast<Android.Support.V7.Widget.SearchView>();
            _searchView.InputType =Convert.ToInt32(Android.Text.InputTypes.TextFlagCapWords);
            _searchView.QueryTextChange += (sender, e) =>
            {
                if (e.NewText == "")
                    listView_dictionary.Adapter = new CustomAdapter_ImageText(this, dataBase);
                else
                {
                    List<Database_images> FD = new List<Database_images>();
                    if (CrossSettings.Current.GetValueOrDefault(Settings.Language.ToString(), null) == Language.en.ToString())
                        FD = SearchWithGetTypeField("Name_image_en", e);
                    else
                        FD = SearchWithGetTypeField("Name_image_ru", e);

                    var ad = new CustomAdapter_ImageText(this, FD);
                    listView_dictionary.Adapter = ad;
                }
            };
            return true;
        }

        public List<Database_images> SearchWithGetTypeField(string nameField, Android.Support.V7.Widget.SearchView.QueryTextChangeEventArgs e)
        {
            List<Database_images> FD = new List<Database_images>();
            foreach (var image in dataBase)
                if (image.GetType().GetProperty(nameField).GetValue(image,null).ToString().Substring(0, ((e.NewText.Length > image.GetType().GetProperty(nameField).GetValue(image, null).ToString().Length) ? 0 : e.NewText.Length)) == e.NewText)
                    FD.Add(image);    
            return FD;
        }

        public void SortNamesImages()
        {
            if (CrossSettings.Current.GetValueOrDefault(Settings.Language.ToString(), null) == Language.en.ToString())
                dataBase.Sort((x, y) => x.Name_image_en.CompareTo(y.Name_image_en));
            else
                dataBase.Sort((x, y) => x.Name_image_ru.CompareTo(y.Name_image_ru));
        }

    }
}

   