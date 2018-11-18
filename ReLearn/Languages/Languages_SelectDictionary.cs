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
        }
        void CreateViewForDictionary(string NameDictionarn, int ImageId, bool flag, bool separate)
        {
            var width = Resources.DisplayMetrics.WidthPixels/100f;
            var DB = DBStatistics.GetWords(NameDictionarn);
            int count = DB.Count;
            LinearLayout DictionarylinearLayout = new LinearLayout(this){
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent)
            };
            Dictionaries.DictionariesBitmap.Add(Dictionaries.CreateBitmapWithStats(BitmapFactory.DecodeResource(Resources, ImageId), DB));
            ImageView ImageDictionary = new ImageView(this) { Tag = NameDictionarn.ToString() };
            ImageDictionary.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent)
            {
                Gravity = flag ? GravityFlags.Left : GravityFlags.Right
            };
            ImageDictionary.SetPadding((int)(5 * width), 0, (int)(5 * width), 0);
            ImageDictionary.SetImageBitmap(Dictionaries.DictionariesBitmap[Dictionaries.DictionariesBitmap.Count - 1]);
            ImageDictionary.Click += SelectDictionaryClick;

            TextView Name = new TextView(this)
            {
                Text = AdditionalFunctions.GetResourceString(NameDictionarn, this.Resources),
                TextSize = 20//(int)(3 * width)
            };
            TextView CountWords = new TextView(this)
            {
                Text = $"{GetString(Resource.String.DatatypeWords)} {count}",
                TextSize = 14//(int)(2.1f * width)
            };
            TextView Description = new TextView(this)
            {
                Text = AdditionalFunctions.GetResourceString($"{NameDictionarn}Description", this.Resources),
                TextSize = 11//(int)(1.7f * width)
            };

            LinearLayout TextlinearLayout = new LinearLayout(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent),
                Orientation = Android.Widget.Orientation.Vertical
            };

            Name.LayoutParameters = CountWords.LayoutParameters = Description.LayoutParameters = 
                new LinearLayout.LayoutParams(52 * Resources.DisplayMetrics.WidthPixels / 100, LinearLayout.LayoutParams.WrapContent);
            Name.SetTextColor(Color.Rgb(215, 248, 254));
            CountWords.SetTextColor(Color.Rgb(121, 150, 155));
            Description.SetTextColor(Color.Rgb(121, 150, 155));
            TextlinearLayout.AddView(Name);
            TextlinearLayout.AddView(CountWords);
            TextlinearLayout.AddView(Description);
            TextlinearLayout.SetPadding(flag? 0: (int)(5 * width), 0, !flag ? 0 : (int)(5 * width), 0);

            DictionarylinearLayout.AddView(flag == true ? (View)ImageDictionary : TextlinearLayout);
            DictionarylinearLayout.AddView(flag == false ? (View)ImageDictionary : TextlinearLayout);
            
            FindViewById<LinearLayout>(Resource.Id.LanguageSelectDictionary).AddView(DictionarylinearLayout);
            if (separate)
            {
                View SeparateView = new View(this)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, (int)(width / 2f))
                    {
                        TopMargin = (int)(2*width),
                        BottomMargin = (int)(2 * width)
                    },
                    Background = GetDrawable(Resource.Drawable.separator),
                };
                FindViewById<LinearLayout>(Resource.Id.LanguageSelectDictionary).AddView(SeparateView);
            }
            Dictionaries.DictionariesView.Add(ImageDictionary);
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
            CreateViewForDictionary(TableNamesLanguage.Home.ToString(), Resource.Drawable.homeDictionary, true , true);
            CreateViewForDictionary(TableNamesLanguage.Education.ToString(), Resource.Drawable.EducationDictionary, false, true);
            CreateViewForDictionary(TableNamesLanguage.Popular_Words.ToString(), Resource.Drawable.PopularWordsDictionary, true, true);
            CreateViewForDictionary(TableNamesLanguage.ThreeFormsOfVerb.ToString(), Resource.Drawable.ThreeFormsOfVerbDictionary, false, true);
            CreateViewForDictionary(TableNamesLanguage.ComputerScience.ToString(), Resource.Drawable.ComputerScience, true, true);
            CreateViewForDictionary(TableNamesLanguage.Nature.ToString(), Resource.Drawable.Nature, false, true);
            CreateViewForDictionary(TableNamesLanguage.My_Directly.ToString(), Resource.Drawable.MyDictionary, true, false);
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


