using System;
using System.Collections.Generic;
using System.Text;

namespace ReLearn.Core
{
    public interface IMessageCore
    {
        void Dialog(string message, Action buttonAction);
        void Toast(string message);
    }
}