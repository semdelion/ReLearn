using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReLearn
{
    [Activity(Label = "Statistics", Theme = "@style/ThemeStat")]
    class Flags_Stats : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Stat);
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundEnFl));
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(128, 0, 0, 0));
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            SetContentView(Resource.Layout.English_Stat);
            TextView tv = new TextView(this);////////////////

            var tab = ActionBar.NewTab();
            tab.SetIcon(Resource.Drawable.arrow);/// icon 1
            tab.TabSelected += (sender, args) => {
                SetContentView(new Graph_Statistics(this));
            };
            ActionBar.AddTab(tab);

            tab = ActionBar.NewTab();
            tab.SetIcon(Resource.Drawable.arrow); /// icon 2
            tab.TabSelected += (sender, args) => {
                SetContentView(tv);
            };
            ActionBar.AddTab(tab);
            
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
}