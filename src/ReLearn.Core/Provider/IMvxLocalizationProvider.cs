using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace ReLearn.Core.Provider
{
    public interface IMvxLocalizationProvider
    {
        CultureInfo CurrentCultureInfo { get; }

        Task ChangeLocale(CultureInfo cultureInfo);

        IEnumerable<CultureInfo> GetAvailableCultures();

        string GetText(string key);
    }
}
