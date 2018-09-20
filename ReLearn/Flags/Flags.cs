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
            Magic_constants.Get_repeat_count("Images_repeat_count");
            Additional_functions.Font();
            DataBase.Table_Name = Table_name.Flags;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags);
            Toolbar toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarFlags);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            //Magic_constants.Language = Convert.ToInt32(CrossSettings.Current.GetValueOrDefault("ImageLanguage", null));
            //if (Magic_constants.Language == 0)
            //    selected = Resource.Id.language_eng;
            //else
            //    selected = Resource.Id.language_rus;
            DataBase.Update_Flags_DB();          
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_flags, menu);
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
            if (id == Resource.Id.View_dictionary_image)
            {
                Intent intent_flags_view = new Intent(this, typeof(Flags_View_Dictionary));
                StartActivity(intent_flags_view);
                return true;
            }
            if (id == Android.Resource.Id.Home)
                this.Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
    
}