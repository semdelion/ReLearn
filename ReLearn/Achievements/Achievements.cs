using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Common.Apis;
using Android.Support.V7.App;
using Android.Gms.Common;
using Android.Util;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using System;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    [Register("com.xamarin.signinquickstart.Achievements")]
    public class Achievements : AppCompatActivity, View.IOnClickListener, GoogleApiClient.IOnConnectionFailedListener, GoogleApiClient.IConnectionCallbacks
    {
        GoogleApiClient mGoogleApiClient;
        public int SIGN_IN_ID = 9001;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Achievements);

            var toolbarAchievements = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_Achievements);
            SetSupportActionBar(toolbarAchievements);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var signInButton = FindViewById<Button>(Resource.Id.signInButton);
            signInButton.Click += GoogleSignInClick;

            ConfigureGoogleSign();
        }

        private void ConfigureGoogleSign()
        {
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                                                             .RequestEmail()
                                                             .Build();
            mGoogleApiClient = new GoogleApiClient.Builder(this)
                                                  .EnableAutoManage(this, this)
                                                  .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                                                  .AddConnectionCallbacks(this)
                                                  .Build();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == SIGN_IN_ID)
            {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                HandleSignInResult(result);
            }
        }

        private void HandleSignInResult(GoogleSignInResult result)
        {
            if (result.IsSuccess)
            {
                var accountDetails = result.SignInAccount;
            }
        }

        private void GoogleSignInClick(object sender, EventArgs e)
        {
            var signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
            StartActivityForResult(signInIntent, SIGN_IN_ID);
        }

        public void OnConnected(Bundle connectionHint)
        {
            //throw new NotImplementedException();
        }

        public void OnConnectionSuspended(int cause)
        {
            //throw new NotImplementedException();
        }

        public void OnClick(View v)
        {
            //throw new NotImplementedException();
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
           // throw new NotImplementedException();
        }
    }
}