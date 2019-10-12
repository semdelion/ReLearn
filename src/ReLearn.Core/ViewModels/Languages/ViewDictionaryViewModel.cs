using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.Settings;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Base;

namespace ReLearn.Core.ViewModels.Languages
{
    public class ViewDictionaryViewModel : BaseViewModel
    {
        #region Constructors

        public ViewDictionaryViewModel()
        {
            Task.Run(async () => Database = await DatabaseWords.GetData()).Wait();
        }

        #endregion

        #region Services
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

        public string TextOk => this["Alert.Ok"];
        public string TextCancel => this["Alert.Cancel"];
        public string TextDelete => this["Alert.Delete"];

        #endregion

        #region Private

        #endregion

        #region Protected

        #endregion

        #region Public

        #endregion
    }
}