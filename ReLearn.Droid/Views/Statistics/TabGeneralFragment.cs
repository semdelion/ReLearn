using Android.OS;
using Android.Views;
using ReLearn.API.Database;
using ReLearn.Droid.Views.Fragments;

namespace ReLearn.Droid.Views.Statistics
{
    public class TabGeneralFragment : Android.Support.V4.App.Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return new GraphGeneralStatistics(inflater.Inflate(Resource.Layout.fragment_tab_statistics_general, container, false).Context,
                StatisticsFragment.LightColor, 
                StatisticsFragment.DarkColor, 
                StatisticsFragment.StatisticsDB,
                StatisticsFragment.DataTupe, 
                DataBase.TableName.ToString());
        }
    }
}

  

   
