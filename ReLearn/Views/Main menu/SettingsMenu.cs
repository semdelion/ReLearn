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
    public class SettingsMenu : AppCompatActivity
    {
        string PronunciationText
        {
            get => FindViewById<TextView>(Resource.Id.pronunciation).Text; 
            set => FindViewById<TextView>(Resource.Id.pronunciation).Text = value; 
        }

        string LanguageText
        {
            get => FindViewById<TextView>(Resource.Id.language).Text; 
            set => FindViewById<TextView>(Resource.Id.language).Text = value; 
        }

        int CheckedItemLanguage()
        {
            if (Settings.Currentlanguage == Language.en.ToString())
            {
                LanguageText = $"{ GetString(Resource.String.Language) }:   English";
                return 0;
            }
            else
            {
                LanguageText = $"{ GetString(Resource.String.Language) }:   Русский";
                return 1;
            }
        }

        [Java.Interop.Export("TextView_Language_Click")]
        public void TextView_Language_Click(View v)
        {
            string[] listLanguage = { "English", "Русский" };
            int checkedItem = CheckedItemLanguage();

            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle(GetString(Resource.String.Language));
            alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
            alert.SetSingleChoiceItems(listLanguage, checkedItem, new EventHandler<DialogClickEventArgs>(delegate (object sender, DialogClickEventArgs e)
            {
                var dialog = (sender as Android.App.AlertDialog);
                checkedItem = e.Which;
                if (listLanguage[e.Which] == "English")
                    Settings.Currentlanguage = Language.en.ToString();
                else
                    Settings.Currentlanguage = Language.ru.ToString();

                AdditionalFunctions.Update_Configuration_Locale(this.Resources);
                LanguageText = $"{ GetString(Resource.String.Language) }:   {listLanguage[e.Which]}";
                StartActivity(typeof(SettingsMenu));
                Finish();
                dialog.Dismiss();
            }));
            alert.Show();
        }

        int CheckedItemPronunciation()
        {
            if (Settings.CurrentPronunciation == Pronunciation.en.ToString())
            {
                PronunciationText = $"{ GetString(Resource.String.Pronunciation) }:  American";
                return 0;
            }
            else
            {
                PronunciationText = $"{ GetString(Resource.String.Pronunciation) }:  British";
                return 1;
            }
        }

        [Java.Interop.Export("TextView_Pronunciation_Click")]
        public void TextView_Pronunciation_Click(View v)
        {
            string[] listPronunciation = { "American", "British" };
            int checkedItem = CheckedItemPronunciation();
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle(GetString(Resource.String.Pronunciation));
            alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
            alert.SetSingleChoiceItems(listPronunciation, checkedItem, new EventHandler<DialogClickEventArgs>(delegate (object sender, DialogClickEventArgs e)
            {
                var dialog = (sender as Android.App.AlertDialog);
                checkedItem = e.Which;
                if (listPronunciation[e.Which] == "American")
                    Settings.CurrentPronunciation = Pronunciation.en.ToString();
                else
                    Settings.CurrentPronunciation = Pronunciation.uk.ToString();

                PronunciationText = $"{ GetString(Resource.String.Pronunciation) }:   {listPronunciation[e.Which]}";
                StartActivity(typeof(SettingsMenu));
                Finish();
                dialog.Dismiss();
            }));
            alert.Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings_Menu);
            CheckedItemLanguage();
            CheckedItemPronunciation();
            var toolbarSettings = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarSetting);
            SetSupportActionBar(toolbarSettings);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            SeekBar SB_Repeat_Language = FindViewById<SeekBar>(Resource.Id.SeekBarCountRepeatLenguage),
                    SB_Repeat_Image = FindViewById<SeekBar>(Resource.Id.SeekBarCountRepeatImages),
                    SB_TimeToBlitz = FindViewById<SeekBar>(Resource.Id.SeekBarTimeToBlitz);
            TextView TV_Repeat_Language = FindViewById<TextView>(Resource.Id.TextView_number_of_word_repeats),
                     TV_Repeat_Image = FindViewById<TextView>(Resource.Id.TextView_number_of_image_repeats),
                     TV_TimeToBlitz = FindViewById<TextView>(Resource.Id.TextView_TimeToBlitz);

            SB_Repeat_Language.Progress = Settings.NumberOfRepeatsLanguage - 5;
            SB_Repeat_Image.Progress = Settings.NumberOfRepeatsImage - 5;
            SB_Repeat_Image.Progress = Settings.TimeToBlitz;
            TV_Repeat_Language.Text = $"{GetString(Resource.String.Number_of_word_repeats )} {Convert.ToString(5 + SB_Repeat_Language.Progress)}";
            TV_Repeat_Image.Text    = $"{GetString(Resource.String.Number_of_image_repeats)} {Convert.ToString(5 + SB_Repeat_Image.Progress)}";
            TV_TimeToBlitz          = $"{GetString(Resource.String.Time_blitz)} {Convert.ToString(SB_TimeToBlitz.Progress)}";

            SB_Repeat_Language.ProgressChanged += (s, e) =>
            {
                TV_Repeat_Language.Text = $"{GetString(Resource.String.Number_of_word_repeats)} {Convert.ToString(5 + e.Progress)}";
                Settings.NumberOfRepeatsLanguage = e.Progress + 5;
            };

            SB_Repeat_Image.ProgressChanged += (s, e) =>
            {
                TV_Repeat_Image.Text =    $"{GetString(Resource.String.Number_of_image_repeats)} {Convert.ToString(5 + e.Progress)}";
                Settings.NumberOfRepeatsImage = e.Progress + 5;
            };
            SB_TimeToBlitz.ProgressChanged += (s, e) =>
            {
                TV_TimeToBlitz.Text = $"{GetString(Resource.String.Time_blitz)} {Convert.ToString(e.Progress)}";
                Settings.NumberOfRepeatsImage = e.Progress;
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            StartActivity(typeof(MainActivity));
            Finish();
            return base.OnOptionsItemSelected(item);
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}