using Android.Graphics;
using Android.OS;
using Android.Support.Animation;
using Android.Views;
using Android.Widget;
using ReLearn.API.Database;
using System;
using System.Collections.Generic;
using ReLearn.API;

namespace ReLearn.Droid.Views.SelectDictionary
{
    class TabImageFragment : Android.Support.V4.App.Fragment
    {
        public View CreateViewForDictionary(View view, List<DBStatistics> DB, string NameDictionary, int ImageId, bool flag, bool separate, Color lightColor, Color darkColor)
        {
            var width = Resources.DisplayMetrics.WidthPixels / 100f;
            int count = DB.Count;
            LinearLayout DictionarylinearLayout = new LinearLayout(view.Context)
            {
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent)
            };
            SelectDictionary.Dictionaries.DictionariesBitmap.Add(SelectDictionary.Dictionaries.CreateBitmapWithStats(BitmapFactory.DecodeResource(Resources, ImageId), DB, lightColor, darkColor));//////fail color
            ImageView ImageDictionary = new ImageView(view.Context) { Tag = NameDictionary.ToString() };
            ImageDictionary.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent)
            {
                Gravity = flag ? GravityFlags.Left : GravityFlags.Right
            };
            ImageDictionary.SetPadding((int)(5 * width), 0, (int)(5 * width), 0);
            ImageDictionary.SetImageBitmap(SelectDictionary.Dictionaries.DictionariesBitmap[SelectDictionary.Dictionaries.DictionariesBitmap.Count - 1]);
            ImageDictionary.Click += SelectDictionary.SelectDictionaryClick;

            TextView Name = new TextView(view.Context)
            {
                Text = AdditionalFunctions.GetResourceString(NameDictionary, this.Resources),
                TextSize = 20//(int)(3 * width)
            };
            TextView CountWords = new TextView(view.Context)
            {
                Text = $"{GetString(Resource.String.DatatypeImages)} {count}, {GetString(Resource.String.StudiedAt)} " +
                $"{(int)(100 - ReLearn.Droid.Statistics.GetAverageNumberLearn(DB) * 100f / Settings.StandardNumberOfRepeats)}%",
                TextSize = 14//(int)(2.1f * width)
            };
            TextView Description = new TextView(view.Context)
            {
                Text = AdditionalFunctions.GetResourceString($"{NameDictionary}Description", this.Resources),
                TextSize = 11//(int)(1.7f * width)
            };
            LinearLayout TextlinearLayout = new LinearLayout(view.Context)
            {
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent),
                Orientation = Android.Widget.Orientation.Vertical
            };

            Name.LayoutParameters = CountWords.LayoutParameters = Description.LayoutParameters =
                new LinearLayout.LayoutParams(52 * Resources.DisplayMetrics.WidthPixels / 100, LinearLayout.LayoutParams.WrapContent);
            Name.SetTextColor(Colors.White);
            CountWords.SetTextColor(Colors.HintWhite);
            Description.SetTextColor(Colors.HintWhite);
            TextlinearLayout.AddView(Name);
            TextlinearLayout.AddView(CountWords);
            TextlinearLayout.AddView(Description);
            TextlinearLayout.SetPadding(flag ? 0 : (int)(5 * width), 0, !flag ? 0 : (int)(5 * width), 0);

            DictionarylinearLayout.AddView(flag == true ? (View)ImageDictionary : TextlinearLayout);
            DictionarylinearLayout.AddView(flag == false ? (View)ImageDictionary : TextlinearLayout);

            view.FindViewById<LinearLayout>(Resource.Id.ImageSelectDictionary).AddView(DictionarylinearLayout);
            if (separate)
            {
                View SeparateView = new View(view.Context)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, (int)(width / 2f))
                    { TopMargin = (int)(2 * width), BottomMargin = (int)(2 * width) },
                    Background = view.Context.GetDrawable(Resource.Drawable.separator)
                };
                view.FindViewById<LinearLayout>(Resource.Id.ImageSelectDictionary).AddView(SeparateView);
            }
            SelectDictionary.Dictionaries.DictionariesView.Add(ImageDictionary);

            return view;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var DBStatFlag = DBStatistics.GetImages(TableNamesImage.Flags.ToString());
            var DBStatFilms = DBStatistics.GetImages(TableNamesImage.Films.ToString());
            var view = inflater.Inflate(Resource.Layout.select_dictionary_image_fragment, container, false);
            CreateViewForDictionary(view, DBStatFlag, TableNamesImage.Flags.ToString(), Resource.Drawable.FlagDictionary, true, true, Colors.Orange, Colors.DarkOrange);
            CreateViewForDictionary(view, DBStatFilms, TableNamesImage.Films.ToString(), Resource.Drawable.FilmDictionary, false, false, Colors.Orange, Colors.DarkOrange);
            SelectDictionary.Dictionaries.Selected(DataBase.TableName.ToString(), DataBase.TableName.ToString());
            return view; 
        }
    }
}