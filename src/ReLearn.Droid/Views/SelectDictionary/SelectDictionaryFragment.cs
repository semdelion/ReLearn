﻿using Android.OS;
using Android.Runtime;
using Android.Support.Animation;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu.SelectDictionary;
using ReLearn.Droid.Views.Menu;
using System;
using System.Collections.Generic;

namespace ReLearn.Droid.Views.SelectDictionary
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("relearn.droid.views.selectdictionary.SelectDictionaryFragment")]
    public class SelectDictionaryFragment : BaseFragment<SelectDictionaryViewModel>
    {
        public static Dictionaries Dictionaries;

        protected override int FragmentId => Resource.Layout.fragment_menu_select_dictionary;

        protected override int Toolbar => Resource.Id.toolbar_select_dictionary;

        public static void SelectDictionaryClick(object sender, EventArgs e)
        {
            var image = sender as ImageView;
            Dictionaries.Selected($"{image.Tag}", $"{DataBase.TableName}");
            Enum.TryParse($"{image.Tag}", out TableNames name);
            DataBase.TableName = name;
            var Animation = new SpringAnimation(image, DynamicAnimation.Rotation, 0);
            Animation.Spring.SetStiffness(SpringForce.StiffnessMedium);
            Animation.SetStartVelocity(500);
            Animation.Start();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            Dictionaries = new Dictionaries((int) (Resources.DisplayMetrics.WidthPixels / 3f));
            var viewPager = view.FindViewById<ViewPager>(Resource.Id.pager);

            var fragments = new List<MvxViewPagerFragmentInfo>
            {
                new MvxViewPagerFragmentInfo("", "", typeof(TabLanguageFragment), typeof(DictionaryLanguageViewModel)),
                new MvxViewPagerFragmentInfo("", "", typeof(TabImageFragment), typeof(DictionaryImageViewModel))
            };
            viewPager.Adapter = new MvxFragmentStatePagerAdapter(Activity, ChildFragmentManager, fragments);
            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tablayout);
            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.GetTabAt(tabLayout.TabCount - 2).SetIcon(Resource.Drawable.ic_dictionary_languages);
            tabLayout.GetTabAt(tabLayout.TabCount - 1).SetIcon(Resource.Drawable.ic_dictionary_images);
            return view;
        }
    }
}