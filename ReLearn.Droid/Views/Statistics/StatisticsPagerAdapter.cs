using System;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Runtime;
using MvvmCross.Droid.Support.V7.AppCompat;
using Java.Lang;


namespace ReLearn.Droid.Views.Statistics
{
    class StatisticsPagerAdapter : FragmentPagerAdapter
    {
        public StatisticsPagerAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm) { }

        public override int Count
        {
            get => 2;
        }
        public override Android.Support.V4.App.Fragment GetItem(int position)
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