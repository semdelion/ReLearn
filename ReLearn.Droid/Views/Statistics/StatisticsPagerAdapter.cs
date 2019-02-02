using Android.Support.V4.App;


namespace ReLearn.Droid.Views.Statistics
{
    class StatisticsPagerAdapter : FragmentStatePagerAdapter
    {
        public StatisticsPagerAdapter(FragmentManager fm) : base(fm) { }

        public override int Count
        {
            get => 2;
        }
        public override Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0: return new TabMainFragment();
                case 1: return new TabGeneralFragment();
            }
            return null;
        }
    }
}