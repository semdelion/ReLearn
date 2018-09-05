using Android.App;
using Android.OS;
using Android.Views;
using Android.Graphics;

namespace ReLearn
{
    [Activity(Label = "Statistics", Theme = "@style/ThemeStat")]
    class English_Stat : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //setting layout
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Stat);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundEnFl));
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(128, 0, 0, 0));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            SetContentView(Resource.Layout.English_Stat);


            Graph_Statistics Stat1 = new Graph_Statistics(this, Color.Rgb(0, 255, 255), Color.Argb(255, 50, 60, 126));
            Graph_General_Statistics Stat2 = new Graph_General_Statistics(this, Color.Rgb(0, 255, 255), Color.Argb(255, 50, 60, 126), true);////////////////

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