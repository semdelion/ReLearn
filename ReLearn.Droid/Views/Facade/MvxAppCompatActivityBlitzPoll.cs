using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Animation;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.ViewModels;

namespace ReLearn.Droid.Views.Facade
{
    public abstract class MvxAppCompatActivityBlitzPoll<ViewModel> : MvxAppCompatActivity<ViewModel> where ViewModel : class, IMvxViewModel
    {
        protected readonly float _displayWidth = Application.Context.Resources.DisplayMetrics.WidthPixels;
        protected readonly float _displayHeight = Application.Context.Resources.DisplayMetrics.HeightPixels;

        protected BitmapDrawable BackgroundWord { get; set; }
        protected float StartX { get; set; } = 0;

        public virtual void RunAnimation(View View, float distance)
        {
            FlingAnimation flingAnimation = new FlingAnimation(View, DynamicAnimation.TranslationX);
            flingAnimation.SetStartVelocity(distance);
            flingAnimation.SetFriction(2);
            flingAnimation.Start();
        }

        public virtual async void Swipes(object s, View.TouchEventArgs e)
        {
            var handled = false;
            if (e.Event.Action == MotionEventActions.Down)
            {
                StartX = e.Event.GetX();
                handled = true;
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                float movement = e.Event.GetX() - StartX;
                float offset = _displayWidth / 10;
                if (Math.Abs(movement) > offset)
                    if (movement < 0)
                        await Answer(false);
                    else
                        await Answer(true);
                handled = true;
            }
            e.Handled = handled;
        }
        public abstract Task Answer(bool UserAnswer);
    }
}