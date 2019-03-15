using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.Settings;
using ReLearn.API;
using ReLearn.API.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Images
{
    public class ViewDictionaryViewModel : MvxViewModel
    {
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

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public ViewDictionaryViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
            Task.Run(async () => DataBase = await DatabaseImages.GetData()).Wait();
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
