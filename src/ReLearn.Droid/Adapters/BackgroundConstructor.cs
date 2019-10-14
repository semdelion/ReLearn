using Android.Graphics;
using Android.Widget;
using ReLearn.API;
using ReLearn.Droid.Helpers;

namespace ReLearn.Droid.Adapters
{
    public static class BackgroundConstructor
    {
        private static void ColorTextView(TextView textView, Color color)
        {
            int TextTransparence = 225,
                BackgroundTransparence = 10;
            textView.SetTextColor(Color.Argb(TextTransparence, color.R, color.G, color.B));
            textView.SetBackgroundColor(Color.Argb(BackgroundTransparence, color.R, color.G, color.B));
        }

        public static void SetColorForItems(int degreeOfStudy, TextView textView)
        {
            if (degreeOfStudy == Settings.StandardNumberOfRepeats)
            {
                ColorTextView(textView, Colors.White);
            }
            else if (degreeOfStudy > Settings.StandardNumberOfRepeats)
            {
                ColorTextView(textView, new Color(230,
                    200 - (degreeOfStudy - Settings.StandardNumberOfRepeats) * 180 / Settings.StandardNumberOfRepeats,
                    20)); //  230, 20, 20   to   230, 200, 20
            }
            else
            {
                ColorTextView(textView,
                    new Color(20 + degreeOfStudy * 180 / (Settings.StandardNumberOfRepeats - 1), 230,
                        20)); //  180, 230, 20   to   20,  230, 20
            }
        }
    }
}