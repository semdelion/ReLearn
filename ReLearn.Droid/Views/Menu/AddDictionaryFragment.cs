using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Droid.Views.Menu
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("relearn.droid.views.menu.AddDictionaryFragments")]
    public class AddDictionaryFragments : BaseFragment<AddDictionaryViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_add_dictionary;

        protected override int Toolbar => Resource.Id.toolbarDictionaryReplenishment;

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnPrepareOptionsMenu(menu);
            inflater.Inflate(Resource.Menu.menu_DictionaryReplenishment, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.dictionary_replenishment_instruction:
                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(ParentActivity);
                    LayoutInflater factory = LayoutInflater.From(ParentActivity);
                    alert.SetView(factory.Inflate(Resource.Layout.alert_dictionary, null));
                    alert.SetTitle(Resource.String.Instruction);
                    alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
                    alert.Show();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}