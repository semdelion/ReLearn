using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.WebBrowser;
using MvvmCross.ViewModels;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class AboutUsViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        public IMvxCommand SupportProjectCommand => new MvxCommand(SupportProject);
        #endregion

        #region Properties
        #endregion

        #region Services
        protected IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public AboutUsViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Private
        private void SupportProject() => Mvx.IoCProvider.Resolve<IMvxWebBrowserTask>().ShowWebPage("http://www.donationalerts.ru/r/semdelionteam");
        #endregion

        #region Protected
        #endregion

        #region Public
        public override void ViewCreated()
        {
            base.ViewCreated();
        }
        #endregion
    }
}
