using ReLearn.Core.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReLearn.Core.ViewModels.Facade
{
    public abstract class MvxDictionaryViewModel : BaseViewModel
    {
        private string _names = "Names";

        private string _dictionaries = "Dictionaries";

        public virtual string GetNameDictionary(string dictionary)
        {
            return this[$"{_names}.{dictionary}"];
        }
        public virtual string GetDescriptionDictionary(string dictionary)
        {
            return this[$"{_dictionaries}.{dictionary}"];
        }
    }
}