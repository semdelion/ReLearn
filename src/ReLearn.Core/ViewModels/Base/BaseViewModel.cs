using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;

namespace ReLearn.Core.ViewModels.Base
{
    public abstract class BaseViewModel : MvxViewModel, IMvxLocalizedTextSourceOwner
    {
        #region Fields
        private IMvxLanguageBinder _localizedTextSource;

        private readonly Lazy<IUserDialogs> _userDialogsLazy =
           new Lazy<IUserDialogs>(Mvx.IoCProvider.Resolve<IUserDialogs>);

        private readonly Lazy<IMvxNavigationService> _navigationServiceLazy =
            new Lazy<IMvxNavigationService>(Mvx.IoCProvider.Resolve<IMvxNavigationService>);
        #endregion

        #region Properties
        public virtual string Title => this["Title"];

        public virtual IMvxLanguageBinder LocalizedTextSource =>
            _localizedTextSource ?? (_localizedTextSource = new MvxLanguageBinder("", GetType().Name));

        protected IUserDialogs UserDialogs => _userDialogsLazy.Value;

        protected IMvxNavigationService NavigationService => _navigationServiceLazy.Value;

        public string this[string localizeKey] => TryLocalize(localizeKey);

        public void ShowLoading() => UserDialogs.ShowLoading();

        public void HideLoading() => UserDialogs.HideLoading();
        #endregion

        #region Private
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
        #endregion
    }

    public abstract class BaseViewModel<TParameter> : BaseViewModel, IMvxViewModel<TParameter>, IMvxViewModel
    {
        public abstract void Prepare(TParameter parameter);
    }
}
