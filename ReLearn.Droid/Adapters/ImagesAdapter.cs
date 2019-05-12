using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Java.Lang;
using ReLearn.API;
using ReLearn.API.Database;
using System.Collections.Generic;

namespace ReLearn.Droid.Adapters
{
    internal class CustomAdapterImage : BaseAdapter
    {
        public CustomAdapterImage(Activity activity, List<DatabaseImages> list)
        {
            CurrentActivity = activity;
            List = list;
        }

        private Activity CurrentActivity { get; }
        private List<DatabaseImages> List { get; }

        public override int Count => List.Count;

        public override Object GetItem(int position)
        {
            return List[position].Image_name;
        }

        public override long GetItemId(int position)
        {
            return List[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ??
                       CurrentActivity.LayoutInflater.Inflate(Resource.Layout.item_image_view_dictionary, parent,
                           false);
            var TextView = view.FindViewById<TextView>(Resource.Id.textView_item_view_dictionary);
            var ImageView = view.FindViewById<ImageView>(Resource.Id.imageView_item_view_dictionary);
            using (var his =
                Application.Context.Assets.Open($"Image{DataBase.TableName}Mini/{List[position].Image_name}.jpg"))
            {
                var bitmap = BitmapFactory.DecodeStream(his);
                ImageView.SetImageBitmap(bitmap);
            }

            BackgroundConstructor.SetColorForItems(List[position].NumberLearn, TextView);

            if (Settings.Currentlanguage == $"{Language.en}")
                TextView.Text = List[position].Name_image_en;
            else
                TextView.Text = List[position].Name_image_ru;
            return view;
        }
    }
}