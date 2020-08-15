using Android.App;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Droid.Views.Facade
{
    public enum StateButton
    {
        Next,
        Unknown
    }

    public class ButtonNext
    {
        public Button button = null;
        public StateButton State { get; set; }
    }

    public abstract class MvxAppCompatActivityRepeat<ViewModel> : MvxActivity<ViewModel>
        where ViewModel : class, IMvxViewModel
    {
        protected readonly float _displayWidth = Application.Context.Resources.DisplayMetrics.WidthPixels;

        protected List<Button> Buttons { get; set; }
        protected ButtonNext ButtonNext { get; set; }

        protected abstract void ButtonEnable(bool state);

        protected abstract Task Answer(params Button[] buttons);

        protected abstract void RandomButton(params Button[] buttons);

        protected abstract void NextTest();

        protected abstract Task Unknown();

    }
}