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
            Additional_functions.Font();
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundMain));
            SetContentView(Resource.Layout.Main);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarMain);
            SetSupportActionBar(toolbarMain);
            base.OnCreate(savedInstanceState);                     
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.settings, menu);
            //expandableListView.ChildClick += (s, args) =>
            //{
            //    var item = expandableListView.Adapter.GetItem(args.GroupPosition);
            //    var document = collectionsDocuments[args.GroupPosition].Documents[args.ChildPosition];
            //    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            //    alert.SetTitle("");
            //    alert.SetMessage($"Select Document:\n{item}\n{args.Id}?");
            //    alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
            //    alert.SetNeutralButton("Ok", delegate
            //    {
            //        Intent intent = new Intent(this, typeof(ViewDocument));
            //        intent.PutExtra("Document", document);
            //        StartActivity(intent);
            //        this.Finish();
            //        Toast.MakeText(this, "Selected Document: " + item, ToastLength.Short).Show();
            //    });
            //    alert.Show();
            //};

            return base.OnCreateOptionsMenu(menu);
        }

        //public override bool OnPrepareOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.settings, menu);
        //    return true;
        //}     

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

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}

