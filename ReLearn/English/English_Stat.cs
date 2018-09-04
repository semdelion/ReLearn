using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SQLite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace ReLearn
{
    [Activity(Label = "Statistics", Theme = "@style/ThemeStat")]
    class English_Stat : Activity
    {        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //setting layout
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Stat);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundEnFl));
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(128, 0, 0, 0));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            SetContentView(Resource.Layout.English_Stat);
            TextView tv = new TextView(this);////////////////

            var tab = ActionBar.NewTab();
            tab.SetIcon(Resource.Drawable.Stat2);/// icon 1
            tab.TabSelected += (sender, args) => {
                SetContentView(new Graph_Statistics(this));
            };
            ActionBar.AddTab(tab);

            tab = ActionBar.NewTab();
            tab.SetIcon(Resource.Drawable.Stat1);/// icon 2
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