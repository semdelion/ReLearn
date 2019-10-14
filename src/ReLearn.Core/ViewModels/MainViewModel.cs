using ReLearn.Core.ViewModels.Base;
using ReLearn.Core.ViewModels.MainMenu;

namespace ReLearn.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Public
        public void ShowMenu()
        {
            NavigationService.Navigate<MenuViewModel>();
        }
        #endregion
    }
}