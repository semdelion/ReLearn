using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.IoC;
using MvvmCross.Localization;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.ViewModels;
using ReLearn.Core;
using ReLearn.Core.Provider;
using ReLearn.Core.Services;
using ReLearn.Droid.Implements;
using ReLearn.Droid.Services;
using Serilog;
using Serilog.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;

namespace ReLearn.Droid
{
    public class Setup : MvxAndroidSetup<Core.App>
    {
        private readonly App _app = new App();
        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeFirstChance(iocProvider);
            Mvx.IoCProvider.RegisterType<IMessageCore>(() => new MessageDroid());
            Mvx.IoCProvider.RegisterType<ITextToSpeech>(() => new TextToSpeech());
            Mvx.IoCProvider.RegisterSingleton<INavigatiomViewUpdater>(() => new NavigatiomViewUpdater());
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("Language", new MvxLanguageConverter());
        }

        public override void InitializeSecondary()
        {
            base.InitializeSecondary();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IInteractionProvider, InteractionProvider>();
            _app.InitializeCultureInfo(new CultureInfo(API.Settings.Currentlanguage));
        }
        protected override IMvxApplication CreateApp(IMvxIoCProvider iocProvider) => _app;

        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
			Log.Logger = new LoggerConfiguration()
			   .MinimumLevel.Debug()
			   .WriteTo.AndroidLog()
			   .CreateLogger();

			return new SerilogLoggerFactory();
        }
    }
}