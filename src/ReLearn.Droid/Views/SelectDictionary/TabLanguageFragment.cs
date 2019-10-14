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

namespace ReLearn.Droid.Views.SelectDictionary
{
    internal class TabLanguageFragment : MvxFragment<DictionaryLanguageViewModel>
    {
        public View CreateViewForDictionary(View view, List<DBStatistics> database, string nameDictionary, int imageId,
            GravityFlags flag, bool separate, Color lightColor, Color darkColor)
        {
            var width = Resources.DisplayMetrics.WidthPixels / 100f;
            var count = database.Count;
            var dictionarylinearLayout = new LinearLayout(view.Context)
            {
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.WrapContent)
            };
            SelectDictionaryFragment.Dictionaries.DictionariesBitmap.Add(
                SelectDictionaryFragment.Dictionaries.CreateBitmapWithStats(
                    BitmapFactory.DecodeResource(Resources, imageId), database, lightColor,
                    darkColor)); //////fail color
            var ImageDictionary = new ImageView(view.Context) { Tag = $"{nameDictionary}" };
            ImageDictionary.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent)
            {
                Gravity = flag
            };
            ImageDictionary.SetPadding((int)(5 * width), 0, (int)(5 * width), 0);
            ImageDictionary.SetImageBitmap(
                SelectDictionaryFragment.Dictionaries.DictionariesBitmap[
                    SelectDictionaryFragment.Dictionaries.DictionariesBitmap.Count - 1]);
            ImageDictionary.Click += SelectDictionaryFragment.SelectDictionaryClick;

            var name = new TextView(view.Context)
            {
                Text = ViewModel.GetNameDictionary(nameDictionary),
                TextSize = 20 //(int)(3 * width)
            };
            var countWords = new TextView(view.Context)
            {
                Text = $"{ViewModel.TextWords} {count}, {ViewModel.TextStudiedAt} " +
                       $"{(int)(100 - API.Statistics.GetAverageNumberLearn(database) * 100f / Settings.StandardNumberOfRepeats)}%",
                TextSize = 14 //(int)(2.1f * width)
            };
            var description = new TextView(view.Context)
            {
                Text = ViewModel.GetDescriptionDictionary(nameDictionary),
                TextSize = 11 //(int)(1.7f * width)
            };
            var rextlinearLayout = new LinearLayout(view.Context)
            {
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent),
                Orientation = Orientation.Vertical
            };

            name.LayoutParameters = countWords.LayoutParameters = description.LayoutParameters =
                new LinearLayout.LayoutParams(52 * Resources.DisplayMetrics.WidthPixels / 100,
                    ViewGroup.LayoutParams.WrapContent);
            name.SetTextColor(Colors.White);
            countWords.SetTextColor(Colors.HintWhite);
            description.SetTextColor(Colors.HintWhite);
            rextlinearLayout.AddView(name);
            rextlinearLayout.AddView(countWords);
            rextlinearLayout.AddView(description);
            rextlinearLayout.SetPadding(flag == GravityFlags.Left ? 0 : (int)(5 * width), 0,
                flag == GravityFlags.Right ? 0 : (int)(5 * width), 0);

            dictionarylinearLayout.AddView(flag == GravityFlags.Left ? (View)ImageDictionary : rextlinearLayout);
            dictionarylinearLayout.AddView(flag == GravityFlags.Right ? (View)ImageDictionary : rextlinearLayout);

            view.FindViewById<LinearLayout>(Resource.Id.LanguageSelectDictionary).AddView(dictionarylinearLayout);
            if (separate)
            {
                var separateView = new View(view.Context)
                {
                    LayoutParameters =
                        new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, (int)(width / 2f))
                        { TopMargin = (int)(2 * width), BottomMargin = (int)(2 * width) },
                    Background = view.Context.GetDrawable(Resource.Drawable.separator)
                };
                view.FindViewById<LinearLayout>(Resource.Id.LanguageSelectDictionary).AddView(separateView);
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