using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags : AppCompatActivity
    {
        [Java.Interop.Export("Button_Flags_Learn_Click")]
        public void Button_Flags_Learn_Click(View v) => StartActivity(new Intent(this, typeof(Flags_Learn)));
        

        [Java.Interop.Export("Button_Flags_Repeat_Click")]
        public void Button_Flags_Repeat_Click(View v) => StartActivity(new Intent(this, typeof(Flags_Repeat)));
             
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarFlags);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            DataBase.UpdateImagesToRepeat();          
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_flags, menu);
            return true;
        }    

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.Stats_Flags)
                StartActivity(new Intent(this, typeof(Flags_Stats)));
            else if (item.ItemId == Resource.Id.MenuFlagsSelectDictionary)
                StartActivity(new Intent(this, typeof(Flags_SelectDictionary)));
            else if(item.ItemId == Resource.Id.View_dictionary_image)
                StartActivity(new Intent(this, typeof(Flags_View_Dictionary)));
            else if(item.ItemId == Android.Resource.Id.Home)
                Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
    
}