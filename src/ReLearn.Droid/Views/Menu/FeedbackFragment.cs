﻿using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid.Views.Menu
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("relearn.droid.views.menu.FeedbackFragment")]
    public class FeedbackFragment : BaseFragment<FeedbackViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_menu_feedback;

        protected override int Toolbar => Resource.Id.toolbar_feedback;
    }
}