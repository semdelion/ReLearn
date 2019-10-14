using ReLearn.API.Database;
using ReLearn.Core.Services;
using ReLearn.Core.ViewModels.Facade;
using System.Collections.Generic;

namespace ReLearn.Core.ViewModels.Languages
{
    public class RepeatViewModel : MvxRepeatViewModel<List<DatabaseWords>>
    {
        #region Fields
        private string _text;
        #endregion

        #region Properties
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public List<DatabaseWords> Database { get; set; }

        public string Word { get; set; }
        #endregion

        #region Services
        public ITextToSpeech TextToSpeech { get; }
        #endregion

        #region Constructors
        public RepeatViewModel(ITextToSpeech textToSpeech) : base()
        {
            TextToSpeech = textToSpeech;
        }
        #endregion

        #region Public
        public override void Prepare(List<DatabaseWords> parameter)
        {
            Database = parameter;
        }
        #endregion
    }
}