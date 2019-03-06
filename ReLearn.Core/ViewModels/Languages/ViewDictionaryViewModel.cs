using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Languages
{
    public class ViewDictionaryViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        #endregion

        #region Properties
        public List<DBWords> Database { get; private set; }
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public ViewDictionaryViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
          
        }
        #endregion

        #region Private
        #endregion

        #region Protected
        #endregion

        #region Public
        public override async Task Initialize() => Database = await DBWords.GetData();
        #endregion
    }
}