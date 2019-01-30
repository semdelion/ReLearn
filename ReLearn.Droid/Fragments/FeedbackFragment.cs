using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("relearn.droid.fragments.FeedbackFragment")]
    public class FeedbackFragment : BaseFragment<FeedbackViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_menu_feedback;

        protected override int Toolbar => Resource.Id.toolbar_Feedback;

        [Java.Interop.Export("Button_Send_Click")] 
        public void Button_Send_Click(View v)
        {
            if (ViewModel.Message == "" || ViewModel.Message == null)
                return;
            //Toast.MakeText(this, GetString(Resource.String.Enter_word), ToastLength.Short).Show();//TODO android Intent;
            else
            {
                var email = new Intent(Intent.ActionSend);
                email.PutExtra(Intent.ExtraEmail, new string[] { "SemdelionTeam@gmail.com" });
                email.PutExtra(Intent.ExtraSubject, "Hello, SemdelionTeam!");
                email.PutExtra(Intent.ExtraText, ViewModel.Message);
                email.SetType("message/rfc822");
                try
                {
                    StartActivity(email);
                    ViewModel.Message = "";
                }
                catch
                {
                    // Toast.MakeText(this, "There are no email applications installed.", ToastLength.Long).Show();
                }
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}

//[Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
//public class FeedbackActivity : MvxAppCompatActivity<FeedbackViewModel>
//{
//   
//    protected override void OnCreate(Bundle savedInstanceState)
//    {
//        base.OnCreate(savedInstanceState);
//        SetContentView(Resource.Layout.menu_feedback_activity);
//        var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_Feedback);
//        SetSupportActionBar(toolbarMain);
//        SupportActionBar.SetDisplayHomeAsUpEnabled(true);
//    }

//    public override bool OnOptionsItemSelected(IMenuItem item)
//    {
//        Finish();
//        return base.OnOptionsItemSelected(item);
//    }
//}