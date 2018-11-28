using Android.Content;
using Calligraphy;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Languages : AppCompatActivity
    {
        [Java.Interop.Export("Button_Languages_Add_Click")]
        public void Button_Languages_Add_Click(View v) => StartActivity(typeof(Languages_Add));
        
        [Java.Interop.Export("Button_Languages_Learn_Click")]
        public void Button_Languages_Learn_Click(View v) => StartActivity(typeof(Languages_Learn));

        [Java.Interop.Export("Button_Languages_Repeat_Click")]
        public void Button_Languages_Repeat_Click(View v) => StartActivity(typeof(Languages_Repeat));
           
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Languages);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarLanguages);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_english, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.Stats)
                StartActivity(typeof(Languages_Stat));
            else if(item.ItemId == Resource.Id.Deleteword)
                StartActivity(typeof(Languages_View_Dictionary));
            else if(item.ItemId == Resource.Id.MenuEnglishSelectDictionary)
                StartActivity(typeof(Languages_SelectDictionary));
            else if(item.ItemId == Android.Resource.Id.Home)
                Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}