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
            {
                Intent intent_flags_stat = new Intent(this, typeof(Flags_Stats));
                StartActivity(intent_flags_stat);
            }
            else if (item.ItemId == Resource.Id.MenuFlagsSelectDictionary)
            {
                Intent intent_SelectDictionary = new Intent(this, typeof(Flags_SelectDictionary));
                StartActivity(intent_SelectDictionary);
            }
            else if(item.ItemId == Resource.Id.View_dictionary_image)
            {
                Intent intent_flags_view = new Intent(this, typeof(Flags_View_Dictionary));
                StartActivity(intent_flags_view);
            }
            else if(item.ItemId == Android.Resource.Id.Home)
                this.Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
    
}