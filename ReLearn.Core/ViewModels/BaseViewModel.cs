using System;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ReLearn.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel, IMvxLocalizedTextSourceOwner
    {
        private IMvxLanguageBinder _localizedTextSource;

        public virtual IMvxLanguageBinder LocalizedTextSource =>
            _localizedTextSource ?? (_localizedTextSource = new MvxLanguageBinder("", this.GetType().Name));

        private readonly Lazy<IUserDialogs> _userDialogsLazy =
            new Lazy<IUserDialogs>(Mvx.IoCProvider.Resolve<IUserDialogs>);

        private readonly Lazy<IMvxNavigationService> navigationServiceLazy =
            new Lazy<IMvxNavigationService>(Mvx.IoCProvider.Resolve<IMvxNavigationService>);

        protected IUserDialogs UserDialogs => _userDialogsLazy.Value;

        protected IMvxNavigationService NavigationService => navigationServiceLazy.Value;

        public string this[string localizeKey] => TryLocalize(localizeKey);

        private string TryLocalize(string localizeKey)
        {
            try
            {
                return LocalizedTextSource.GetText(localizeKey);
            }
            catch
            {
                return "Key not found";
            }
        }

        public void ShowLoading() => UserDialogs.ShowLoading();

        public void HideLoading() => UserDialogs.HideLoading();
    }

    public abstract class BaseViewModel<TParameter> : BaseViewModel, IMvxViewModel<TParameter>, IMvxViewModel
    {
        public abstract void Prepare(TParameter parameter);
    }
}
