using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Converters;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Localization;
using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using ReLearn.Core.Services;
using ReLearn.Droid.Services;
using ReLearn.Core;
using ReLearn.Core.Provider;
using ReLearn.Droid.Implements;

namespace ReLearn.Droid
{
    public class Setup : MvxAppCompatSetup<Core.App>
    {
        private App _app = new App();
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