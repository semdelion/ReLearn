using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Calligraphy;


namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Languages_SelectDictionary : AppCompatActivity
    {
        SelectDictionary Dictionaries { get; set; }

        private void SelectDictionaryClick(object sender, EventArgs e)
        {
            ImageView ImgV = sender as ImageView;
            Dictionaries.Selected(ImgV.Tag.ToString(), DataBase.TableNameLanguage.ToString());
            Enum.TryParse(ImgV.Tag.ToString(), out TableNamesLanguage name);
            DataBase.TableNameLanguage = name;
            DataBase.UpdateWordsToRepeat();
        }

        public void CreateViewForDictionary(string name, int ImageId)
        {
            var database = DataBase.Connect(Database_Name.English_DB);
            List<DBStatistics> Database_NL_and_D = database.Query<DBStatistics>($"SELECT NumberLearn, DateRecurrence FROM {name}");
            Dictionaries.DictionariesBitmap.Add(Dictionaries.CreateBitmapWithStats(BitmapFactory.DecodeResource(Resources, ImageId), Database_NL_and_D));

            ImageView DictionaryImage = new ImageView(this)
            {
                LayoutParameters = Dictionaries.ParmsImage,
                Tag = name.ToString()
            };
            DictionaryImage.SetImageBitmap(Dictionaries.DictionariesBitmap[Dictionaries.DictionariesBitmap.Count - 1]);
            DictionaryImage.Click += SelectDictionaryClick;
            FindViewById<LinearLayout>(Resource.Id.Languages_SelectDictionary).AddView(DictionaryImage);

            TextView DictionaryName = new TextView(this)
            {
                Text = AdditionalFunctions.GetResourceString(name.ToString(), this.Resources),
                Gravity = Dictionaries.ParmsImage.Gravity
            };
            DictionaryName.SetTextColor(Color.Rgb(215, 248, 254));
            FindViewById<LinearLayout>(Resource.Id.Languages_SelectDictionary).AddView(DictionaryName);
            Dictionaries.DictionariesView.Add(DictionaryImage);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Languages_SelectDictionary);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguages_SelectDictionary);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            Dictionaries = new SelectDictionary((int)(Resources.DisplayMetrics.WidthPixels / 3f));
   
            CreateViewForDictionary(TableNamesLanguage.Home.ToString(), Resource.Drawable.homeDictionary);
            CreateViewForDictionary(TableNamesLanguage.Education.ToString(), Resource.Drawable.EducationDictionary);
            CreateViewForDictionary(TableNamesLanguage.Popular_Words.ToString(), Resource.Drawable.PopularWordsDictionary);
            CreateViewForDictionary(TableNamesLanguage.ThreeFormsOfVerb.ToString(), Resource.Drawable.ThreeFormsOfVerbDictionary);
            CreateViewForDictionary(TableNamesLanguage.My_Directly.ToString(), Resource.Drawable.MyDictionary);
            Dictionaries.Selected(DataBase.TableNameLanguage.ToString(), DataBase.TableNameLanguage.ToString());
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();
            return true;
        }
        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}


