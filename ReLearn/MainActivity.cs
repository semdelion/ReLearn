using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content.PM;
using Java.Util;
using Plugin.Settings;
using Android.Content;
using Calligraphy;
using Android.Content.Res;
using System;
using Android.Support.V7.App;
using Android.Graphics;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Locale)]
    public class MainActivity : AppCompatActivity
    {
        [Java.Interop.Export("Button_Language_Click")]
        public void Button_Language_Click(View v) => StartActivity(new Intent(this, typeof(Languages)));
        
        [Java.Interop.Export("Button_Flags_Click")]
        public void Button_Flags_Click(View v) => StartActivity(new Intent(this, typeof(Flags)));
          
        protected override void OnCreate(Bundle savedInstanceState)
        {          
            AdditionalFunctions.Font();
            SetContentView(Resource.Layout.Main);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarMain);
            SetSupportActionBar(toolbarMain);
            base.OnCreate(savedInstanceState);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.settings, menu);
            return base.OnCreateOptionsMenu(menu);
        }
      
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == Resource.Id.about_us)
                StartActivity(new Intent(this, typeof(About_us)));
            else if(item.ItemId == Resource.Id.Feedback)
                StartActivity(new Intent(this, typeof(Feedback)));
            else if (item.ItemId == Android.Resource.Id.Home)
                Finish();
            else if (item.ItemId == Resource.Id.Settings_Menu)
            {
                StartActivity(new Intent(this, typeof(Settings_Menu)));
                Finish();
            }
            return true;
        }      

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}

