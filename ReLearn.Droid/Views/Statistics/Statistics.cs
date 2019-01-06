using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels.Images;
using Android.Views;

namespace ReLearn.Droid.Views.Statistics
{
    [Activity(Label ="", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Statistics : MvxAppCompatActivity<StatisticViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Statistics);
            SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_statistics));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.pager);
            StatisticsPagerAdapter myPagerAdapter = new StatisticsPagerAdapter(SupportFragmentManager);
            viewPager.Adapter = myPagerAdapter;
            TabLayout tabLayout = FindViewById<TabLayout>(Resource.Id.tablayout);

            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.GetTabAt(tabLayout.TabCount - 2).SetIcon(Resource.Drawable.Stat1);
            tabLayout.GetTabAt(tabLayout.TabCount - 1).SetIcon(Resource.Drawable.Stat2);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}
