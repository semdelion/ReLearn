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
        [Java.Interop.Export("Button_Send_Click")]
        public void Button_Send_Click(View v)
        {
            EditText editText_Feedback = FindViewById<EditText>(Resource.Id.editText_Feedback);
            if (editText_Feedback.Text == "")
                Toast.MakeText(this, GetString(Resource.String.Enter_word), ToastLength.Short).Show();
            else
            {
                var email = new Intent(Intent.ActionSend);
                email.PutExtra(Intent.ExtraEmail, new string[] { "SemdelionTeam@gmail.com" });
                email.PutExtra(Intent.ExtraSubject, "Hello, SemdelionTeam!");
                email.PutExtra(Intent.ExtraText, editText_Feedback.Text);
                email.SetType("message/rfc822");
                StartActivity(email);
                editText_Feedback.Text = "";
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MenuFeedbackActivity);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_Feedback);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            
            Finish();
            return base.OnOptionsItemSelected(item);
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
}