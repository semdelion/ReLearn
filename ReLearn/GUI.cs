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
using System.Threading;

namespace ReLearn
{
    class GUI:Activity
    {
        public static Context Res;
        public static void NewTouch(object sender, View.TouchEventArgs e)
        {
            Button button = (Button)sender;           
            var handled = false;
            if (e.Event.Action == MotionEventActions.Down)
                button.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.button_touch));
            else if (e.Event.Action == MotionEventActions.Up)
            {
                //button.Click += delegate
                //{
                //    Thread.Sleep(100);
                //    GUI.button_default(button);
                //};
                button.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
            }
            e.Handled = handled;
        }

        public static void TouchAdd(object sender, View.TouchEventArgs e)
        {
            Button button = (Button)sender;
            var handled = false;
            if (e.Event.Action == MotionEventActions.Down)
                button.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.buttonAfterClicking));
            else if (e.Event.Action == MotionEventActions.Up)
                button.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
            e.Handled = handled;
        }

        public static void button_click(Button B)
        {
            B.SetBackgroundDrawable(Res.Resources.GetDrawable(Resource.Drawable.buttonAfterClicking));
        }
        public static void button_default(Button B)
        {
            B.SetBackgroundDrawable(Res.Resources.GetDrawable(Resource.Drawable.buttonBeforeClicking));
        }
        public static void button_true(Button B)
        {
            B.SetBackgroundDrawable(Res.Resources.GetDrawable(Resource.Drawable.button_true));
        }
        public static void button_false(Button B)
        {
            B.SetBackgroundDrawable(Res.Resources.GetDrawable(Resource.Drawable.button_false));
        }
        public static void button_ebabled(Button B)
        {
            B.SetBackgroundDrawable(Res.Resources.GetDrawable(Resource.Drawable.button_enabled));
            B.Enabled = false;
        }
        
        public static void Button_enable(Button B1, Button B2, Button B3, Button B4, Button BNext) // отключаем кнопки для того, что бы нельзя было выбрать 2 ответ.
        {
            B1.Enabled = false;
            B1.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.button_enabled));
            B2.Enabled = false;
            B2.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.button_enabled));
            B3.Enabled = false;
            B3.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.button_enabled));
            B4.Enabled = false;
            B4.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.button_enabled));
            BNext.Enabled = true;
            BNext.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
        }
        public static void Button_Refresh(Button B1, Button B2, Button B3, Button B4, Button BNext) // обновляем кнопки для нового теста.
        {
            B1.Enabled = true; B2.Enabled = true; B3.Enabled = true; B4.Enabled = true;
            B1.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
            B2.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
            B3.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
            B4.SetBackgroundDrawable(Res.GetDrawable(Resource.Drawable.buttonBeforeClicking));
        }
        
    }
}