using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.WebBrowser;
using MvvmCross.ViewModels;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class AboutUsViewModel : BaseViewModel
    {
        #region Constructors
        #endregion

        #region Commands

        public IMvxCommand SupportProjectCommand => new MvxCommand(SupportProject);

        #endregion

        #region Services
        #endregion

        #region Private

        private void SupportProject()
        {
            Mvx.IoCProvider.Resolve<IMvxWebBrowserTask>().ShowWebPage("http://www.donationalerts.ru/r/semdelionteam");
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Protected

        #endregion

        #region Public

        #endregion
    }
}