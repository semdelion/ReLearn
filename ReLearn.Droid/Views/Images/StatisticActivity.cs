﻿using Android.App;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Content;
using Calligraphy;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;
using ReLearn.Core.ViewModels.Images;

namespace ReLearn.Droid.Images
{
    [Activity( Theme = "@style/ThemeStat", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class StatisticActivity : MvxActivity<StatisticViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.StatisticsActivity);

            Typeface face = Typeface.CreateFromAsset(Assets, Settings.font);
            TextView textView = new TextView(ApplicationContext)
            {
                Text = GetString(Resource.String.Statistics),
                Typeface = face,
                Gravity = GravityFlags.CenterVertical
            };
            textView.SetTextSize(Android.Util.ComplexUnitType.Dip, 25f);
            textView.SetTextColor(Colors.White);

            ActionBar.SetDisplayOptions(ActionBarDisplayOptions.ShowCustom, ActionBarDisplayOptions.ShowCustom | ActionBarDisplayOptions.ShowTitle);
            ActionBar.SetCustomView(textView, new ActionBar.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundEnFl));
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Color.Argb(128, 0, 0, 0));

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            SetContentView(Resource.Layout.StatisticsActivity);

            ActionBar.SetStackedBackgroundDrawable(new ColorDrawable(Color.Transparent));
            GraphGeneralStatistics Stat2 = new GraphGeneralStatistics(this, Colors.Orange, Colors.DarkOrange, 
                DBStatistics.GetImages(DataBase.TableNameImage.ToString()), 
                DataBase.TableNameImage.ToString(), DataBase.TableNameImage.ToString());
            GraphStatistics Stat1 = new GraphStatistics(this, Colors.Orange, Colors.DarkOrange, DataBase.TableNameImage.ToString());

            var tab = ActionBar.NewTab();
            tab.SetIcon(Resource.Drawable.Stat1);/// icon 1
            tab.TabSelected += (sender, args) => {
                SetContentView(Stat1);
            };
            ActionBar.AddTab(tab);

            tab = ActionBar.NewTab();
            tab.SetIcon(Resource.Drawable.Stat2);/// icon 2
            tab.TabSelected += (sender, args) => {
                SetContentView(Stat2);
            };
            ActionBar.AddTab(tab);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}