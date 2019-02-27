using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Localization;
using ReLearn.Core.Services;
using ReLearn.Droid.Services;

namespace ReLearn.Droid
{
    public class Setup : MvxAppCompatSetup<Core.App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.IoCProvider.RegisterType<IMessageCore>(() => new MessageDroid());
            Mvx.IoCProvider.RegisterType<ITextToSpeech>(() => new TextToSpeech());
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("Language", new MvxLanguageConverter());
        }
    }
}
