using Android.Content;
using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("relearn.droid.fragments.SettingsFragment")]
    public class AboutUslFragment : BaseFragment<AboutUsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_about_us;
        [Java.Interop.Export("Button_Support_Project_Click")] //TODO 
        public void Button_Support_Project_Click(View v)
        {
            Intent browserIntent = new Intent(Intent.ActionView);
            browserIntent.SetData(Android.Net.Uri.Parse("http://www.donationalerts.ru/r/semdelionteam"));
            StartActivity(browserIntent);
        }

        //protected override void OnCreateView(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //    SetContentView(Resource.Layout.menu_about_us_activity);
        //    var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_About_Us);
        //    SetSupportActionBar(toolbarMain);
        //    SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        //}

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            //Finish();
            return base.OnOptionsItemSelected(item);
        }

        
    }
}

//[Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
//public class AboutUsActivity : MvxAppCompatActivity<AboutUsViewModel>
//{
//    protected override void OnCreate(Bundle savedInstanceState)
//    {
//        base.OnCreate(savedInstanceState);
//        SetContentView(Resource.Layout.menu_about_us_activity);
//        var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_About_Us);
//        SetSupportActionBar(toolbarMain);
//        SupportActionBar.SetDisplayHomeAsUpEnabled(true);
//    }

//    public override bool OnOptionsItemSelected(IMenuItem item)
//    {      
//        Finish();
//        return base.OnOptionsItemSelected(item);
//    }

//    protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
//}