using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Telephony;
using MvvmCross;
using MvvmCross.Platforms.Android;
using ReLearn.Core.Provider;
using System;

namespace ReLearn.Droid.Implements
{
    public class InteractionProvider : IInteractionProvider
    {
        private static Activity TopActivity => Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public bool IsApplicationCallSupported()
        {
            try
            {
                var phoneService = (TelephonyManager)TopActivity.GetSystemService(Context.TelephonyService);
                return phoneService != null && (phoneService.SimState == SimState.Ready || TopActivity.PackageManager.HasSystemFeature(PackageManager.FeatureTelephony));
            }
            catch
            {
                return false;
            }
        }

        public void OpenCallApplication(string phoneNumber)
        {
            try
            {
                var intent = new Intent(Intent.ActionDial);
                intent.SetData(Android.Net.Uri.Parse($"tel:{phoneNumber}"));
                TopActivity.StartActivity(intent);
            }
            catch
            {
            }
        }
    }
}