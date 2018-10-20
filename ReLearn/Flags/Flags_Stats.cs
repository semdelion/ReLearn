﻿using Android.App;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.Content;
using Calligraphy;
using Android.Widget;

namespace ReLearn
{
    [Activity( Theme = "@style/ThemeStat", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags_Stats : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Stat);

            Typeface face = Typeface.CreateFromAsset(Assets, Magic_constants.font);
            TextView textView = new TextView(ApplicationContext)
            {
                Text = Additional_functions.GetResourceString("Statistics", this.Resources),
                Typeface = face,
                Gravity = GravityFlags.CenterVertical
            };
            textView.SetTextSize(Android.Util.ComplexUnitType.Dip, 25f);
            textView.SetTextColor(Color.Rgb(215, 248, 254));

            ActionBar.SetDisplayOptions(ActionBarDisplayOptions.ShowCustom, ActionBarDisplayOptions.ShowCustom | ActionBarDisplayOptions.ShowTitle);
            ActionBar.SetCustomView(textView, new ActionBar.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundEnFl));
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Color.Argb(128, 0, 0, 0));

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            SetContentView(Resource.Layout.English_Stat);

            var database = DataBase.Connect(Database_Name.Flags_DB);
            ActionBar.SetStackedBackgroundDrawable(new ColorDrawable(Color.Transparent));
            List<Database_for_stats> Database_NL_and_D = database.Query<Database_for_stats>("SELECT NumberLearn, DateRecurrence FROM " + DataBase.TableNameImage);// количество строк в БД

            Graph_General_Statistics Stat2 = new Graph_General_Statistics(this, Color.Argb(255, 254, 166, 10), Color.Argb(255, 154, 66, 3), Database_NL_and_D,"Flags", DataBase.TableNameImage);
            Graph_Statistics Stat1 = new Graph_Statistics(this, Color.Argb(255, 254, 166, 10), Color.Argb(255, 154, 66, 3), DataBase.TableNameImage);

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
            this.Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}