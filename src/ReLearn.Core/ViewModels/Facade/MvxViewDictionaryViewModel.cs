using ReLearn.Core.ViewModels.Base;

namespace ReLearn.Core.ViewModels.Facade
{
    public abstract class MvxViewDictionaryViewModel: BaseViewModel
    {
        #region Properties
        public virtual string TextSortDescending => this["Menu.Texts.SortDescending"];
        public virtual string TextSortAscending => this["Menu.Texts.SortAscending"];
        public virtual string TextSortAlphabetically => this["Menu.Texts.SortAlphabetically"];
        public virtual string TextHideStudied => this["Menu.Texts.HideStudied"];
        #endregion
    }
}
