using System;
using Android.App;
using MvvmCross;
using MvvmCross.Platforms.Android;
using ReLearn.Core;
using Android.Widget;

namespace ReLearn.Droid
{
    class MessageDroid : IMessageCore
    {
        public Android.App.Activity CurrentActivity => Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
        public void Dialog(string message, Action buttonAction)
        {
            CurrentActivity.RunOnUiThread(() => new AlertDialog.Builder(CurrentActivity).SetTitle("Error").SetMessage(message)
                .SetPositiveButton("Refresh", (sender, e) => { buttonAction(); })
                .SetNegativeButton("Close", (s, e) => { }).Show());
        }

        public void Toast(string message) => Android.Widget.Toast.MakeText(CurrentActivity, message, ToastLength.Long).Show();
    }
}
