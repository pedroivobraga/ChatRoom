using System;
using System.Collections.Generic;
using System.Text;

namespace WebScoket
{
    public interface ILogger
    {
        void Error(string message);
        void Notify(string message);
    }
}
