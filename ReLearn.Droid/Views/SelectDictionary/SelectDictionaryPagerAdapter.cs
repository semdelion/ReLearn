using Android.Support.V4.App;

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