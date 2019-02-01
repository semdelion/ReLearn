using Android.OS;
using Android.Views;
using ReLearn.API.Database;
using ReLearn.Droid.Views.Fragments;

namespace ReLearn.Droid.Views.Statistics
{
    public class TabMainFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return new GraphStatistics(inflater.Inflate(Resource.Layout.statistics_main_fragment, container, false).Context,
                                                        StatisticsFragment.LightColor, StatisticsFragment.DarkColor, DataBase.TableName.ToString());
        }


    }
}