using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
                    DictionaryImages.Adapter = new CustomAdapter_ImageText(this, Settings.Currentlanguage == Language.en.ToString() ?
                         SearchWithGetTypeField("Name_image_en", e) :
                         SearchWithGetTypeField("Name_image_ru", e));
            };

            //DictionaryImages.ItemClick += (s, args) =>
            //{
            //    var image = DictionaryImages.Adapter.GetItem(args.Position);
            //    DBImages images = new DBImages();
            //    foreach (var item in dataBase)
            //        if (item.Image_name == image.ToString())
            //        {
            //            images = item.Find();
            //            break;
            //        }

            //    View view = LayoutInflater.From(this).Inflate(Resource.Layout.item_alert_big_image, null);
            //    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            //    alert.SetView(view);

            //    var ImageView = view.FindViewById<ImageView>(Resource.Id.imageView_Alert_BigImage);
            //    using (var his = Application.Context.Assets.Open($"Image{DataBase.TableNameImage}/{images.Image_name}.png"))
            //    {
            //        Bitmap bitmap = BitmapFactory.DecodeStream(his);
            //        double imageWidthInPX = (double)ImageView.Width;
            //        ImageView.SetImageBitmap(bitmap);
            //    }
    
            //    alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
            //    alert.Show();
            //};

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

   