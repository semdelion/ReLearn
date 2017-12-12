using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace ReLearn.Resources
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView textView_en { get; set; }
    }
    public class CustomAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Words> list;
        public CustomAdapter(Activity activity, List<Words> list)
        {
            this.activity = activity;
            this.list = list;
        }
        public override int Count
        {
            get
            {
                return list.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {

            return list[position].numberLearn;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.del_list, parent, false);
            var txtName = view.FindViewById<TextView>(Resource.Id.textView1);
            if (list[position].numberLearn > 10)
                txtName.SetBackgroundColor(Android.Graphics.Color.Argb(127, 50, 0, 0));
            //txtName.SetTextColor(Android.Graphics.Color.Red);         
            if (list[position].numberLearn <= 10)
                txtName.SetBackgroundColor(Android.Graphics.Color.Argb(127, 0, 50, 0));
            // txtName.SetTextColor(Android.Graphics.Color.Green);


            txtName.Text = list[position].enWords;
            return view;
        }
    }
}