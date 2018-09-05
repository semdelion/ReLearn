using Android.App;
using Android.OS;
using Android.Views;
using Android.Graphics;

namespace ReLearn
{
    [Activity(Label = "Statistics", Theme = "@style/ThemeStat")]
    class Flags_Stats : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Stat);
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundEnFl));
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(128, 0, 0, 0));
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            SetContentView(Resource.Layout.English_Stat);

            //Shader shader = new LinearGradient(0, Bottom - padding_bottom - height, 0, Bottom - padding_bottom, ,, TileMode.Clamp); 
            Graph_General_Statistics Stat2 = new Graph_General_Statistics(this, Color.Argb(255, 254, 166, 10), Color.Argb(255, 154, 66, 3), true);////////////////
            Graph_Statistics Stat1 = new Graph_Statistics(this, Color.Argb(255, 254, 166, 10), Color.Argb(255, 154, 66, 3));

            var tab = ActionBar.NewTab();
            tab.SetIcon(Resource.Drawable.Stat2);/// icon 1
            tab.TabSelected += (sender, args) => {
                SetContentView(Stat1);
            };
            ActionBar.AddTab(tab);

            tab = ActionBar.NewTab();
            tab.SetIcon(Resource.Drawable.Stat1);/// icon 2
            tab.TabSelected += (sender, args) => {
                SetContentView(Stat2);
            };
            ActionBar.AddTab(tab);

        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
}