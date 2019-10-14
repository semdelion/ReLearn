using ReLearn.Core.ViewModels.Base;

namespace ReLearn.Core.ViewModels.Facade
{
    public abstract class MvxDictionaryViewModel : BaseViewModel
    {
        #region Fields
        private const string _names = "Names";

        private const string _dictionaries = "Dictionaries";
        #endregion

        #region Properties
        public virtual string GetNameDictionary(string dictionary) => this[$"{_names}.{dictionary}"];

        public virtual string GetDescriptionDictionary(string dictionary) => this[$"{_dictionaries}.{dictionary}"];
        #endregion
    }
}