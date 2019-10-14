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
        #region Properties
        public bool HideStudied
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.HideStudied}", true);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.HideStudied}", value);
        }

        public List<DatabaseImages> DataBase { get; private set; }
        #endregion

        #region Constructors
        public ViewDictionaryViewModel()
        {
            Task.Run(async () => DataBase = await DatabaseImages.GetData()).Wait();
        }
        #endregion
    }
}