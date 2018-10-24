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
    class SelectDictionary : AppCompatActivity
    {

        private Bitmap CreateSingleImageFromMultipleImages(Bitmap firstImage, Bitmap secondImage)
        {
            Bitmap result = Bitmap.CreateBitmap(firstImage.Width, firstImage.Height, firstImage.GetConfig());
            Canvas canvas = new Canvas(result);
            canvas.DrawBitmap(firstImage, 0f, 0f, null);
            canvas.DrawBitmap(secondImage, 10f, 10f, null);
            return result;
        }

        private void SelectDictionaryClick(object sender, EventArgs e)
        {
            ImageView ImgV = sender as ImageView;
            DataBase.TableNameLanguage = ImgV.Tag.ToString();
            Toast.MakeText(this, Additional_functions.GetResourceString(ImgV.Tag.ToString() + "IsSelected", this.Resources), ToastLength.Short).Show();
            DataBase.UpdateWordsToRepeat();
        }

        public void CreateDictionary(TableNames name, int ImageId)
        {
            int width = (int)Resources.DisplayMetrics.WidthPixels / 3;
            LinearLayout.LayoutParams parmsImage = new LinearLayout.LayoutParams(width, width)
            {
                Gravity = GravityFlags.Center,
                TopMargin = 30,
                BottomMargin = 10
            };

            BitmapFactory.Options t = new BitmapFactory.Options
            {
                OutWidth = 100,
                OutHeight = 100
            };
            Bitmap baseImage = Bitmap.CreateBitmap(width, width, Bitmap.Config.Argb4444);
            Bitmap overlayImage = BitmapFactory.DecodeResource(Resources, ImageId,  t);

            Color background_color = new Color(Color.Argb(150, 16, 19, 38));
            FrameStatistics DegreeOfStudy = new FrameStatistics(0, 0, baseImage.Width, baseImage.Width, background_color);

            Canvas baseCan = new Canvas(baseImage);

            DegreeOfStudy.DrawPieChart(baseCan, 1, 6, Color.Rgb(0, 255, 255), Color.Rgb(50, 60, 126), new PointF(width / 2, width / 2), (float)(width / 3));

            Bitmap finalImage = CreateSingleImageFromMultipleImages(baseImage, overlayImage);

            ImageView DictionaryImage = new ImageView(this)
            {
                LayoutParameters = parmsImage,
                Tag = name.ToString()
            };

            DictionaryImage.SetImageBitmap(finalImage);

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
           
            CreateDictionary(TableNames.Home, Resource.Drawable.homeDictionary);
            CreateDictionary(TableNames.Education, Resource.Drawable.EducationDictionary);
            CreateDictionary(TableNames.Popular_Words, Resource.Drawable.PopularWordsDictionary);
            CreateDictionary(TableNames.My_Directly, Resource.Drawable.MyDictionary);
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