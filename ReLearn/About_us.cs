using Android.Content;
using Calligraphy;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class About_us : Activity
    {
        [Java.Interop.Export("Button_Support_Project_Click")]
        public void Button_Support_Project_Click(View v)
        {
            Intent browserIntent = new Intent(Intent.ActionView);
            browserIntent.SetData(Android.Net.Uri.Parse("https://www.patreon.com/SemdelionTeam"));
            StartActivity(browserIntent);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.About_us);
            Toolbar toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbar_About_Us);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
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