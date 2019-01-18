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

namespace ReLearn.Droid
{
    enum StateButton
    {
        Next,
        Unknown
    }

    class ButtonNext
    {
        public StateButton State { get; set; }
        public Button button = null;
    }
}