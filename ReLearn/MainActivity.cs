using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Graphics;
using Android.Content.PM;
using Java.Util;
using Plugin.Settings;
using System.Globalization;
using System;
using Android.Content.Res;

namespace ReLearn
{
    [Activity( Label = "", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Locale)]
    public class MainActivity : Activity
    {
        private int selected = Resource.Id.en;
        public string language = CrossSettings.Current.GetValueOrDefault("Language", null);


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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (System.String.IsNullOrEmpty(language))
                CrossSettings.Current.AddOrUpdateValue("Language", "en");                         
            language = CrossSettings.Current.GetValueOrDefault("Language", null);        
            if(language == "en")
                selected = Resource.Id.en;
            else if (language == "ru")
                selected = Resource.Id.ru;
            Update_Configuration_Locale(language);

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundMain));
            Toolbar toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarMain);
            SetActionBar(toolbarMain);          
            
            DataBase.GetDatabasePath(Database_Name.English_DB);
            DataBase.GetDatabasePath(Database_Name.Flags_DB);
            DataBase.GetDatabasePath(Database_Name.Statistics);
        }        

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.settings, menu);
            if (selected == Resource.Id.ru)
            {
                menu.FindItem(Resource.Id.ru).SetChecked(true);
                return true;
            }
            if (selected == Resource.Id.en)
            {
                menu.FindItem(Resource.Id.en).SetChecked(true);    
                return true;
            }
            return true;
        }     

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.en)
            {                
                Toast.MakeText(this, Additional_functions.GetResourceString("EnIsSelected", this.Resources), ToastLength.Short).Show();
                CrossSettings.Current.AddOrUpdateValue("Language", "en");
                item.SetChecked(true);
                Update_Configuration_Locale("en");
                StartActivity(new Intent(this, typeof(MainActivity)));
                return true;
            }
            if (id == Resource.Id.ru) 
            {
                Toast.MakeText(this, Additional_functions.GetResourceString("RuIsSelected", this.Resources), ToastLength.Short).Show();
                CrossSettings.Current.AddOrUpdateValue("Language", "ru");
                item.SetChecked(true);
                Update_Configuration_Locale("ru");
                StartActivity(new Intent(this, typeof(MainActivity)));
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
            Configuration conf = new Configuration
            {
                Locale = locale
            };
            Resources.UpdateConfiguration(conf, Resources.DisplayMetrics);
            //this.CreateConfigurationContext(conf);
        }
    }
}

