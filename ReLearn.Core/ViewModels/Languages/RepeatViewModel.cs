using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReLearn.API.Database;
using ReLearn.Core.Services;
using ReLearn.Core.ViewModels.Facade;
using ReLearn.Core.ViewModels.MainMenu.Statistics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Core.ViewModels.Languages
{
    public class RepeatViewModel : MvxRepeatViewModel<List<DatabaseWords>>
    {
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

        #region Services
        public ITextToSpeech TextToSpeech { get; }
        #endregion

        #region Constructors
        public RepeatViewModel(IMvxNavigationService navigationService, ITextToSpeech textToSpeech)
            :base(navigationService)
        {
            TextToSpeech = textToSpeech;
        }
        #endregion

        #region Private
        #endregion

        #region Protected
        #endregion

        #region Public
        public override void Prepare(List<DatabaseWords> parameter) => Database = parameter;
        #endregion
    }
}
