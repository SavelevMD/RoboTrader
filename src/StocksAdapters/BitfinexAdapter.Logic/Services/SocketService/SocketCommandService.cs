using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using BitfinexAdapter.Logic.Models;
using BitfinexAdapter.Logic.Subscription;

using Microsoft.Extensions.Logging;

using Models.Results;

namespace BitfinexAdapter.Logic.Services.SocketService
{
    internal class SocketCommandService : ISocketCommandService
    {
        private readonly ConcurrentQueue<string> _deferredCommands = new ConcurrentQueue<string>();
        private readonly SubscriptionManager _subscriptionManager;
        private readonly BitfinexMessageGenerator _bitfinexMessageGenerator;
        private readonly ILogger<SocketCommandService> _logger;

        public Subject<IStockCommand> CommandSubject { get; } = new Subject<IStockCommand>();

        public SocketCommandService(SubscriptionManager subscriptionManager,
            BitfinexMessageGenerator bitfinexMessageGenerator,
            ILogger<SocketCommandService> logger)
        {
            _subscriptionManager = subscriptionManager;
            _bitfinexMessageGenerator = bitfinexMessageGenerator;
            _logger = logger;
        }

        private volatile bool _isMaintenanceMode;
        public bool IsMaintenanceMode
        {
            get => _isMaintenanceMode;
            set
            {
                _isMaintenanceMode = value;
                if (!value)
                {
                    TrySendCommand();
                }
            }
        }

        private void TrySendCommand()
        {
            foreach (var _ in _deferredCommands)
            {
                if (!_isMaintenanceMode &&
                    _deferredCommands.TryDequeue(out var command))
                {
                    SendCommand(command);
                }
            }

            if (_deferredCommands.Count > 0)
            {
                _logger.LogError("Не вся коллекия отложенных команд была выполнена");
            }
        }

        public Subject<bool> ReconnectIsNeeded { get; private set; } = new Subject<bool>();

        public OperationResult ReconnectCommand()
        {
            ReconnectIsNeeded.OnNext(true);
            return OperationResult.Success();
        }

        public void RefreshSubscriptions()
        {
            //TODO: add configuration command
            SendCommand("{\"event\":\"conf\",\"flags\":32768}");

            _subscriptionManager.SetResubscriptionState();
            var commands = _subscriptionManager.Pairs
                .Where(r => !r.UnsubscribeMark)
                .Select(r => _bitfinexMessageGenerator.Subscribe(r.CurrencyPair, r.Frame))
                .Where(r => r.IsSuccess)
                .Select(r => r.Result);
            Parallel.ForEach(commands, r => SendCommand(r));
        }

        public OperationResult SendCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                OperationResult.Failure($"Входной параметр {nameof(command)} не может быть пустым или равным null");
            }

            if (!IsMaintenanceMode)
            {
                CommandSubject.OnNext(new StockCommand { Command = command });
                return OperationResult.Success();
            }

            if (!command.Contains("ping"))
            {
                _deferredCommands.Enqueue(command);
            }
            return OperationResult.Success("Биржа находится на техническом перерыве, команда отложена в список ожидания");
        }
    }
}
