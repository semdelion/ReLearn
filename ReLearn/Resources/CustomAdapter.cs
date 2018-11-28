﻿using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace ReLearn.Resources
{
    public class CustomAdapter : BaseAdapter
    {
        private Activity activity;
        private List<DBWords> list;

        public CustomAdapter(Activity activity, List<DBWords> list)
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

            AdditionalFunctions.SetColorForItems(list[position].NumberLearn, TView);
         
            TView.Text = $"{list[position].Word}  -  {list[position].TranslationWord}";
            return view;
        }
    }
}