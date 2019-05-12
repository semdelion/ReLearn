using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.Settings;
using ReLearn.API;
using ReLearn.API.Database;

namespace ReLearn.Core.ViewModels.Languages
{
    public class ViewDictionaryViewModel : MvxViewModel
    {
        #region Constructors

        public ViewDictionaryViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
            Task.Run(async () => Database = await DatabaseWords.GetData()).Wait();
        }

        #endregion

        #region Services

        protected IMvxNavigationService NavigationService { get; }

        #endregion

        #region Fields

        #endregion

        #region Commands

        #endregion

        #region Properties

        public List<DatabaseWords> Database { get; private set; }

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