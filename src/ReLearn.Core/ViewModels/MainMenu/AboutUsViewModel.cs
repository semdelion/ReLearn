using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.WebBrowser;
using ReLearn.Core.ViewModels.Base;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class AboutUsViewModel : BaseViewModel
    {
        #region Commands
        public IMvxCommand SupportProjectCommand => new MvxCommand(SupportProject);
        #endregion

        #region Properties
        public string TextSupport => this["Buttons.Support"];

        public string TextMessage => this["Texts.Message"];
        #endregion

        #region Private
        private void SupportProject()
        {
            Mvx.IoCProvider.Resolve<IMvxWebBrowserTask>().ShowWebPage("http://www.donationalerts.ru/r/semdelionteam");
        }
        #endregion
    }
}