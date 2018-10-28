using Android.Content;
using Calligraphy;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class About_us : AppCompatActivity
    {
        [Java.Interop.Export("Button_Support_Project_Click")]
        public void Button_Support_Project_Click(View v)
        {
            Intent browserIntent = new Intent(Intent.ActionView);
            browserIntent.SetData(Android.Net.Uri.Parse("http://www.donationalerts.ru/r/semdelionteam"));
            StartActivity(browserIntent);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.About_us);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_About_Us);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {      
            if (item.ItemId == Android.Resource.Id.Home)
                this.Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}