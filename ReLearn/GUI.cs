﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;

namespace ReLearn
{
    class GUI:Activity
    {
        public static Context Res;

        public static void Button_Touch(object sender, View.TouchEventArgs e)
        {
            Button button = (Button)sender;           
            var handled = false;
            if (e.Event.Action == MotionEventActions.Down)
                button.Background = (Res.GetDrawable(Resource.Drawable.button_touch));
            else if (e.Event.Action == MotionEventActions.Up)
                button.Background = (Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
            e.Handled = handled;
        }

        public static void Button_1_Click(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;
            button.Background = (Res.GetDrawable(Resource.Drawable.buttonAfterClicking));
        }

        public static void Button_Click(object sender, View.TouchEventArgs e)
        {
            Button button = (Button)sender;
            var handled = false;
            if (e.Event.Action == MotionEventActions.Down)
                button.Background = (Res.GetDrawable(Resource.Drawable.buttonAfterClicking));
            else if (e.Event.Action == MotionEventActions.Up)
                button.Background = (Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
            e.Handled = handled;
        }

        public static void Button_click(Button B)
        {
            B.Background = (Res.GetDrawable(Resource.Drawable.buttonAfterClicking));
        }

        public static void Button_default(Button B)
        {
            B.Background = (Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
        }

        public static void Button_true(Button B)
        {
            B.Background = (Res.GetDrawable(Resource.Drawable.button_true));
        }

        public static void Button_false(Button B)
        {
            B.Background = (Res.GetDrawable(Resource.Drawable.button_false));
        }

        public static void Button_ebabled(Button B)
        {
            B.Background = (Res.GetDrawable(Resource.Drawable.button_enabled));
            B.Enabled = false;
        }
        
        public static void Button_enable(Button B1, Button B2, Button B3, Button B4, Button BNext) // отключаем кнопки для того, что бы нельзя было выбрать 2 ответ.
        {
            B1.Enabled = false;
            B1.Background = (Res.GetDrawable(Resource.Drawable.button_enabled));
            B2.Enabled = false;
            B2.Background = (Res.GetDrawable(Resource.Drawable.button_enabled));
            B3.Enabled = false;
            B3.Background = (Res.GetDrawable(Resource.Drawable.button_enabled));
            B4.Enabled = false;
            B4.Background = (Res.GetDrawable(Resource.Drawable.button_enabled));
            BNext.Enabled = true;
            BNext.Background = (Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
        }

        public static void Button_Refresh(Button B1, Button B2, Button B3, Button B4, Button BNext) // обновляем кнопки для нового теста.
        {
            B1.Enabled = true; B2.Enabled = true; B3.Enabled = true; B4.Enabled = true;
            B1.Background = (Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
            B2.Background = (Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
            B3.Background = (Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
            B4.Background = (Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
        }
    }
}