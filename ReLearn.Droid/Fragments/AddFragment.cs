using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("relearn.droid.fragments.AddFragment")]
    public class AddFragment : BaseFragment<AddViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_add;

        protected override int Toolbar => Resource.Id.toolbarEnglishAdd;

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu_english_add, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.dictionary_replenishment:
                    ViewModel.ToDictionaryReplenishment.Execute();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}

//[Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
//public class AddActivity : MvxAppCompatActivity<AddViewModel>
//{
//    protected override void OnCreate(Bundle savedInstanceState)
//    {
//        base.OnCreate(savedInstanceState);
//        SetContentView(Resource.Layout.languages_add_activity);
//        var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEnglishAdd);
//        SetSupportActionBar(toolbarMain);
//        SupportActionBar.SetDisplayHomeAsUpEnabled(true);
//    }

//    public override bool OnPrepareOptionsMenu(IMenu menu)
//    {
//        MenuInflater.Inflate(Resource.Menu.menu_english_add, menu);
//        return base.OnPrepareOptionsMenu(menu);
//    }

//    public override bool OnOptionsItemSelected(IMenuItem item)
//    {
//        switch (item.ItemId)
//        {
//            case Resource.Id.dictionary_replenishment:
//                ViewModel.ToDictionaryReplenishment.Execute();
//                return true;
//            case Android.Resource.Id.Home:
//                Finish();
//                return true;
//            default:
//                return base.OnOptionsItemSelected(item);
//        }
//    }
//}