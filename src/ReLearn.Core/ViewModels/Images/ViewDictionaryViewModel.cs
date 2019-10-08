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
        #region Constructors

        public ViewDictionaryViewModel()
        {
            Task.Run(async () => DataBase = await DatabaseImages.GetData()).Wait();
        }

        #endregion

        #region Services
        #endregion

        #region Fields

        #endregion

        #region Commands

        #endregion

        #region Properties

        public List<DatabaseImages> DataBase { get; private set; }

        public bool HideStudied
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.HideStudied}", true);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.HideStudied}", value);
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        #endregion

        #region Public

        #endregion
    }
}