using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Graphics;
namespace ReLearn
{
    [Activity( MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        [Java.Interop.Export("Button_Language_Click")]
        public void Button_Language_Click(View v)
        {
            v.Enabled = false;
            Intent intent_english = new Intent(this, typeof(English));
            StartActivity(intent_english);
        }

        [Java.Interop.Export("Button_Flags_Click")]
        public void Button_Flags_Click(View v)
        {
            v.Enabled = false;
            Intent intent_flags = new Intent(this, typeof(Flags));
            StartActivity(intent_flags);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundMain));
            DataBase.GetDatabasePath(Database_Name.English_DB);
            DataBase.GetDatabasePath(Database_Name.Flags_DB);
            DataBase.GetDatabasePath(Database_Name.Statistics);
        }

        [Android.Runtime.Register("onWindowFocusChanged", "(Z)V", "GetOnWindowFocusChanged_ZHandler")]
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            if (hasFocus)
            {
                FindViewById<Button>(Resource.Id.button_english).Enabled = true;
                FindViewById<Button>(Resource.Id.button_flags).Enabled = true;
            }
        }
    }
}

