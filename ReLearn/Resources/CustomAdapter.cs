using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace ReLearn.Resources
{
    public class CustomAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Database_Words> list;

        public CustomAdapter(Activity activity, List<Database_Words> list)
        {
            this.activity = activity;
            this.list = list;
        }

        public override int Count => list.Count;
        
        public override Java.Lang.Object GetItem(int position) => list[position].Word;
        
        public override long GetItemId(int position) => list[position].NumberLearn;


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.item_view_dictionary_word, parent, false);
            var TView = view.FindViewById<TextView>(Resource.Id.item_view_dictionary);

            Additional_functions.SetColorForItems(list[position].NumberLearn, TView);
         
            TView.Text = list[position].Word +"  -  "+ list[position].TranslationWord;
            return view;
        }
    }
}