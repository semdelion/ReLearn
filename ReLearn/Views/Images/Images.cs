using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;

namespace ReLearn.Images
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Images : AppCompatActivity
    {
        [Java.Interop.Export("Button_Images_Learn_Click")]
        public void Button_Images_Learn_Click(View v) => StartActivity(typeof(Learn));
        
        [Java.Interop.Export("Button_Images_Repeat_Click")]
        public void Button_Images_Repeat_Click(View v) => StartActivity(typeof(Repeat));
             
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Images);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarImages);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_flags, menu);
            return base.OnPrepareOptionsMenu(menu);
        }    

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.Stats_Flags:
                    StartActivity(typeof(Statistic));
                    return true;
                case Resource.Id.MenuFlagsSelectDictionary:
                    StartActivity(typeof(SelectDictionary));
                    return true;
                case Resource.Id.View_dictionary_image:
                    StartActivity(typeof(ViewDictionary));
                    return true;
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
    
}