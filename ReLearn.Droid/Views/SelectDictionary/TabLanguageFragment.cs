using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.MainMenu.SelectDictionary;
using ReLearn.Droid.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Droid.Views.SelectDictionary
{
    class TabLanguageFragment : MvxFragment<DictionaryLanguageViewModel>
    {
        public View CreateViewForDictionary(View view, List<DBStatistics> database, string nameDictionary, int imageId, GravityFlags flag, bool separate, Color lightColor, Color darkColor)
        {
            var width = Resources.DisplayMetrics.WidthPixels / 100f;
            int count = database.Count;
            LinearLayout DictionarylinearLayout = new LinearLayout(view.Context)
            {
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
            };
            SelectDictionaryFragment.Dictionaries.DictionariesBitmap.Add(SelectDictionaryFragment.Dictionaries.CreateBitmapWithStats(BitmapFactory.DecodeResource(Resources, imageId), database, lightColor, darkColor));//////fail color
            ImageView ImageDictionary = new ImageView(view.Context) { Tag = $"{nameDictionary}" };
            ImageDictionary.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            {
                Gravity = flag
            };
            ImageDictionary.SetPadding((int)(5 * width), 0, (int)(5 * width), 0);
            ImageDictionary.SetImageBitmap(SelectDictionaryFragment.Dictionaries.DictionariesBitmap[SelectDictionaryFragment.Dictionaries.DictionariesBitmap.Count - 1]);
            ImageDictionary.Click += SelectDictionaryFragment.SelectDictionaryClick;

            TextView Name = new TextView(view.Context)
            {
                Text = Helpers.GetString.GetResourceString(nameDictionary, this.Resources),
                TextSize = 20//(int)(3 * width)
            };
            TextView CountWords = new TextView(view.Context)
            {
                Text = $"{GetString(Resource.String.DatatypeWords)} {count}, {GetString(Resource.String.StudiedAt)} " +
                $"{(int)(100 - API.Statistics.GetAverageNumberLearn(database) * 100f / Settings.StandardNumberOfRepeats)}%",
                TextSize = 14//(int)(2.1f * width)
            };
            TextView Description = new TextView(view.Context)
            {
                Text = Helpers.GetString.GetResourceString($"{nameDictionary}Description", this.Resources),
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

            view.FindViewById<LinearLayout>(Resource.Id.LanguageSelectDictionary).AddView(DictionarylinearLayout);
            if (separate)
            {
                View SeparateView = new View(view.Context)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, (int)(width / 2f))
                    { TopMargin = (int)(2 * width), BottomMargin = (int)(2 * width) },
                    Background = view.Context.GetDrawable(Resource.Drawable.separator)
                };
                view.FindViewById<LinearLayout>(Resource.Id.LanguageSelectDictionary).AddView(SeparateView);
            }
            SelectDictionaryFragment.Dictionaries.DictionariesView.Add(ImageDictionary);

            return view;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_tab_languages_dictionary, container, false);

            CreateViewForDictionary(view, ViewModel.Home,
                $"{TableNamesLanguage.Home}",
                Resource.Drawable.image_dictionary_home,
                GravityFlags.Left, true, Colors.Blue, Colors.DarkBlue);
            CreateViewForDictionary(view, ViewModel.Education,
                $"{TableNamesLanguage.Education}",
                Resource.Drawable.image_dictionary_education,
                GravityFlags.Right, true, Colors.Blue, Colors.DarkBlue);
            CreateViewForDictionary(view, ViewModel.PopularWords,
                $"{TableNamesLanguage.Popular_Words}",
                Resource.Drawable.image_dictionary_popular_words,
                GravityFlags.Left, true, Colors.Blue, Colors.DarkBlue);
            CreateViewForDictionary(view, ViewModel.ThreeFormsOfVerb,
                $"{TableNamesLanguage.ThreeFormsOfVerb}",
                Resource.Drawable.image_dictionary_three_forms_of_verb,
                GravityFlags.Right, true, Colors.Blue, Colors.DarkBlue);
            CreateViewForDictionary(view, ViewModel.ComputerScience,
                $"{TableNamesLanguage.ComputerScience}",
                Resource.Drawable.image_dictionary_computer_science,
                GravityFlags.Left, true, Colors.Blue, Colors.DarkBlue);
            CreateViewForDictionary(view, ViewModel.Nature,
                $"{TableNamesLanguage.Nature}",
                Resource.Drawable.image_dictionary_nature,
                GravityFlags.Right, true, Colors.Blue, Colors.DarkBlue);
            CreateViewForDictionary(view, ViewModel.MyDirectly,
                $"{TableNamesLanguage.My_Directly}",
                Resource.Drawable.image_dictionary_my,
                GravityFlags.Left, false, Colors.Blue, Colors.DarkBlue);
            return view;
        }
    }
}