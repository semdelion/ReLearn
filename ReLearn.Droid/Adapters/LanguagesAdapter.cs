using Android.App;
using Android.Views;
using Android.Widget;
using ReLearn.API.Database;
using ReLearn.Droid.Adapters;
using System.Collections.Generic;

namespace ReLearn.Droid.Resources
{
    public class CustomAdapterWord : BaseAdapter
    {
        private Activity CurrentActivity { get; set; }
        private List<DatabaseWords> List { get; set; }

        public CustomAdapterWord(Activity activity, List<DatabaseWords> list)
        {
            this.CurrentActivity = activity;
            this.List = list;
        }

        public override int Count => List.Count;
        
        public override Java.Lang.Object GetItem(int position) => List[position].Word;
        
        public override long GetItemId(int position) => List[position].NumberLearn;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? CurrentActivity.LayoutInflater.Inflate(Resource.Layout.item_languages_view_dictionary, parent, false);
            var textView = view.FindViewById<TextView>(Resource.Id.item_view_dictionary);

            BackgroundConstructor.SetColorForItems(List[position].NumberLearn, textView);
         
            textView.Text = $"{List[position].Word}  -  {List[position].TranslationWord}";
            return view;
        }
    }
}