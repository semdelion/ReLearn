using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.Core;

namespace ReLearn.Droid
{
    public class Setup : MvxAppCompatSetup<Core.App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.IoCProvider.RegisterType<IMessage>(() => new MessageDroid());
        }
    }
}
