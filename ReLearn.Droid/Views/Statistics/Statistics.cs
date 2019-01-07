using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using Android.Views;
using ReLearn.Core.ViewModels;
using Android.Graphics;
using System.Collections.Generic;

namespace ReLearn.Droid.Views.Statistics
{
    [Activity(Label ="", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Statistics : MvxAppCompatActivity<StatisticViewModel>
    {
        public static Color LightColor { get; private set; }
        public static Color DarkColor { get; private set; }
        public static List<DBStatistics> StatisticsDB { get; private set; }
        public static string DataTupe { get; private set; }

        private void GetDate()
        {
            bool isContain = DBImages.DatabaseIsContain(DataBase.TableName.ToString());
            LightColor   = isContain ? Colors.Orange     : Colors.Blue;
            DarkColor    = isContain ? Colors.DarkOrange : Colors.DarkBlue;
            DataTupe     = isContain ?  "Flags" : "Words";
            StatisticsDB = isContain ? DBStatistics.GetImages(DataBase.TableName.ToString()) :
                                       DBStatistics.GetWords(DataBase.TableName.ToString());
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Statistics);
            SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_statistics));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            GetDate();
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.pager);
            StatisticsPagerAdapter myPagerAdapter = new StatisticsPagerAdapter(SupportFragmentManager);
            viewPager.Adapter = myPagerAdapter;
            TabLayout tabLayout = FindViewById<TabLayout>(Resource.Id.tablayout);

            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.GetTabAt(tabLayout.TabCount - 2).SetIcon(Resource.Drawable.icon_main_statistics);
            tabLayout.GetTabAt(tabLayout.TabCount - 1).SetIcon(Resource.Drawable.icon_general_statistics);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}
