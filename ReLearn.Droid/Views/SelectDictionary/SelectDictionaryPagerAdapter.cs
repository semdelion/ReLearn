using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace ReLearn.Droid.Views.SelectDictionary
{
    class SelectDictionaryPagerAdapter : FragmentPagerAdapter
    {
        public SelectDictionaryPagerAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm) { }

        public override int Count
        {
            get => 2;
        }
        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0: return new TabLanguageFragment();
                case 1: return new TabImageFragment();
            }
            return null;
        }
    }
}