using MvvmCross.ViewModels;
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

        private MvxObservableCollection<DatabaseWords> _items = new MvxObservableCollection<DatabaseWords>();
        public MvxObservableCollection<DatabaseWords> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public bool HideStudied
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.HideStudied}", true);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.HideStudied}", value);
        }
        #endregion

        #region Constructors
        #endregion

        public override async void Prepare()
        {
            try
            {
                UserDialogs.ShowLoading();
                var database = await DatabaseWords.GetData();
                Items.AddRange(database);
            }
            finally
            {
                UserDialogs.HideLoading();
            }
        }
    }
}