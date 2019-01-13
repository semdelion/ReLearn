using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Support.V7.App;
using System.Collections.Generic;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core.ViewModels.Images;
using Android.Util;
using Android.Graphics.Drawables;
using static Android.Graphics.PorterDuff;
using ReLearn.API.Database;

namespace ReLearn.Droid.Images
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LearnActivity : MvxAppCompatActivity<LearnViewModel>
    {
        int Count { get; set; }
        List<DBImages> ImagesDatabase { get; set; }

        

        string ImageName
        {
            get => FindViewById<TextView>(Resource.Id.textView_Images_learn).Text; 
            set => FindViewById<TextView>(Resource.Id.textView_Images_learn).Text = value; 
        }
        
        [Java.Interop.Export("Button_Images_Learn_NotRepeat_Click")]
        public void Button_Images_Learn_NotRepeat_Click(View v)
        {
            DBImages.UpdateLearningNotRepeat(ImageName);
            Button_Images_Learn_Next_Click(null);
        }

        [Java.Interop.Export("Button_Images_Learn_Next_Click")]
        public void Button_Images_Learn_Next_Click(View v)
        {
            if (Count < ImagesDatabase.Count)
            {
                DBImages.UpdateLearningNext(ImagesDatabase[Count].Image_name);
                try
                {   using (var image = BitmapFactory.DecodeStream(Application.Context.Assets.Open( $"Image{DataBase.TableName}/{ImagesDatabase[Count].Image_name}.png")))
                    using (var ImageViewBox = BitmapHandler.GetRoundedCornerBitmap(image, AdditionalFunctions.DpToPX(5)))
                        FindViewById<ImageView>(Resource.Id.imageView_Images_learn).SetImageBitmap(ImageViewBox);
                        ImageName = ImagesDatabase[Count++].ImageName;
                }
                catch(Exception ex)
                {
                    Toast.MakeText(this, ex.Message , ToastLength.Short).Show();
                }
            }
            else
                Toast.MakeText(this, GetString(Resource.String.DictionaryOver), ToastLength.Short).Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ImagesLearnActivity);
            SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarImagesLearn));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ImagesDatabase = DBImages.GetDataNotLearned;
            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);
            var _background = new BitmapDrawable(Resources, Background.GetBackgroung(
                                displayMetrics.WidthPixels - AdditionalFunctions.DpToPX(20),
                                AdditionalFunctions.DpToPX(300)));
            FindViewById<LinearLayout>(Resource.Id.learn_background).Background = _background;

            if (ImagesDatabase.Count == 0)
            {
                Toast.MakeText(this, GetString(Resource.String.RepeatedAllImages), ToastLength.Short).Show();
                Finish();
                return;
            }
            Button_Images_Learn_Next_Click(null);
        }

        public override bool OnOptionsItemSelected(IMenuItem item) // button home
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}