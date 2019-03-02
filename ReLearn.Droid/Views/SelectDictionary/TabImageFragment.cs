using Android.Graphics;
using Android.OS;
using Android.Views;
using ReLearn.Droid.Helpers;
using Android.Widget;
using ReLearn.API;
using ReLearn.API.Database;
using System.Collections.Generic;
using MvvmCross.Droid.Support.V4;
using ReLearn.Core.ViewModels.MainMenu.SelectDictionary;

namespace ReLearn.Droid.Views.SelectDictionary
{
    class TabImageFragment : MvxFragment<DictionaryImageViewModel>
    {
        public View CreateViewForDictionary(View view, List<DBStatistics> DB, string NameDictionary, int ImageId, GravityFlags flag, bool separate, Color lightColor, Color darkColor)
        {
            var width = Resources.DisplayMetrics.WidthPixels / 100f;
            int count = DB.Count;
            LinearLayout DictionarylinearLayout = new LinearLayout(view.Context)
            {
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
            };
            SelectDictionaryFragment.Dictionaries.DictionariesBitmap.Add(SelectDictionaryFragment.Dictionaries.CreateBitmapWithStats(BitmapFactory.DecodeResource(Resources, ImageId), DB, lightColor, darkColor));//////fail color
            ImageView ImageDictionary = new ImageView(view.Context) { Tag = $"{NameDictionary}" };
            ImageDictionary.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            {
                Gravity = flag
            };
            ImageDictionary.SetPadding((int)(5 * width), 0, (int)(5 * width), 0);
            ImageDictionary.SetImageBitmap(SelectDictionaryFragment.Dictionaries.DictionariesBitmap[SelectDictionaryFragment.Dictionaries.DictionariesBitmap.Count - 1]);
            ImageDictionary.Click += SelectDictionaryFragment.SelectDictionaryClick;

            TextView Name = new TextView(view.Context)
            {
                Text = Helpers.GetString.GetResourceString(NameDictionary, this.Resources),
                TextSize = 20//(int)(3 * width)
            };
            TextView CountWords = new TextView(view.Context)
            {
                Text = $"{GetString(Resource.String.DatatypeImages)} {count}, {GetString(Resource.String.StudiedAt)} " +
                $"{(int)(100 - API.Statistics.GetAverageNumberLearn(DB) * 100f / Settings.StandardNumberOfRepeats)}%",
                TextSize = 14//(int)(2.1f * width)
            };
            TextView Description = new TextView(view.Context)
            {
                Text = Helpers.GetString.GetResourceString($"{NameDictionary}Description", this.Resources),
                TextSize = 11//(int)(1.7f * width)
            };
            LinearLayout TextlinearLayout = new LinearLayout(view.Context)
            {
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent),
                Orientation = Orientation.Vertical
            };

            Name.LayoutParameters = CountWords.LayoutParameters = Description.LayoutParameters =
                new LinearLayout.LayoutParams(52 * Resources.DisplayMetrics.WidthPixels / 100, ViewGroup.LayoutParams.WrapContent);
            Name.SetTextColor(Colors.White);
            CountWords.SetTextColor(Colors.HintWhite);
            Description.SetTextColor(Colors.HintWhite);
            TextlinearLayout.AddView(Name);
            TextlinearLayout.AddView(CountWords);
            TextlinearLayout.AddView(Description);
            TextlinearLayout.SetPadding(flag == GravityFlags.Left ? 0 : (int)(5 * width), 0, flag == GravityFlags.Right ? 0 : (int)(5 * width), 0);

            DictionarylinearLayout.AddView(flag == GravityFlags.Left ? (View)ImageDictionary : TextlinearLayout);
            DictionarylinearLayout.AddView(flag == GravityFlags.Right ? (View)ImageDictionary : TextlinearLayout);

            view.FindViewById<LinearLayout>(Resource.Id.ImageSelectDictionary).AddView(DictionarylinearLayout);
            if (separate)
            {
                View SeparateView = new View(view.Context)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, (int)(width / 2f))
                    { TopMargin = (int)(2 * width), BottomMargin = (int)(2 * width) },
                    Background = view.Context.GetDrawable(Resource.Drawable.separator)
                };
                view.FindViewById<LinearLayout>(Resource.Id.ImageSelectDictionary).AddView(SeparateView);
            }
            SelectDictionaryFragment.Dictionaries.DictionariesView.Add(ImageDictionary);

            return view;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_tab_images_dictionary, container, false);
            var DBStatFlag  = DBStatistics.GetImages($"{TableNamesImage.Flags}");
            var DBStatFilms = DBStatistics.GetImages($"{TableNamesImage.Films}");
            CreateViewForDictionary(view, DBStatFlag,   
                $"{TableNamesImage.Flags}", 
                Resource.Drawable.image_dictionary_flags,
                GravityFlags.Left,  true, Colors.Orange, Colors.DarkOrange);
            CreateViewForDictionary(view, DBStatFilms,  
                $"{TableNamesImage.Films}", 
                Resource.Drawable.image_dictionary_films,
                GravityFlags.Right, false, Colors.Orange, Colors.DarkOrange);
            SelectDictionaryFragment.Dictionaries.Selected($"{DataBase.TableName}",$"{DataBase.TableName}");
            return view; 
        }
    }
}