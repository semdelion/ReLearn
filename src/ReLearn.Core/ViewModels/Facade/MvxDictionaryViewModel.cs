using ReLearn.Core.ViewModels.Base;

namespace ReLearn.Core.ViewModels.Facade
{
    public abstract class MvxDictionaryViewModel : BaseViewModel
    {
        private const string _names = "Names";

        private const string _dictionaries = "Dictionaries";

        public virtual string GetNameDictionary(string dictionary) => this[$"{_names}.{dictionary}"];
        
        public virtual string GetDescriptionDictionary(string dictionary) => this[$"{_dictionaries}.{dictionary}"];
    }
}