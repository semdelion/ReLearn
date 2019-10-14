using Plugin.Settings;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Images
{
    public class ViewDictionaryViewModel : BaseViewModel
    {
        public ViewDictionaryViewModel()
        {
            Task.Run(async () => DataBase = await DatabaseImages.GetData()).Wait();
        }

        public List<DatabaseImages> DataBase { get; private set; }

        public bool HideStudied
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.HideStudied}", true);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.HideStudied}", value);
        }
    }
}