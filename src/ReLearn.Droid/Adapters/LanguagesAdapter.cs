using Android.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using ReLearn.API.Database;
using System.Collections.Generic;

namespace ReLearn.Droid.Adapters
{
    public class CustomAdapterWord : BaseAdapter
    {
        public CustomAdapterWord(Activity activity, List<DatabaseWords> list)
        {
            CurrentActivity = activity;
            List = list;
        }

        private Activity CurrentActivity { get; }
        private List<DatabaseWords> List { get; }

        public override int Count => List.Count;

        public override Object GetItem(int position)
        {
            return List[position].Word;
        }

        public override long GetItemId(int position)
        {
            return List[position].NumberLearn;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ??
                       CurrentActivity.LayoutInflater.Inflate(Resource.Layout.item_languages_view_dictionary, parent,
                           false);
            var textView = view.FindViewById<TextView>(Resource.Id.item_view_dictionary);

            BackgroundConstructor.SetColorForItems(List[position].NumberLearn, textView);

            textView.Text = $"{List[position].Word}  -  {List[position].TranslationWord}";
            return view;
        }
    }
}