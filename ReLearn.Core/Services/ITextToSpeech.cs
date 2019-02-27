using System;
using System.Collections.Generic;
using System.Text;

namespace ReLearn.Core.Services
{
    public interface ITextToSpeech
    {
        void Speak(string text);
    }
}
