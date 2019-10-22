using Plugin.Settings;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Facade;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Languages
{
    public class ViewDictionaryViewModel : MvxViewDictionaryViewModel
    {
        #region Properties
        public string TextOk => this["Alert.Ok"];
        public string TextCancel => this["Alert.Cancel"];
        public string TextDelete => this["Alert.Delete"];
        public List<DatabaseWords> Database { get; private set; }

        public bool HideStudied
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.HideStudied}", true);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.HideStudied}", value);
        }
        #endregion

        #region Constructors
        public ViewDictionaryViewModel()
        {
            Task.Run(async () => Database = await DatabaseWords.GetData()).Wait();
        }
        #endregion
    }
}