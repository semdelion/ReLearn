using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Email;
using ReLearn.Core.ViewModels.Base;

namespace ReLearn.Core.ViewModels.MainMenu
{
    public class FeedbackViewModel : BaseViewModel
    {
        #region Fields
        private string _message = "";
        #endregion

        #region Commands
        public IMvxCommand SendEmailCommand => new MvxCommand(ComposeEmail);
        #endregion

        #region Properties
        public string TextSend => this["Buttons.Send"];

        public string TextMessageHint => this["Texts.MessageHint"];

        public string TextDescription => this["Texts.Description"];

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
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
    }
}