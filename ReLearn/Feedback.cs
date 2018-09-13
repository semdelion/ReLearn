using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Feedback : Activity
    {
        [Java.Interop.Export("Button_Send_Click")]
        public void Button_Send_Click(View v)
        {
            EditText editText_Feedback = FindViewById<EditText>(Resource.Id.editText_Feedback);
            if (editText_Feedback.Text == "")
                Toast.MakeText(this, Additional_functions.GetResourceString("Enter_word", this.Resources), ToastLength.Short).Show();
            else
            {
                var email = new Intent(Intent.ActionSend);
                email.PutExtra(Intent.ExtraEmail, new string[] { "SemdelionTeam@gmail.com" });
                email.PutExtra(Intent.ExtraSubject, "Hello, SemdelionTeam!");
                email.PutExtra(Intent.ExtraText, editText_Feedback.Text);
                email.SetType("message/rfc822");
                StartActivity(email);
                editText_Feedback.Text = "";
               // Toast.MakeText(this, Additional_functions.GetResourceString("Message_Sent", this.Resources), ToastLength.Short).Show();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Additional_functions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Feedback);
            Toolbar toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbar_Feedback);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Android.Resource.Id.Home)
            {
                this.Finish();
                return true;
            }
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
}