using Android.Graphics;

namespace ReLearn.Droid.Statistics
{
    public class BaseStatistics
    {
        protected readonly float _height;
        protected readonly float _width;

        public BaseStatistics(Canvas canvas)
        {
            _width = canvas.Width;
            _height = canvas.Height;
            Canvas = canvas;
        }

        protected Canvas Canvas { get; set; }
    }
}