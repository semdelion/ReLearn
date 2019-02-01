using System;

namespace ReLearn.Core
{
    public interface IMessageCore
    {
        void Dialog(string message, Action buttonAction);
        void Toast(string message);
    }
}