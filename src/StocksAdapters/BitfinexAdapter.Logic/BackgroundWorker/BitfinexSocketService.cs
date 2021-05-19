using System;
using System.Threading;
using System.Threading.Tasks;

using BitfinexAdapter.Logic.Options;
using BitfinexAdapter.Logic.Services;

using Brocker.Services.Processing;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Websocket.Client;

namespace BitfinexAdapter.BackgroundWorker
{
    internal class BitfinexSocketService : IHostedService
    {
        private readonly MessageProcessing _processing;
        private readonly ISocketCommandService _commandService;
        private readonly IOptions<StockApiConnections> _options;
        private readonly ILogger<BitfinexSocketService> _logger;
        private WebsocketClient _socket;
        private IDisposable _disconnectSubscriber;
        private IDisposable _messageSubscriber;
        private IDisposable _commandSubscriber;

        public BitfinexSocketService(MessageProcessing processing,
            ISocketCommandService commandService,
            IOptions<StockApiConnections> options,
            ILogger<BitfinexSocketService> customlogger)
        {
            _processing = processing;
            _commandService = commandService;
            _options = options;
            _logger = customlogger;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitSocketAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _disconnectSubscriber?.Dispose();
            _messageSubscriber?.Dispose();
            _commandSubscriber?.Dispose();
            return Task.CompletedTask;
        }

        #region private

        private async Task InitSocketAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //TODO: add constructor with IOptionsMonitor with connection string
            _socket = new WebsocketClient(new Uri(_options.Value.ConnectionString))
            {
                ReconnectTimeout = TimeSpan.FromSeconds(_options.Value.ReconnectionTimeout)
            };

            _disconnectSubscriber = _socket.DisconnectionHappened.Subscribe(info =>
            {
                _commandService.IsMaintenanceMode = true;
                _logger.LogError(info.Exception, $"{DateTime.UtcNow} Произошел дисконнект по сокету");
            });

            _socket.ReconnectionHappened.Subscribe(info =>
            {
                _commandService.IsMaintenanceMode = false;
                _logger.LogInformation($"{DateTime.UtcNow} Вроде как переподключились");
                _commandService.RefreshSubscriptions();
            });

            _commandSubscriber = _commandService.CommandSubject.Subscribe(command =>
            {
                if (_socket.IsRunning)
                {
                    _socket.Send(command.Command);
                }
                else
                {
                    _logger.LogError($"{DateTime.UtcNow} Сокет в состоянии 'остановлен'");
                }
            }, exception =>
            {
                _logger.LogError(exception, "В процессе получения команд для отправки на биржу произошла ошибка");
            });

            _commandService.ReconnectIsNeeded.Subscribe((needReconnect) =>
            {
                if (needReconnect)
                {
                    _logger.LogInformation("Рестарт соединения через сокет");
                    _socket.Reconnect();
                }
            });

            _messageSubscriber = _socket.MessageReceived.Subscribe(message =>
            {
                _processing.MessageHandler(message.Text);
            },
            exception =>
            {
                _logger.LogError(exception, "В процессе получения сообщений от биржи произошла ошибка");
            },
            () =>
            {
                _logger.LogInformation("Остановка приема сообщений по сокету, по причине остновки источника");
            });

            await _socket.Start();
            _commandService.IsMaintenanceMode = !_socket.IsRunning;
        }
        #endregion
    }
}
