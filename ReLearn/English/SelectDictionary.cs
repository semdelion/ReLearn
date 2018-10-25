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
        List<Bitmap> DictionariesBitmap { get; set; }
        List<ImageView> DictionariesView { get; set; }
        
        int Width;
        private Bitmap CreateSingleImageFromMultipleImages(Bitmap firstImage, Bitmap secondImage, PointF C)
        {
            Bitmap result = Bitmap.CreateBitmap(firstImage.Width, firstImage.Height, firstImage.GetConfig());
            Canvas canvas = new Canvas(result);
            canvas.DrawBitmap(firstImage, 0f, 0f, null);
            canvas.DrawBitmap(secondImage, C.X, C.Y, null);
            return result;
        }
        private float GetAVG_numberLearn_stat(TableNames name)
        {
            var database = DataBase.Connect(Database_Name.English_DB);
            List<Database_for_stats> Database_NL_and_D = database.Query<Database_for_stats>("SELECT NumberLearn, DateRecurrence FROM " + name.ToString());
            float avg_numberLearn_stat = (float)Database_NL_and_D.Sum(
                r => r.NumberLearn > Magic_constants.StandardNumberOfRepeats ?
                Magic_constants.StandardNumberOfRepeats : r.NumberLearn) / (float)Database_NL_and_D.Count;
            return avg_numberLearn_stat;
        }
        private void Selected(string name)
        {
            for (int i = 0; i < DictionariesView.Count; i++)
                if (DictionariesView[i].Tag.ToString() == DataBase.TableNameLanguage)
                {
                    DictionariesView[i].SetImageBitmap(DictionariesBitmap[i]);
                    break;
                }

            Bitmap Image1 = Bitmap.CreateBitmap(Width, Width, Bitmap.Config.Argb4444);
            Canvas baseCan = new Canvas(Image1);
            Paint paint2 = new Paint { Color = Color.Argb(200,215, 248, 254), AntiAlias = true };
            baseCan.DrawCircle(Width/2, Width/2, Width / 2.5f, paint2);

            for (int i = 0; i < DictionariesView.Count; i++)
                if (DictionariesView[i].Tag.ToString() == name)
                {
                    DictionariesView[i].SetImageBitmap(CreateSingleImageFromMultipleImages(Image1, DictionariesBitmap[i] , new PointF(0,0)));
                    break;
                }
        }
        private void SelectDictionaryClick(object sender, EventArgs e)
        {
            ImageView ImgV = sender as ImageView;
            Selected(ImgV.Tag.ToString());
            DataBase.TableNameLanguage = ImgV.Tag.ToString();
            DataBase.UpdateWordsToRepeat();
        }

        private Bitmap GetBitmap(int ImageId, TableNames name)
        {
            Bitmap Image1 = Bitmap.CreateBitmap(Width, Width, Bitmap.Config.Argb4444);
            FrameStatistics FRAME = new FrameStatistics(0, 0, Image1.Width, Image1.Width, Color.Argb(150, 16, 19, 38));
            float WidthLine = FRAME.Width / 10;
            Bitmap Image2 = Bitmap.CreateScaledBitmap(BitmapFactory.DecodeResource(Resources, ImageId), 
                (int)((Width / 2.5) * 2 - WidthLine), (int)((Width / 2.5) * 2 - WidthLine), false);
            Canvas baseCan = new Canvas(Image1);
            FRAME.DrawPieChart(baseCan, GetAVG_numberLearn_stat(name), Magic_constants.StandardNumberOfRepeats, 
                               Color.Rgb(0, 255, 255), Color.Rgb(50, 60, 126), (float)(baseCan.Height / 2.5), WidthLine);
            Bitmap finalImage = CreateSingleImageFromMultipleImages(Image1, Image2,
                new PointF(((FRAME.Left + FRAME.Width)  / 2) - (float)(baseCan.Height / 2.5) + WidthLine / 2, 
                           ((FRAME.Top  + FRAME.Height) / 2) - (float)(baseCan.Height / 2.5) + WidthLine / 2));
            return finalImage;
        }

        public void CreateViewForDictionary(TableNames name, int ImageId, GravityFlags GF)
        {
            LinearLayout.LayoutParams parmsImage = new LinearLayout.LayoutParams(Width, Width)
                {Gravity = GF, TopMargin = 10};
            DictionariesBitmap.Add(GetBitmap(ImageId, name));

            ImageView DictionaryImage = new ImageView(this)
            {
                LayoutParameters = parmsImage,
                Tag = name.ToString()
            };
            DictionaryImage.SetImageBitmap(DictionariesBitmap[DictionariesBitmap.Count - 1]);
            DictionaryImage.Click += SelectDictionaryClick;
            FindViewById<LinearLayout>(Resource.Id.SelectDictionary).AddView(DictionaryImage);

            TextView DictionaryName = new TextView(this)
            {
                Text = Additional_functions.GetResourceString(name.ToString(), this.Resources),
                Gravity = GF
            };
            DictionaryName.SetTextColor(Color.Rgb(215, 248, 254));
            FindViewById<LinearLayout>(Resource.Id.SelectDictionary).AddView(DictionaryName);
            DictionariesView.Add(DictionaryImage);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectDictionary);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarSelectDictionary);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            DictionariesBitmap = new List<Bitmap>();
            DictionariesView = new List<ImageView>();
            Width = (int)(Resources.DisplayMetrics.WidthPixels / 3f);
            CreateViewForDictionary(TableNames.Home, Resource.Drawable.homeDictionary, GravityFlags.Center);
            CreateViewForDictionary(TableNames.Education, Resource.Drawable.EducationDictionary, GravityFlags.Center);
            CreateViewForDictionary(TableNames.Popular_Words, Resource.Drawable.PopularWordsDictionary, GravityFlags.Center);
            CreateViewForDictionary(TableNames.My_Directly, Resource.Drawable.MyDictionary, GravityFlags.Center);
            Selected(DataBase.TableNameLanguage);
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


