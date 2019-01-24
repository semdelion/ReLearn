using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class FeedbackActivity : MvxAppCompatActivity<FeedbackViewModel>
    {
        [Java.Interop.Export("Button_Send_Click")] //TODO android Intent;
        public void Button_Send_Click(View v)
        {
            if (ViewModel.Message == "" || ViewModel.Message == null)
                Toast.MakeText(this, GetString(Resource.String.Enter_word), ToastLength.Short).Show();
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
                    Toast.MakeText(this, "There are no email applications installed.", ToastLength.Long).Show();
                }
            }
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menu_feedback_activity);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_Feedback);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}