using System;
using System.Collections.Generic;
using System.Text;

namespace ReLearn.Core
{
    public interface IMessage
    {
        void Dialog(string message, Action buttonAction);
    }
}