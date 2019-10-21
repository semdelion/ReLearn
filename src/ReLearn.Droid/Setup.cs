using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.IoC;
using MvvmCross.Localization;
using MvvmCross.ViewModels;
using ReLearn.Core;
using ReLearn.Core.Provider;
using ReLearn.Core.Services;
using ReLearn.Droid.Implements;
using ReLearn.Droid.Services;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace ReLearn.Droid
{
    public class Setup : MvxAppCompatSetup<Core.App>
    {
        private readonly App _app = new App();
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.IoCProvider.RegisterType<IMessageCore>(() => new MessageDroid());
            Mvx.IoCProvider.RegisterType<ITextToSpeech>(() => new TextToSpeech());
            Mvx.IoCProvider.RegisterSingleton<INavigatiomViewUpdater>(() => new NavigatiomViewUpdater());
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("Language", new MvxLanguageConverter());
        }
        protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
        {
            typeof(MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerView).Assembly
        };

        public override void InitializeSecondary()
        {
            base.InitializeSecondary();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IInteractionProvider, InteractionProvider>();
            _app.InitializeCultureInfo(new CultureInfo(API.Settings.Currentlanguage));
        }
        protected override IMvxApplication CreateApp() => _app;
    }
}