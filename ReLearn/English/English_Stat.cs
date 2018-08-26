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
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(127, 0, 0, 0));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            NameDatabase.Statistics = NameDatabase.English_Stat_DB;
            SetContentView(new Graph_Statistics(this)); 
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
}