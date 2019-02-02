using Android.App;
using Android.OS;
using Android.Support.Animation;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels;
using System;

namespace ReLearn.Droid.Views.SelectDictionary
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SelectDictionary : MvxAppCompatActivity<SelectDictionaryViewModel>
    {
        public static Dictionaries Dictionaries;
        public static void SelectDictionaryClick(object sender, EventArgs e)
        {
            ImageView ImgV = sender as ImageView;
            Dictionaries.Selected(ImgV.Tag.ToString(), DataBase.TableName.ToString());
            Enum.TryParse(ImgV.Tag.ToString(), out TableNames name);
            DataBase.TableName = name;
            var Animation = new SpringAnimation(ImgV, DynamicAnimation.Rotation, 0);
            Animation.Spring.SetStiffness(SpringForce.StiffnessMedium);
            Animation.SetStartVelocity(500);
            Animation.Start();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.fragment_select_dictionary);
            SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarSelectDictionary));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            Dictionaries = new Dictionaries((int)(Resources.DisplayMetrics.WidthPixels / 3f));
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.pager);
            SelectDictionaryPagerAdapter myPagerAdapter = new SelectDictionaryPagerAdapter(SupportFragmentManager);
            viewPager.Adapter = myPagerAdapter;
            TabLayout tabLayout = FindViewById<TabLayout>(Resource.Id.tablayout);
            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.GetTabAt(tabLayout.TabCount - 2).SetIcon(Resource.Drawable.icon_language);
            tabLayout.GetTabAt(tabLayout.TabCount - 1).SetIcon(Resource.Drawable.icon_image);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}
