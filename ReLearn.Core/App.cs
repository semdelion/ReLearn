using MvvmCross.ViewModels;
using ReLearn.Core.ViewModels;

namespace ReLearn.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<MainViewModel>();
        }
    }
}