using Microsoft.Extensions.Options;
using System;
using TradingEngineServer.Logging.LoggingConfiguration;

namespace TradingEngineServer.Logging
{
    public class TextLogger : AbstractLogger, ITextLogger
    {
        private readonly LoggerConfiguration _loggerConfiguration;
        public TextLogger(IOptions<LoggerConfiguration> loggerConfiguration) : base()
        {
            _loggerConfiguration = loggerConfiguration.Value ?? throw new ArgumentNullException(nameof(loggerConfiguration));
        }
        protected override void Log(LogLevel logLevel, string module, string message)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
