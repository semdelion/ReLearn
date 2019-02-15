using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Statistics;
using ReLearn.Droid.Views.Fragments;

namespace ReLearn.Droid.Views.Statistics
{
    public class TabGeneralFragment : MvxFragment<GeneralStatisticsViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return new GraphGeneralStatistics(inflater.Inflate( 
                Resource.Layout.fragment_tab_statistics_general, container, false).Context,
                StatisticsFragment.LightColor, 
                StatisticsFragment.DarkColor, 
                StatisticsFragment.StatisticsDB,
                StatisticsFragment.DataTupe, 
                DataBase.TableName.ToString()
                );
        }
    }
}

  

   
