using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Logging
{
    abstract class AbstractLogger : ILogger
    {
        protected abstract void Log(LogLevel logLevel, string module, string message);

        public void Debug(string module, string message)
        {
            throw new NotImplementedException();
        }

        public void Debug(string module, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(string module, string message)
        {
            throw new NotImplementedException();
        }

        public void Error(string module, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Information(string module, string message)
        {
            throw new NotImplementedException();
        }

        public void Information(string module, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warning(string module, string message)
        {
            throw new NotImplementedException();
        }

        public void Warning(string module, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
