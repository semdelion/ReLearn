﻿using Android.Content;
using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("relearn.droid.fragments.AboutUslFragment")]
    public class AboutUslFragment : BaseFragment<AboutUsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_about_us;

        protected override int Toolbar => Resource.Id.toolbar_About_Us;

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }
    }
}