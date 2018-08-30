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

        public override int Count
        {
            get { return list.Count;}
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return list[position].Word;
        }

        public override long GetItemId(int position)
        {
            return list[position].NumberLearn;
        }
        void Color_TextView(TextView TV, Color color)
        {
            int TrText = 170, // прозрачность текста и фона
                TrBack = 10;
            TV.SetTextColor(Color.Argb(TrText, color.R, color.G, color.B));
            TV.SetBackgroundColor(Color.Argb(TrBack, color.R, color.G, color.B));
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.del_list, parent, false);
            var TView = view.FindViewById<TextView>(Resource.Id.textView1);
            
            switch (list[position].NumberLearn / 3)
            {
                case 4:
                    {
                        Color_TextView(TView, new Color( 255, 0, 0));
                        break;
                    }
                case 3:
                    {
                        Color_TextView(TView, new Color(255, 105, 50));
                        break;
                    }
                case 2:
                    {
                        if (list[position].NumberLearn % 3 == 0)
                            Color_TextView(TView, new Color(238, 252, 255));
                        else
                            Color_TextView(TView, new Color(255, 152, 50));
                        break;

                    }
                case 1:
                    {
                        Color_TextView(TView, new Color(197, 255, 50));
                        break;
                    }
                case 0:
                    {
                        if (list[position].NumberLearn % 3 == 0)
                            Color_TextView(TView, new Color(134, 48, 255));
                        else
                            Color_TextView(TView, new Color(48, 255, 55));
                        break;
                    }
                default:
                    break; 

            }
            TView.Text = list[position].Word +"  -  "+ list[position].TranslationWord;
            return view;
        }
    }
}