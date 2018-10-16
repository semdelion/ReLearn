using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Calligraphy;
using Java.Util;
using Plugin.Settings;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Settings_Menu: Activity
    {      
        int CheckedItem()
        {
            if (CrossSettings.Current.GetValueOrDefault("Language", null) == "en")
            {
                FindViewById<TextView>(Resource.Id.language).Text = $"{Additional_functions.GetResourceString("Language", this.Resources) }:\t\t\tEnglish";
                return 0;
            }
            else
            {
                FindViewById<TextView>(Resource.Id.language).Text = $"{Additional_functions.GetResourceString("Language", this.Resources) }:\t\t\tРусский";
                return 1;
            }
        }

        [Java.Interop.Export("TextView_Language_Click")]
        public void TextView_Language_Click(View v)
        {            
            string[] listLanguage = { "English", "Русский" };
            int checkedItem = CheckedItem();

            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(Additional_functions.GetResourceString("Language", this.Resources));
            alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
            alert.SetSingleChoiceItems(listLanguage, checkedItem, new EventHandler<DialogClickEventArgs>(delegate (object sender, DialogClickEventArgs e) {
                var d = (sender as Android.App.AlertDialog);
                checkedItem = e.Which;
                if (listLanguage[e.Which] == "English")
                {
                    CrossSettings.Current.AddOrUpdateValue("Language", "en");     
                    Toast.MakeText(this, Additional_functions.GetResourceString("EnIsSelected", this.Resources), ToastLength.Short).Show();
                }
                else
                {
                    CrossSettings.Current.AddOrUpdateValue("Language", "ru");
                    Toast.MakeText(this, Additional_functions.GetResourceString("RuIsSelected", this.Resources), ToastLength.Short).Show();
                }
                Additional_functions.Update_Configuration_Locale(Plugin.Settings.CrossSettings.Current.GetValueOrDefault("Language", null), this.Resources);
                FindViewById<TextView>(Resource.Id.language).Text = $"{Additional_functions.GetResourceString("Language", this.Resources)}:\t\t\t{listLanguage[e.Which]}";
                StartActivity(new Intent(this, typeof(Settings_Menu)));
                this.Finish();        
                d.Dismiss();





































            }));          
            alert.Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {          
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings_Menu);
            CheckedItem();
            Toolbar toolbarSettings = FindViewById<Toolbar>(Resource.Id.toolbarSetting);
            SetActionBar(toolbarSettings);

            SeekBar SB_Repeat_Language = FindViewById<SeekBar>(Resource.Id.SeekBarCountRepeatLenguage);
            SeekBar SB_Repeat_Image = FindViewById<SeekBar>(Resource.Id.SeekBarCountRepeatImages);
            TextView TV_Repeat_Language = FindViewById<TextView>(Resource.Id.TextView_number_of_word_repeats);
            TextView TV_Repeat_Image = FindViewById<TextView>(Resource.Id.TextView_number_of_image_repeats);

            SB_Repeat_Language.Progress = Convert.ToInt32(CrossSettings.Current.GetValueOrDefault("Language_repeat_count", null)) - 5; 
            SB_Repeat_Image.Progress = Convert.ToInt32(CrossSettings.Current.GetValueOrDefault("Images_repeat_count", null)) - 5;

            TV_Repeat_Language.Text = Additional_functions.GetResourceString("number_of_word_repeats", this.Resources) + " " + Convert.ToString(5 + SB_Repeat_Language.Progress);
            TV_Repeat_Image.Text = Additional_functions.GetResourceString("number_of_image_repeats", this.Resources) + " " + Convert.ToString(5 + SB_Repeat_Image.Progress);

            SB_Repeat_Language.ProgressChanged += (s, e) =>
            {
                TV_Repeat_Language.Text = Additional_functions.GetResourceString("number_of_word_repeats", this.Resources) + " " + Convert.ToString(5 + e.Progress);
                Magic_constants.Set_repeat_count("Language_repeat_count", e.Progress + 5);
            };

            SB_Repeat_Image.ProgressChanged += (s, e) =>
            {
                TV_Repeat_Image.Text = Additional_functions.GetResourceString("number_of_image_repeats", this.Resources) + " " + Convert.ToString(5 + e.Progress);
                Magic_constants.Set_repeat_count("Images_repeat_count",  e.Progress + 5);
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Android.Resource.Id.Home)
            {
                StartActivity(new Intent(this, typeof(MainActivity)));
                this.Finish();
                return true;
            }
            return true;
        }    

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}