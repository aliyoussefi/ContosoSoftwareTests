using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoSoftware.PowerApps.UIAutomation.Tests
{
    public interface ILogger
    {
        void Log(string message);
        void Log();
        void Log(string message, Exception exception);
        void Log(Dictionary<string, string> dictionaryMessages);
    }
}
