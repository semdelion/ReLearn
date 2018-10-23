using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
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
    class SelectDictionary : AppCompatActivity
    {
        private void SelectDictionaryClick(object sender, EventArgs e)
        {
            ImageView ImgV = sender as ImageView;
            DataBase.TableNameLanguage = ImgV.Tag.ToString();
            Toast.MakeText(this, Additional_functions.GetResourceString(ImgV.Tag.ToString() + "IsSelected", this.Resources), ToastLength.Short).Show();
            DataBase.UpdateWordsToRepeat();
        }

        public void CreateDictionary(TableNames name, Drawable originalImage)
        {
            int width = (int)Resources.DisplayMetrics.WidthPixels / 4;
            LinearLayout.LayoutParams parmsImage = new LinearLayout.LayoutParams(width, width);
            parmsImage.Gravity = GravityFlags.Center;
            parmsImage.TopMargin = 30;
            parmsImage.BottomMargin = 10;
            ImageView DictionaryImage = new ImageView(this)
            {
                LayoutParameters = parmsImage,
                Tag = name.ToString()
            };
            DictionaryImage.SetImageDrawable(originalImage);
            DictionaryImage.Click += SelectDictionaryClick;
            FindViewById<LinearLayout>(Resource.Id.SelectDictionary).AddView(DictionaryImage);

            TextView DictionaryName = new TextView(this)
            {
                Text = Additional_functions.GetResourceString(name.ToString(), this.Resources),
                Gravity = GravityFlags.Center
            };
            DictionaryName.SetTextColor(Color.Rgb(215, 248, 254));

            FindViewById<LinearLayout>(Resource.Id.SelectDictionary).AddView(DictionaryName);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectDictionary);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarSelectDictionary);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            CreateDictionary(TableNames.Home, GetDrawable(Resource.Drawable.homeDictionary));
            CreateDictionary(TableNames.Education, GetDrawable(Resource.Drawable.EducationDictionary));
            CreateDictionary(TableNames.Popular_Words, GetDrawable(Resource.Drawable.PopularWordsDictionary));
            CreateDictionary(TableNames.My_Directly, GetDrawable(Resource.Drawable.MyDictionary));
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                this.Finish();
                return true;
            }
            return true;
        }
        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}


//void SetSelected() // TODO to new layout just crutch.
//{
//    if (DataBase.TableNameLanguage == TableNames.My_Directly.ToString())
//        selected = Resource.Id.menuDatabase_MyDictionary;
//    else if (DataBase.TableNameLanguage == TableNames.Home.ToString())
//        selected = Resource.Id.menuDatabase_Home;
//    else if (DataBase.TableNameLanguage == TableNames.Education.ToString())
//        selected = Resource.Id.menuDatabase_Education;
//    else if (DataBase.TableNameLanguage == TableNames.Popular_Words.ToString())
//        selected = Resource.Id.menuDatabase_PopularWords;
//}

//public override bool OnPrepareOptionsMenu(IMenu menu)
//{
//    MenuInflater.Inflate(Resource.Menu.menu_english, menu);
//    if (selected == Resource.Id.menuDatabase_MyDictionary)
//    {
//        menu.FindItem(Resource.Id.menuDatabase_MyDictionary).SetChecked(true);
//        return true;
//    }
//    if (selected == Resource.Id.menuDatabase_Home)
//    {
//        menu.FindItem(Resource.Id.menuDatabase_Home).SetChecked(true);
//        return true;
//    }
//    if (selected == Resource.Id.menuDatabase_Education)
//    {
//        menu.FindItem(Resource.Id.menuDatabase_Education).SetChecked(true);
//        return true;
//    }
//    if (selected == Resource.Id.menuDatabase_PopularWords)
//    {
//        menu.FindItem(Resource.Id.menuDatabase_PopularWords).SetChecked(true);
//        return true;
//    }
//    return true;
//}