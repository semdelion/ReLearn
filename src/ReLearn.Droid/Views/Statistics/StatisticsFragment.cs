using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.Tabs;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.ViewPager;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Views.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Droid.Views.Statistics
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("relearn.droid.views.statistics.StatisticsFragment")]
    public class StatisticsFragment : MvxBaseFragment<StatisticViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_menu_statistics;

        protected override int Toolbar => Resource.Id.toolbar_statistics;

        public static Color LightColor { get; private set; }
        public static Color DarkColor { get; private set; }
        public static List<DBStatistics> StatisticsDatabase { get; private set; }
        public static string DataTupe { get; private set; }

        private async Task GetDate()
        {
            var isContain = DatabaseImages.DatabaseIsContain($"{DataBase.TableName}");
            LightColor = isContain ? Colors.Orange : Colors.Blue;
            DarkColor = isContain ? Colors.DarkOrange : Colors.DarkBlue;
            DataTupe = isContain ? $"{DataBase.TableName}" : "Words";
            StatisticsDatabase = isContain
                ? await DBStatistics.GetImages($"{DataBase.TableName}")
                : await DBStatistics.GetWords($"{DataBase.TableName}");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            Task.Run(async () => await GetDate()).Wait();
            var viewPager = view.FindViewById<ViewPager>(Resource.Id.pager);
            if (viewPager != null)
            {
                var fragments = new List<MvxViewPagerFragmentInfo>
                {
                    new MvxViewPagerFragmentInfo("", "", typeof(TabMainFragment),  new MvxViewModelRequest(typeof(MainStatisticsViewModel))),
                    new MvxViewPagerFragmentInfo("", "", typeof(TabGeneralFragment),  new MvxViewModelRequest(typeof(GeneralStatisticsViewModel)))
                };
                viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(Activity, ChildFragmentManager, fragments);
            }

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tablayout);

            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.GetTabAt(tabLayout.TabCount - 2).SetIcon(Resource.Drawable.ic_statistics_main);
            tabLayout.GetTabAt(tabLayout.TabCount - 1).SetIcon(Resource.Drawable.ic_statistics_general);
            
            return view;
        }
    }
}