using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;

namespace TradingEngineServer.Core
{
    sealed class TradingEngineServer : BackgroundService, ITradingEngineServer
    {
        private readonly IOptions<TradingEngineServerConfiguration> _engineConfiguration;
        private readonly ITextLogger _textLogger;

        public TradingEngineServer(IOptions<TradingEngineServerConfiguration> engineConfiguration,
                                   ITextLogger textLogger)
        {
            _engineConfiguration = engineConfiguration ?? throw new ArgumentNullException(nameof(engineConfiguration));
            _textLogger = textLogger ?? throw new ArgumentNullException(nameof(textLogger));
        }

        public Task Run(CancellationToken token) => ExecuteAsync(token);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting {nameof(TradingEngineServer)}");
            while (!stoppingToken.IsCancellationRequested)
            {

            }
            _logger.LogInformation($"Stopping {nameof(TradingEngineServer)}");
            return Task.CompletedTask;
        }
    }
}

