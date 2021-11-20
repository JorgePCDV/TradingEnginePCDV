using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TradingEngineServer.Logging.LoggingConfiguration;

namespace TradingEngineServer.Logging
{
    public class TextLogger : AbstractLogger, ITextLogger
    {
        private readonly LoggerConfiguration _loggerConfiguration;
        private readonly BufferBlock<LogInformation> _logQueue = new BufferBlock<LogInformation>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private bool _disposed = false;

        public TextLogger(IOptions<LoggerConfiguration> loggerConfiguration) : base()
        {
            _loggerConfiguration = loggerConfiguration.Value ?? throw new ArgumentNullException(nameof(loggerConfiguration));
            
            if(_loggerConfiguration.LoggerType != LoggerType.Text)
            {
                throw new InvalidOperationException($"{nameof(TextLogger)} doesn't match LoggerType of {_loggerConfiguration.LoggerType}");
            }

            var now = DateTime.Now;
            string logDirectory = Path.Combine(_loggerConfiguration.TextLoggerConfiguration.Directory, 
                $"{now:yyy-MM-dd}");
            string baseLogName = Path.Combine(_loggerConfiguration.TextLoggerConfiguration.Filename, 
                _loggerConfiguration.TextLoggerConfiguration.FileExtension);
            string filepath = Path.Combine(logDirectory, baseLogName);
            _ = Task.Run(() => LogAsync(filepath, _logQueue, _cancellationTokenSource.Token));
        }

        private static async Task LogAsync(string filePath, BufferBlock<LogInformation> logQueue, CancellationToken cancellationToken)
        {
            // using disposes of these streams at the end of the scope
            using var fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            using var streamWriter = new StreamWriter(fileStream);

            try
            {
                while(true)
                {
                    var logItem = await logQueue.ReceiveAsync(cancellationToken).ConfigureAwait(false);
                    string formattedMessage = FormatLogItem(logItem);
                    await streamWriter.WriteAsync(formattedMessage).ConfigureAwait(false);
                }
            } catch(OperationCanceledException) {}
        }

        private static string FormatLogItem(LogInformation logItem)
        {
            return $"[{logItem.Now:yyyy-MM-dd HH-mm-ss.fffffff}] [{logItem.ThreadName,-30}:{logItem.ThreadId:000}]"
                + $"[{logItem.LogLevel} {logItem.Message}]";
        }

        protected override void Log(LogLevel logLevel, string module, string message)
        {
            _logQueue.Post(new LogInformation(logLevel, module, message, DateTime.Now, 
                                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name));
        }

        

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            if(disposing)
            {
                // Get rid of managed resources
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }

            // Get rid of unmanaged resources
        }
    }
}
