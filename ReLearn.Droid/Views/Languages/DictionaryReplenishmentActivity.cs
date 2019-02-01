using Android.App;
using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels.Languages;

namespace ReLearn.Droid.Languages
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class DictionaryReplenishmentActivity : MvxAppCompatActivity<DictionaryReplenishmentViewModel>
    {                                          
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.languages_dictionary_replenishment_activity);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarDictionaryReplenishment);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }    

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_DictionaryReplenishment, menu);
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.dictionary_replenishment_instruction:
                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                    LayoutInflater factory = LayoutInflater.From(this);
                    alert.SetView(factory.Inflate(Resource.Layout.languages_alert_image_activity, null));
                    alert.SetTitle(Resource.String.Instruction);
                    alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
                    alert.Show();
                    return true;
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}