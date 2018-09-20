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

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Locale)]
    public class MainActivity : Activity
    {
        [Java.Interop.Export("Button_Language_Click")]
        public void Button_Language_Click(View v)
        {
            Intent intent_english = new Intent(this, typeof(English));
            StartActivity(intent_english);
        }

        [Java.Interop.Export("Button_Flags_Click")]
        public void Button_Flags_Click(View v)
        {
            Intent intent_flags = new Intent(this, typeof(Flags));
            StartActivity(intent_flags);
        }

        void Checklanguage()
        {
            if (System.String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault("Language", null)))
                CrossSettings.Current.AddOrUpdateValue("Language", "en");
            Update_Configuration_Locale(CrossSettings.Current.GetValueOrDefault("Language", null));
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Checklanguage();          
            Additional_functions.Font();           
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundMain));
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            Toolbar toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarMain);
            SetActionBar(toolbarMain);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.settings, menu);
            return true;
        }     

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.about_us)
            {
                Intent intent_about_us = new Intent(this, typeof(About_us));
                StartActivity(intent_about_us);
                return true;
            }
            if (id == Resource.Id.Settings_Menu)
            {
                Intent intent_Settings_Menu = new Intent(this, typeof(Settings_Menu));
                StartActivity(intent_Settings_Menu);
                this.Finish();
                return true;
            }
            if (id == Resource.Id.Feedback)
            {
                Intent intent_Feedback = new Intent(this, typeof(Feedback));
                StartActivity(intent_Feedback);
                return true;
            }
            if (id == Android.Resource.Id.Home)
            {
                this.Finish();
                return true;
            }
            return true;
        }

        public void Update_Configuration_Locale(string str)
        {
            Locale locale = new Locale(str);
            Configuration conf = new Configuration { Locale = locale };
            Resources.UpdateConfiguration(conf, Resources.DisplayMetrics);
            //this.CreateConfigurationContext(conf);
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}

