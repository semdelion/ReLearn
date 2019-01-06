using Android.App;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Content;
using Calligraphy;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;
using ReLearn.Core.ViewModels.Languages;

namespace ReLearn.Droid.Languages
{
    [Activity(Theme = "@style/ThemeStat", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class StatisticActivity : MvxActivity<StatisticViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Typeface face = Typeface.Create(Assets, Settings.font);
            TextView textView = new TextView(ApplicationContext)
            {
                Text = GetString(Resource.String.Statistics),
                // Typeface = face,
                Gravity = GravityFlags.CenterVertical
            };
            textView.SetTextSize(Android.Util.ComplexUnitType.Dip, 25f);
            textView.SetTextColor(Colors.White);

            ActionBar.SetDisplayOptions(ActionBarDisplayOptions.ShowCustom, ActionBarDisplayOptions.ShowCustom | ActionBarDisplayOptions.ShowTitle);
            ActionBar.SetCustomView(textView, new ActionBar.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            Window.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgroundEnFl));
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(128, 0, 0, 0));
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            SetContentView(Resource.Layout.StatisticsActivity);

            ActionBar.SetStackedBackgroundDrawable(new ColorDrawable(Color.Transparent));
            GraphStatistics Stat1 = new GraphStatistics(this, Colors.Blue, Colors.DarkBlue, DataBase.TableNameLanguage.ToString());

            GraphGeneralStatistics Stat2 = new GraphGeneralStatistics(
                this, Colors.Blue, Colors.DarkBlue,
                DBStatistics.GetWords(DataBase.TableNameLanguage.ToString()),
                "Words", DataBase.TableNameLanguage.ToString());

            var tab = ActionBar.NewTab();
            tab.SetIcon(Resource.Drawable.Stat1);/// icon 1
            tab.TabSelected += (sender, args) => SetContentView(Stat1);

            ActionBar.AddTab(tab);

            tab = ActionBar.NewTab();
            tab.SetIcon(Resource.Drawable.Stat2);/// icon 2
            tab.TabSelected += (sender, args) => SetContentView(Stat2);
            ActionBar.AddTab(tab);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}