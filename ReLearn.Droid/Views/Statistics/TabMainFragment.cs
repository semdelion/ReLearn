using Android.OS;
using Android.Views;
using ReLearn.API.Database;
using ReLearn.Droid.Views.Fragments;

namespace ReLearn.Droid.Views.Statistics
{
    public class TabMainFragment : Android.Support.V4.App.Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return new GraphStatistics(inflater.Inflate(Resource.Layout.fragment_tab_statistics_main, container, false).Context,
                                                        StatisticsFragment.LightColor, 
                                                        StatisticsFragment.DarkColor, 
                                                        DataBase.TableName.ToString());
        }
    }
}