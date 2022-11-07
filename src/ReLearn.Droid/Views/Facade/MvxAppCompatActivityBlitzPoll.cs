﻿using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.DynamicAnimation;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;

namespace ReLearn.Droid.Views.Facade
{
    public abstract class MvxAppCompatActivityBlitzPoll<ViewModel> : MvxActivity<ViewModel>
        where ViewModel : class, IMvxViewModel
    {
        protected readonly float _displayHeight = Application.Context.Resources.DisplayMetrics.HeightPixels;
        protected readonly float _displayWidth = Application.Context.Resources.DisplayMetrics.WidthPixels;

        protected BitmapDrawable BackgroundWord { get; set; }
        protected float StartX { get; set; }

        public virtual void RunAnimation(View View, float distance)
        {
            var flingAnimation = new FlingAnimation(View, DynamicAnimation.TranslationX);
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
                var movement = e.Event.GetX() - StartX;
                var offset = _displayWidth / 10;
                if (Math.Abs(movement) > offset)
                {
                    if (movement < 0)
                    {
                        await Answer(false);
                    }
                    else
                    {
                        await Answer(true);
                    }
                }

                handled = true;
            }

            e.Handled = handled;
        }

        public abstract Task Answer(bool UserAnswer);
    }
}