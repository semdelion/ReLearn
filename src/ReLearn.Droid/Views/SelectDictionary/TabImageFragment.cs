using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Views.Fragments;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.MainMenu.SelectDictionary;
using ReLearn.Droid.Helpers;
using System.Collections.Generic;

namespace ReLearn.Droid.Views.SelectDictionary
{
    internal class TabImageFragment : MvxFragment<DictionaryImageViewModel>
    {
        public View CreateViewForDictionary(View view, List<DBStatistics> database, string nameDictionary, int ImageId,
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
                    BitmapFactory.DecodeResource(Resources, ImageId), database, lightColor,
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
                Text = $"{ViewModel.TextImages} {count}, {ViewModel.TextStudiedAt} " +
                       $"{(int)(100 - API.Statistics.GetAverageNumberLearn(database) * 100f / Settings.StandardNumberOfRepeats)}%",
                TextSize = 14 //(int)(2.1f * width)
            };
            var description = new TextView(view.Context)
            {
                Text = ViewModel.GetDescriptionDictionary(nameDictionary),
                TextSize = 11 //(int)(1.7f * width)
            };
            var textlinearLayout = new LinearLayout(view.Context)
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
            textlinearLayout.AddView(name);
            textlinearLayout.AddView(countWords);
            textlinearLayout.AddView(description);
            textlinearLayout.SetPadding(flag == GravityFlags.Left ? 0 : (int)(5 * width), 0,
                flag == GravityFlags.Right ? 0 : (int)(5 * width), 0);

            dictionarylinearLayout.AddView(flag == GravityFlags.Left ? (View)ImageDictionary : textlinearLayout);
            dictionarylinearLayout.AddView(flag == GravityFlags.Right ? (View)ImageDictionary : textlinearLayout);

            view.FindViewById<LinearLayout>(Resource.Id.ImageSelectDictionary).AddView(dictionarylinearLayout);
            if (separate)
            {
                var SeparateView = new View(view.Context)
                {
                    LayoutParameters =
                        new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, (int)(width / 2f))
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
            CreateViewForDictionary(view, ViewModel.DatabaseFlag,
                $"{TableNamesImage.Flags}",
                Resource.Drawable.image_dictionary_flags,
                GravityFlags.Left, true, Colors.Orange, Colors.DarkOrange);
            CreateViewForDictionary(view, ViewModel.DatabaseFilms,
                $"{TableNamesImage.Films}",
                Resource.Drawable.image_dictionary_films,
                GravityFlags.Right, false, Colors.Orange, Colors.DarkOrange);
            SelectDictionaryFragment.Dictionaries.Selected($"{DataBase.TableName}", $"{DataBase.TableName}");
            return view;
        }
    }
}