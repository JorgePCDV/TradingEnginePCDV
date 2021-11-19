using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using TradingEngineServer.Logging.LoggingConfiguration;

namespace TradingEngineServer.Logging
{
    public class TextLogger : AbstractLogger, ITextLogger
    {
        private readonly LoggerConfiguration _loggerConfiguration;
        private readonly BufferBlock<LogInformation> _logQueue = new BufferBlock<LogInformation>();

        public TextLogger(IOptions<LoggerConfiguration> loggerConfiguration) : base()
        {
            _loggerConfiguration = loggerConfiguration.Value ?? throw new ArgumentNullException(nameof(loggerConfiguration));
        }
        protected override void Log(LogLevel logLevel, string module, string message)
        {
            _logQueue.Post(new LogInformation(logLevel, module, message, DateTime.Now, 
                                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
