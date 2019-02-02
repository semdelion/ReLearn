using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using ReLearn.API;
using ReLearn.API.Database;
using System.Collections.Generic;

namespace ReLearn.Droid.Views.SelectDictionary
{
    class TabLanguageFragment : Android.Support.V4.App.Fragment
    {
        public View CreateViewForDictionary(View view, List<DBStatistics> DB, string NameDictionary, int ImageId, GravityFlags flag, bool separate, Color lightColor, Color darkColor)
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
                Gravity = flag
            };
            ImageDictionary.SetPadding((int)(5 * width), 0, (int)(5 * width), 0);
            ImageDictionary.SetImageBitmap(SelectDictionary.Dictionaries.DictionariesBitmap[SelectDictionary.Dictionaries.DictionariesBitmap.Count - 1]);
            ImageDictionary.Click += SelectDictionary.SelectDictionaryClick;

            TextView Name = new TextView(view.Context)
            {
                Text = Droid.GetString.GetResourceString(NameDictionary, this.Resources),
                TextSize = 20//(int)(3 * width)
            };
            TextView CountWords = new TextView(view.Context)
            {
                Text = $"{GetString(Resource.String.DatatypeWords)} {count}, {GetString(Resource.String.StudiedAt)} " +
                $"{(int)(100 - API.Statistics.GetAverageNumberLearn(DB) * 100f / Settings.StandardNumberOfRepeats)}%",
                TextSize = 14//(int)(2.1f * width)
            };
            TextView Description = new TextView(view.Context)
            {
                Text = Droid.GetString.GetResourceString($"{NameDictionary}Description", this.Resources),
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
            TextlinearLayout.SetPadding(flag == GravityFlags.Left ? 0 : (int)(5 * width), 0, flag == GravityFlags.Right ? 0 : (int)(5 * width), 0);

            DictionarylinearLayout.AddView(flag == GravityFlags.Left ? (View)ImageDictionary : TextlinearLayout);
            DictionarylinearLayout.AddView(flag == GravityFlags.Right ? (View)ImageDictionary : TextlinearLayout);

            view.FindViewById<LinearLayout>(Resource.Id.LanguageSelectDictionary).AddView(DictionarylinearLayout);
            if (separate)
            {
                View SeparateView = new View(view.Context)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, (int)(width / 2f))
                    { TopMargin = (int)(2 * width), BottomMargin = (int)(2 * width) },
                    Background = view.Context.GetDrawable(Resource.Drawable.separator)
                };
                view.FindViewById<LinearLayout>(Resource.Id.LanguageSelectDictionary).AddView(SeparateView);
            }
            SelectDictionary.Dictionaries.DictionariesView.Add(ImageDictionary);

            return view;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_tab_languages_dictionary, container, false);
            var Home             = DBStatistics.GetWords($"{TableNamesLanguage.Home}");
            var Education        = DBStatistics.GetWords($"{TableNamesLanguage.Education}");
            var Popular_Words    = DBStatistics.GetWords($"{TableNamesLanguage.Popular_Words}");
            var ThreeFormsOfVerb = DBStatistics.GetWords($"{TableNamesLanguage.ThreeFormsOfVerb}");
            var ComputerScience  = DBStatistics.GetWords($"{TableNamesLanguage.ComputerScience}");
            var Nature           = DBStatistics.GetWords($"{TableNamesLanguage.Nature}");
            var My_Directly      = DBStatistics.GetWords($"{TableNamesLanguage.My_Directly}");
            CreateViewForDictionary(view, Home,
                $"{TableNamesLanguage.Home}",
                Resource.Drawable.homeDictionary,
                GravityFlags.Left,  true, Colors.Blue, Colors.DarkBlue);
            CreateViewForDictionary(view, Education,        
                $"{TableNamesLanguage.Education}",
                Resource.Drawable.EducationDictionary,
                GravityFlags.Right, true, Colors.Blue, Colors.DarkBlue);
            CreateViewForDictionary(view, Popular_Words,    
                $"{TableNamesLanguage.Popular_Words}",   
                Resource.Drawable.PopularWordsDictionary,
                GravityFlags.Left,  true, Colors.Blue, Colors.DarkBlue);
            CreateViewForDictionary(view, ThreeFormsOfVerb, 
                $"{TableNamesLanguage.ThreeFormsOfVerb}", 
                Resource.Drawable.ThreeFormsOfVerbDictionary,
                GravityFlags.Right, true, Colors.Blue, Colors.DarkBlue);
            CreateViewForDictionary(view, ComputerScience,  
                $"{TableNamesLanguage.ComputerScience}", 
                Resource.Drawable.ComputerScience,
                GravityFlags.Left,  true, Colors.Blue, Colors.DarkBlue);
            CreateViewForDictionary(view, Nature,           
                $"{TableNamesLanguage.Nature}",           
                Resource.Drawable.Nature,
                GravityFlags.Right, true, Colors.Blue, Colors.DarkBlue);
            CreateViewForDictionary(view, My_Directly,     
                $"{TableNamesLanguage.My_Directly}",      
                Resource.Drawable.MyDictionary,
                GravityFlags.Left, false, Colors.Blue, Colors.DarkBlue);
            return view;
        }
    }
}