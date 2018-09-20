using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Plugin.Settings;

namespace ReLearn
{
    class CustomAdapter_ImageText : BaseAdapter
    {
        private Activity activity;
        private List<Database_images> list;

        public CustomAdapter_ImageText(Activity activity, List<Database_images> list)
        {
            this.activity = activity;
            this.list = list;
        }

        public override int Count => list.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return list[position].Image_name;
        }

        public override long GetItemId(int position)
        {
            return list[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.item_view_dictionary_image, parent, false);
            var TextView = view.FindViewById<TextView>(Resource.Id.textView_item_view_dictionary);
            var ImageView = view.FindViewById<ImageView>(Resource.Id.imageView_item_view_dictionary);

            var his = Application.Context.Assets.Open("ImageFlags/" + list[position].Image_name + ".png");
            Bitmap bitmap = BitmapFactory.DecodeStream(his);
            ImageView.SetImageBitmap(bitmap);

            Additional_functions.SetColorForItems(list[position].NumberLearn, TextView);

            if (CrossSettings.Current.GetValueOrDefault("Language", null)=="en")
                TextView.Text = list[position].Name_image_en;
            else
                TextView.Text = list[position].Name_image_ru;            
            return view;
        }
    }
}