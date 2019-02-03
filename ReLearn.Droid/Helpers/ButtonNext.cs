using Android.Widget;

namespace ReLearn.Droid.Helpers
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