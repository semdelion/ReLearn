using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Views.Menu;
using ReLearn.Droid.Views.Statistics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Droid.Views.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    [Register("relearn.droid.views.statistics.StatisticsFragment")]
    public class StatisticsFragment : BaseFragment<StatisticViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_menu_statistics;

        protected override int Toolbar => Resource.Id.toolbar_statistics;

        public static Color LightColor { get; private set; }
        public static Color DarkColor { get; private set; }
        public static List<DBStatistics> StatisticsDatabase { get; private set; }
        public static string DataTupe { get; private set; }

        private async Task GetDate()
        {
            bool isContain = DBImages.DatabaseIsContain($"{DataBase.TableName}");
            LightColor = isContain ? Colors.Orange : Colors.Blue;
            DarkColor = isContain ? Colors.DarkOrange : Colors.DarkBlue;
            DataTupe = isContain ? $"{DataBase.TableName}" : "Words";
            StatisticsDatabase = isContain ? await DBStatistics.GetImages($"{DataBase.TableName}") :
                                       await DBStatistics.GetWords($"{DataBase.TableName}");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            Task.Run(async () => await GetDate()).Wait();
            ViewPager viewPager = view.FindViewById<ViewPager>(Resource.Id.pager);
            if (viewPager != null)
            {
                var fragments = new List<MvxViewPagerFragmentInfo>
                {
                    new MvxViewPagerFragmentInfo("", "", typeof(TabMainFragment), typeof(MainStatisticsViewModel)),
                    new MvxViewPagerFragmentInfo("", "", typeof(TabGeneralFragment), typeof(GeneralStatisticsViewModel))
                };
                viewPager.Adapter = new MvxFragmentStatePagerAdapter(Activity, ChildFragmentManager, fragments); 
            }

            TabLayout tabLayout = view.FindViewById<TabLayout>(Resource.Id.tablayout);

            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.GetTabAt(tabLayout.TabCount - 2).SetIcon(Resource.Drawable.ic_statistics_main);
            tabLayout.GetTabAt(tabLayout.TabCount - 1).SetIcon(Resource.Drawable.ic_statistics_general);
          
            return view;
        }
    }
}