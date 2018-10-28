using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
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
    public class Settings_Menu: AppCompatActivity
    {      
        int CheckedItem()
        {
            if (CrossSettings.Current.GetValueOrDefault(Settings.Language.ToString(), null) == Language.en.ToString())
            {
                FindViewById<TextView>(Resource.Id.language).Text = $"{ GetString(Resource.String.Language) }:\t\t\tEnglish";
                return 0;
            }
            else
            {
                FindViewById<TextView>(Resource.Id.language).Text = $"{ GetString(Resource.String.Language) }:\t\t\tРусский";
                return 1;
            }
        }

        [Java.Interop.Export("TextView_Language_Click")]
        public void TextView_Language_Click(View v)
        {            
            string[] listLanguage = { "English", "Русский" };
            int checkedItem = CheckedItem();

            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle(GetString(Resource.String.Language));
            alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
            alert.SetSingleChoiceItems(listLanguage, checkedItem, new EventHandler<DialogClickEventArgs>(delegate (object sender, DialogClickEventArgs e) 
            {
                var dialog = (sender as Android.App.AlertDialog);
                checkedItem = e.Which;
                if (listLanguage[e.Which] == "English")
                {
                    CrossSettings.Current.AddOrUpdateValue(Settings.Language.ToString(), Language.en.ToString());     
                    Toast.MakeText(this, GetString(Resource.String.EnIsSelected), ToastLength.Short).Show();
                }
                else
                {
                    CrossSettings.Current.AddOrUpdateValue(Settings.Language.ToString(), Language.ru.ToString());
                    Toast.MakeText(this, GetString(Resource.String.RuIsSelected), ToastLength.Short).Show();
                }
                Additional_functions.Update_Configuration_Locale(this.Resources);
                FindViewById<TextView>(Resource.Id.language).Text = $"{ GetString(Resource.String.Language) }:\t\t\t{listLanguage[e.Which]}";
                StartActivity(new Intent(this, typeof(Settings_Menu)));
                this.Finish();        
                dialog.Dismiss();
            }));          
            alert.Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {          
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings_Menu);
            CheckedItem();
            var toolbarSettings = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarSetting);
            SetSupportActionBar(toolbarSettings);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            SeekBar  SB_Repeat_Language = FindViewById<SeekBar> (Resource.Id.SeekBarCountRepeatLenguage),
                     SB_Repeat_Image    = FindViewById<SeekBar> (Resource.Id.SeekBarCountRepeatImages);
            TextView TV_Repeat_Language = FindViewById<TextView>(Resource.Id.TextView_number_of_word_repeats),
                     TV_Repeat_Image    = FindViewById<TextView>(Resource.Id.TextView_number_of_image_repeats);

            SB_Repeat_Language.Progress = Magic_constants.NumberOfRepeatsLanguage - 5; 
            SB_Repeat_Image.Progress    = Magic_constants.NumberOfRepeatsImage    - 5;
            TV_Repeat_Language.Text = GetString(Resource.String.Number_of_word_repeats)  + " " + Convert.ToString(5 + SB_Repeat_Language.Progress);
            TV_Repeat_Image.Text    = GetString(Resource.String.Number_of_image_repeats) + " " + Convert.ToString(5 + SB_Repeat_Image.Progress);

            SB_Repeat_Language.ProgressChanged += (s, e) =>
            {
                TV_Repeat_Language.Text = GetString(Resource.String.Number_of_word_repeats)  + " " + Convert.ToString(5 + e.Progress);
                Magic_constants.NumberOfRepeatsLanguage = e.Progress + 5;
            };

            SB_Repeat_Image.ProgressChanged += (s, e) =>
            {
                TV_Repeat_Image.Text    = GetString(Resource.String.Number_of_image_repeats) + " " + Convert.ToString(5 + e.Progress);
                Magic_constants.NumberOfRepeatsImage    = e.Progress + 5;
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                StartActivity(new Intent(this, typeof(MainActivity)));
                this.Finish();
            }
            return true;
        }    

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}