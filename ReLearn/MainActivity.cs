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
        public static Button button_english;
        public static Button button_flags;
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            GUI.Res = this;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundMain));
            DataBase.GetDatabasePath(Database_Name.English_DB);
            DataBase.GetDatabasePath(Database_Name.Flags_DB);
            DataBase.GetDatabasePath(Database_Name.Statistics);

            button_english = FindViewById<Button>(Resource.Id.button_english);
            button_flags = FindViewById<Button>(Resource.Id.button_flags);

            button_english.Touch += GUI.Button_Touch;
            button_flags.Touch += GUI.Button_Touch;

            button_english.Click += GUI.Button_1_Click;
            button_flags.Click += GUI.Button_1_Click;

            button_english.Click += delegate
            {
                Intent intent_english = new Intent(this, typeof(English));
                StartActivity(intent_english);
            };

            button_flags.Click += delegate
            {
                Intent intent_flags = new Intent(this, typeof(Flags));
                StartActivity(intent_flags);
            };
        }
    }
}

