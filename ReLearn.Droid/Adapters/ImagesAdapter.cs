using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Droid.Adapters;
using System.Collections.Generic;

namespace ReLearn.Droid
{
    class CustomAdapterImage : BaseAdapter
    {
        private Activity CurrentActivity { get; set; }
        private List<DatabaseImages> List { get; set; }

        public CustomAdapterImage(Activity activity, List<DatabaseImages> list)
        {
            this.CurrentActivity = activity;
            this.List = list;
        }

        public override int Count => List.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return List[position].Image_name;
        }

        public override long GetItemId(int position)
        {
            return List[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? CurrentActivity.LayoutInflater.Inflate(Resource.Layout.item_image_view_dictionary, parent, false);
            var TextView = view.FindViewById<TextView>(Resource.Id.textView_item_view_dictionary);
            var ImageView = view.FindViewById<ImageView>(Resource.Id.imageView_item_view_dictionary);
            using (var his = Application.Context.Assets.Open($"Image{DataBase.TableName}Mini/{List[position].Image_name}.jpg"))
            {
                Bitmap bitmap = BitmapFactory.DecodeStream(his);
                ImageView.SetImageBitmap(bitmap);
            }
            BackgroundConstructor.SetColorForItems(List[position].NumberLearn, TextView);

            if (Settings.Currentlanguage == $"{ Language.en}")
                TextView.Text = List[position].Name_image_en;
            else
                TextView.Text = List[position].Name_image_ru;            
            return view;
        }
    }
}