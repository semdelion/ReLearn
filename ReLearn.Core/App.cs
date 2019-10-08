using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Localization;
using MvvmCross.Plugin.ResxLocalization;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.Localization;
using ReLearn.Core.Provider;
using ReLearn.Core.ViewModels;
using Xamarin.Yaml.Localization.Configs;
using Xamarin.Yaml.Parser;

namespace ReLearn.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.RegisterSingleton<IMvxTextProvider>(new MvxResxTextProvider(AppResources.ResourceManager));
            Mvx.IoCProvider.RegisterSingleton(() => UserDialogs.Instance);
            RegisterAppStart<MainViewModel>();
            DataBase.SetupConnection();
            Task.Run(async () =>
            {
                await DatabaseImages.UpdateData();
                await DatabaseWords.UpdateData();
            });

            var assemblyConfig = new AssemblyContentConfig(this.GetType().GetTypeInfo().Assembly)
            {
                ResourceFolder = "Locales",
                ParserConfig = new ParserConfig
                {
                    ThrowWhenKeyNotFound = true
                }
            };

            var textProvider = new MvxYamlTextProvider(assemblyConfig);

            Mvx.IoCProvider.RegisterSingleton<IMvxTextProvider>(textProvider);
            Mvx.IoCProvider.RegisterSingleton<IMvxLocalizationProvider>(textProvider);
        }
        public void InitializeCultureInfo(CultureInfo cultureInfo)
        {
            Mvx.IoCProvider.Resolve<IMvxLocalizationProvider>().ChangeLocale(cultureInfo).GetAwaiter().GetResult();
        }
    }
}