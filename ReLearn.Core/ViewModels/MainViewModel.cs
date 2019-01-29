using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.MainMenu;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);
        #endregion

        #region Properties

        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; set; }

        #endregion

        #region Constructors
        public MainViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Private
       
        #endregion

        #region Protected
        #endregion

        #region Public
        public override void ViewCreated()
        {
            base.ViewCreated();
        }
        public void ShowMenu() => NavigationService.Navigate<MenuViewModel>();
        
        #endregion
    }
}

