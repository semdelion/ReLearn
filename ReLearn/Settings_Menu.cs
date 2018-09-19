using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Calligraphy;
using Plugin.Settings;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Settings_Menu: Activity
    {
        SeekBar SB_Repeat_Language;
        SeekBar SB_Repeat_Image;
        TextView TV_Repeat_Language;
        TextView TV_Repeat_Image;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings_Menu);
            Toolbar toolbarSettings = FindViewById<Toolbar>(Resource.Id.toolbarSetting);
            SetActionBar(toolbarSettings);
            SB_Repeat_Language = FindViewById<SeekBar>(Resource.Id.SeekBarCountRepeatLenguage);
            TV_Repeat_Language = FindViewById<TextView>(Resource.Id.TextView_number_of_word_repeats);

            SB_Repeat_Image = FindViewById<SeekBar>(Resource.Id.SeekBarCountRepeatImages);
            TV_Repeat_Image = FindViewById<TextView>(Resource.Id.TextView_number_of_image_repeats);

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
                this.Finish();
                return true;
            }
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}