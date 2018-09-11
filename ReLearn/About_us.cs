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
    [Activity(Label = "Language", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class About_us : Activity
    {
        [Java.Interop.Export("Button_Support_Project_Click")]
        public void Button_Support_Project_Click(View v)
        {
            Intent browserIntent = new Intent(Intent.ActionView);
            browserIntent.SetData(Android.Net.Uri.Parse("https://www.patreon.com/SemdelionTeam"));
            StartActivity(browserIntent);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.About_us);
            Toolbar toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbar_About_Us);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;         
            if (id == Android.Resource.Id.Home)
            {
                this.Finish();
                return true;
            }
            return true;
        }

    }
}