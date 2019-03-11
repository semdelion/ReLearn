﻿using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Images;
using ReLearn.Droid.Helpers;
using System;
using System.Collections.Generic;

namespace ReLearn.Droid.Images
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LearnActivity : MvxAppCompatActivity<LearnViewModel>
    {
        [Java.Interop.Export("Button_Images_Learn_NotRepeat_Click")]
        public async void Button_Images_Learn_NotRepeat_Click(View v)
        {
            await DatabaseImages.UpdateLearningNotRepeat(ViewModel.ImageName);
            Button_Images_Learn_Next_Click(null);
        }

        [Java.Interop.Export("Button_Images_Learn_Next_Click")]
        public async void Button_Images_Learn_Next_Click(View v)
        {
            if (ViewModel.Count < ViewModel.Database.Count)
            {
                await DatabaseImages.UpdateLearningNext(ViewModel.Database[ViewModel.Count].Image_name);
                using (var image = BitmapFactory.DecodeStream(Application.Context.Assets.Open( $"Image{DataBase.TableName}/{ViewModel.Database[ViewModel.Count].Image_name}.png")))
                using (var ImageViewBox = BitmapHelper.GetRoundedCornerBitmap(image, PixelConverter.DpToPX(5)))
                    FindViewById<ImageView>(Resource.Id.imageView_Images_learn).SetImageBitmap(ImageViewBox);
                    ViewModel.ImageName = ViewModel.Database[ViewModel.Count++].ImageName;
            }
            else
                Toast.MakeText(this, GetString(Resource.String.DictionaryOver), ToastLength.Short).Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_images_learn);
            SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_images_learn));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);
            using(var background = BitmapHelper.GetBackgroung(Resources, displayMetrics.WidthPixels - PixelConverter.DpToPX(20), PixelConverter.DpToPX(300)))
                FindViewById<LinearLayout>(Resource.Id.learn_background).Background = background;
            Button_Images_Learn_Next_Click(null);
        }

        public override bool OnOptionsItemSelected(IMenuItem item) // button home
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}