using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Email;
using MvvmCross.ViewModels;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class FeedbackViewModel : MvxViewModel
    {
        #region Constructors

        public FeedbackViewModel(IMvxNavigationService mvxNavigationService)
        {
            NavigationService = mvxNavigationService;
        }

        #endregion

        #region Commands

        public IMvxCommand SendEmailCommand => new MvxCommand(ComposeEmail);

        #endregion

        #region Services

        private IMvxNavigationService NavigationService { get; }

        #endregion

        #region Protected

        protected void ComposeEmail()
        {
            if (Message == "")
            {
                Mvx.IoCProvider.Resolve<IUserDialogs>().Toast(new ToastConfig("Enter message!"));
            }
            else
            {
                Mvx.IoCProvider.Resolve<IMvxComposeEmailTask>().ComposeEmail("SemdelionTeam@gmail.com",
                    subject: "Hello, SemdelionTeam!", body: Message);
                Message = "";
            }
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        private string _message = "";

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        #endregion

        #region Private

        #endregion

        #region Public

        #endregion
    }
}