using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Plugin.Settings;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    
    class Flags : Activity
    {
        private int selected = 0;

        [Java.Interop.Export("Button_Flags_Learn_Click")]
        public void Button_Flags_Learn_Click(View v)
        {
            Intent intent_flags_learn = new Intent(this, typeof(Flags_Learn));
            StartActivity(intent_flags_learn);
        }

        [Java.Interop.Export("Button_Flags_Repeat_Click")]
        public void Button_Flags_Repeat_Click(View v)
        {
            Intent intent_flags_repeat = new Intent(this, typeof(Flags_Repeat));
            StartActivity(intent_flags_repeat);
        }      

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            DataBase.Table_Name = Table_name.Flags;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags);
            Toolbar toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarFlags);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            Magic_constants.Language = Convert.ToInt32(CrossSettings.Current.GetValueOrDefault("ImageLanguage", null));
            if (Magic_constants.Language == 0)
                selected = Resource.Id.language_eng;
            else
                selected = Resource.Id.language_rus;
            DataBase.Update_Flags_DB();          
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_flags, menu);
            if (selected == Resource.Id.language_eng)
            {
                menu.FindItem(Resource.Id.language_eng).SetChecked(true);
                return true;
            }
            if (selected == Resource.Id.language_rus)
            {
                menu.FindItem(Resource.Id.language_rus).SetChecked(true);
                return true;
            }
            return true;
        }    

        public override bool OnOptionsItemSelected(IMenuItem item)
        {         
            int id = item.ItemId;
            if (id == Resource.Id.Stats_Flags)
            {
                Intent intent_flags_stat = new Intent(this, typeof(Flags_Stats));
                StartActivity(intent_flags_stat);
                return true;
            }
            if (id == Resource.Id.language_eng)
            {
                //databaseSetting.Query<Setting_Database>("UPDATE Setting_Database SET language = " + 0 + " WHERE Setting_bd = ?", "flags");
                Toast.MakeText(this, Additional_functions.GetResourceString("EnIsSelected", this.Resources), ToastLength.Short).Show();
                CrossSettings.Current.AddOrUpdateValue("ImageLanguage", "0");
                Magic_constants.Language = Convert.ToInt32(CrossSettings.Current.GetValueOrDefault("ImageLanguage", null));
                item.SetChecked(true);
                return true;
            }
            if (id == Resource.Id.language_rus)
            {
                Toast.MakeText(this, Additional_functions.GetResourceString("RuIsSelected", this.Resources), ToastLength.Short).Show();
                CrossSettings.Current.AddOrUpdateValue("ImageLanguage", "1");
                Magic_constants.Language = Convert.ToInt32(CrossSettings.Current.GetValueOrDefault("ImageLanguage", null));
                item.SetChecked(true);
                return true;
            }
            if (id == Android.Resource.Id.Home)
                this.Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
    
}