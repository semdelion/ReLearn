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
            Intent intent_english = new Intent(this, typeof(English));
            StartActivity(intent_english);
        }

        [Java.Interop.Export("Button_Flags_Click")]
        public void Button_Flags_Click(View v)
        {
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
    }
}

