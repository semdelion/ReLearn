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

namespace ReLearn.Droid.Images
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LearnActivity : MvxAppCompatActivity<LearnViewModel>
    {
        int Count { get; set; }
        List<DBImages> ImagesDatabase { get; set; }

        Bitmap ImageViewBox
        {
            set => FindViewById<ImageView>(Resource.Id.imageView_Images_learn).SetImageBitmap(value);
        }

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
                {
                    using (ImageViewBox = BitmapFactory.DecodeStream(Application.Context.Assets.Open(
                            $"Image{DataBase.TableNameImage}/{ImagesDatabase[Count].Image_name}.png")))
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
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ImagesLearnActivity);
            SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarImagesLearn));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ImagesDatabase = DBImages.GetDataNotLearned;
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

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
}