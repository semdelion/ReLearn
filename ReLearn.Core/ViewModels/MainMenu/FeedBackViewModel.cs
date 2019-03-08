using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;
using MvvmCross.Plugin.Email;
using Acr.UserDialogs;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class FeedbackViewModel : MvxViewModel
    {
        #region Fields
        #endregion

        #region Commands
        public IMvxCommand SendEmailCommand => new MvxCommand(ComposeEmail);
        #endregion

        #region Properties
        private string _message = "";
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        #endregion

        #region Services
        private IMvxNavigationService NavigationService { get; }
        #endregion

        #region Constructors
        public FeedbackViewModel(IMvxNavigationService mvxNavigationService)
        {
            NavigationService = mvxNavigationService;
        }
        #endregion

        #region Private
        #endregion

        #region Protected
        protected void ComposeEmail()
        {
            if (Message == "")
                Mvx.IoCProvider.Resolve<IUserDialogs>().Toast(new ToastConfig("Enter message!"));
            else
            {
                Mvx.IoCProvider.Resolve<IMvxComposeEmailTask>().ComposeEmail("SemdelionTeam@gmail.com", subject: "Hello, SemdelionTeam!", body: Message);
                Message = "";
            }
        }
        #endregion

        #region Public
        #endregion
    }
}
