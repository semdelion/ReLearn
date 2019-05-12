using System.Collections.Generic;
using MvvmCross.Navigation;
using ReLearn.API.Database;
using ReLearn.Core.Services;
using ReLearn.Core.ViewModels.Facade;

namespace ReLearn.Core.ViewModels.Languages
{
    public class RepeatViewModel : MvxRepeatViewModel<List<DatabaseWords>>
    {
        #region Constructors

        public RepeatViewModel(IMvxNavigationService navigationService, ITextToSpeech textToSpeech)
            : base(navigationService)
        {
            TextToSpeech = textToSpeech;
        }

        #endregion

        #region Services

        public ITextToSpeech TextToSpeech { get; }

        #endregion

        #region Public

        public override void Prepare(List<DatabaseWords> parameter)
        {
            Database = parameter;
        }

        #endregion

        #region Fields

        #endregion

        #region Commands

        #endregion

        #region Properties

        public List<DatabaseWords> Database { get; set; }
        private string _text;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public string Word { get; set; }

        #endregion

        #region Private

        #endregion

        #region Protected

        #endregion
    }
}