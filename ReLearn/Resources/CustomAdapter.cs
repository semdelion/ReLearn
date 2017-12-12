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
            return list[position].enWords;
        }

        public override long GetItemId(int position)
        {
            return list[position].numberLearn;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.del_list, parent, false);
            var txtName = view.FindViewById<TextView>(Resource.Id.textView1);
            

            //switch (list[position].numberLearn / 5)
            //{
            //    case 4:
            //        {
            //            txtName.SetBackgroundColor(Android.Graphics.Color.Argb(20, 255, 0, 0));
            //            break;
            //        }
            //    case 3:
            //        {
            //            txtName.SetBackgroundColor(Android.Graphics.Color.Argb(20, 255, 105, 50));
            //            break;
            //        }
            //    case 2:
            //        {
            //            if (list[position].numberLearn % 5 == 0)
            //            { txtName.SetBackgroundColor(Android.Graphics.Color.Argb(20, 255, 255, 0)); }
            //            else { txtName.SetBackgroundColor(Android.Graphics.Color.Argb(20, 255, 152, 50)); }
            //            break;

            //        }
            //    case 1:
            //        {
            //            txtName.SetBackgroundColor(Android.Graphics.Color.Argb(20, 197, 255, 50));
            //            break;

            //        }
            //    case 0:
            //        {
            //            if (list[position].numberLearn % 5 == 0)
            //            { txtName.SetBackgroundColor(Android.Graphics.Color.Argb(20, 134, 48, 255)); }
            //            else { txtName.SetBackgroundColor(Android.Graphics.Color.Argb(20, 48, 255, 55)); }
            //            break;
            //        }
            //    default:
            //        { break; }

            //}
            //switch (list[position].numberLearn / 5)
            //{
            //    case 4:
            //        {
            //            txtName.SetTextColor(Android.Graphics.Color.Argb(150, 255, 0, 0));
            //            break;
            //        }
            //    case 3:
            //        {
            //            txtName.SetTextColor(Android.Graphics.Color.Argb(150, 255, 105, 50));
            //            break;
            //        }
            //    case 2:
            //        {
            //            if (list[position].numberLearn % 5 == 0)
            //            {
            //                txtName.SetTextColor(Android.Graphics.Color.Argb(150, 255, 255, 0));
            //            }
            //            else { txtName.SetTextColor(Android.Graphics.Color.Argb(150, 255, 152, 50)); }
            //            break;

            //        }
            //    case 1:
            //        {
            //            txtName.SetTextColor(Android.Graphics.Color.Argb(150, 197, 255, 50));
            //            break;

            //        }
            //    case 0:
            //        {
            //            if (list[position].numberLearn % 5 == 0)
            //            {
            //                txtName.SetTextColor(Android.Graphics.Color.Argb(150, 134, 48, 255));
            //            }
            //            else { txtName.SetTextColor(Android.Graphics.Color.Argb(150, 48, 255, 55)); }
            //            break;
            //        }
            //    default:
            //        {
            //            break;
            //        }
            //}

            switch (list[position].numberLearn / 5)
            {
                case 4:
                    {
                        txtName.SetTextColor(Android.Graphics.Color.Argb(170, 255, 0, 0));
                        txtName.SetBackgroundColor(Android.Graphics.Color.Argb(2, 255, 0, 0));
                        break;
                    }
                case 3:
                    {
                        txtName.SetTextColor(Android.Graphics.Color.Argb(170, 255, 105, 50));
                        txtName.SetBackgroundColor(Android.Graphics.Color.Argb(2, 255, 105, 50));
                        break;
                    }
                case 2:
                    {
                        if (list[position].numberLearn % 5 == 0)
                        {
                            txtName.SetTextColor(Android.Graphics.Color.Argb(170, 255, 255, 0));
                            txtName.SetBackgroundColor(Android.Graphics.Color.Argb(2, 255, 255, 0));
                        }
                        else
                        {
                            txtName.SetTextColor(Android.Graphics.Color.Argb(170, 255, 152, 50));
                            txtName.SetBackgroundColor(Android.Graphics.Color.Argb(2, 255, 152, 50));
                        }
                        break;

                    }
                case 1:
                    {
                        txtName.SetTextColor(Android.Graphics.Color.Argb(170, 197, 255, 50));
                        txtName.SetBackgroundColor(Android.Graphics.Color.Argb(2, 197, 255, 50));
                        break;
                    }
                case 0:
                    {
                        if (list[position].numberLearn % 5 == 0)
                        {
                            txtName.SetTextColor(Android.Graphics.Color.Argb(170, 134, 48, 255));
                            txtName.SetBackgroundColor(Android.Graphics.Color.Argb(2, 134, 48, 255));
                        }
                        else
                        {
                            txtName.SetTextColor(Android.Graphics.Color.Argb(170, 48, 255, 55));
                            txtName.SetBackgroundColor(Android.Graphics.Color.Argb(2, 48, 255, 55));
                        }
                        break;
                    }
                default:
                    { break; }

            }

            txtName.Text = list[position].enWords +"  -  "+ list[position].ruWords;
            return view;
        }
    }
}