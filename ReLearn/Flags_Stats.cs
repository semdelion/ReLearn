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
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //setting layout

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Stat);

           // var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglishStat);
           // SetActionBar(toolbarMain);
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundEnFl));
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(127, 0, 0, 0));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            NameDatabase.Statistics = NameDatabase.Flags_DB_Stat_DB;
            SetContentView(new Graph_Statistics(this));
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
}