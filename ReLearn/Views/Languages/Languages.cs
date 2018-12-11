using Android.Content;
using Calligraphy;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;

namespace ReLearn.Droid.Languages
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Languages : AppCompatActivity
    {
        [Java.Interop.Export("Button_Languages_Add_Click")]
        public void Button_Languages_Add_Click(View v) => StartActivity(typeof(Add));
        
        [Java.Interop.Export("Button_Languages_Learn_Click")]
        public void Button_Languages_Learn_Click(View v) => StartActivity(typeof(Learn));

        [Java.Interop.Export("Button_Languages_Repeat_Click")]
        public void Button_Languages_Repeat_Click(View v)
        {
            if (Settings.TypeOfRepetition == TypeOfRepetitions.Blitz && Statistics.Count == 0 && Settings.BlitzEnable)
            {
                Settings.TypeOfRepetition = TypeOfRepetitions.FourOptions;
                StartActivity(typeof(BlitzPoll));
                return;
            }
            else
            {
                Settings.TypeOfRepetition = TypeOfRepetitions.Blitz;
                StartActivity(typeof(Repeat));
            }
        }
           
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
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.Stats:
                    StartActivity(typeof(Statistic));
                    return true;
                case Resource.Id.Deleteword:
                    StartActivity(typeof(ViewDictionary));
                    return true;
                case Resource.Id.MenuEnglishSelectDictionary:
                    StartActivity(typeof(SelectDictionary));
                    return true;
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}