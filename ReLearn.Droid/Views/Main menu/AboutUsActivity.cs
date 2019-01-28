using Android.Content;
using Calligraphy;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid
{
    //[Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    //public class AboutUsActivity : MvxAppCompatActivity<AboutUsViewModel>
    //{
    //    [Java.Interop.Export("Button_Support_Project_Click")] //TODO 
    //    public void Button_Support_Project_Click(View v)
    //    {
    //        Intent browserIntent = new Intent(Intent.ActionView);
    //        browserIntent.SetData(Android.Net.Uri.Parse("http://www.donationalerts.ru/r/semdelionteam"));
    //        StartActivity(browserIntent);
    //    }

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
}