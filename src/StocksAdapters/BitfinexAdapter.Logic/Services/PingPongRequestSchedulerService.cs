using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using Scheduler.Interfaces;

namespace BitfinexAdapter.Logic.Services
{
    internal class PingPongRequestSchedulerService
    {
        private readonly ConcurrentDictionary<int, IDisposable> _scheduledPingPongRequest = new ConcurrentDictionary<int, IDisposable>();
        public ConcurrentDictionary<int, int> _cids = new ConcurrentDictionary<int, int>();

        private readonly ICommonScheduler _commonScheduler;
        private readonly BitfinexMessageGenerator _messageGenerator;
        private readonly Random _random;
        private readonly ILogger<PingPongRequestSchedulerService> _logger;

        public PingPongRequestSchedulerService(ICommonScheduler commonScheduler,
            BitfinexMessageGenerator messageGenerator,
            ILogger<PingPongRequestSchedulerService> logger)
        {
            _commonScheduler = commonScheduler;
            _messageGenerator = messageGenerator;
            _random = new Random();
            _logger = logger;
        }

        public void NewSubscription(int frame)
        {
            if (!_scheduledPingPongRequest.TryGetValue(frame, out var scheduled))
            {
                var cid = _random.Next();
                var pingPongMessage = _messageGenerator.GeneratePingMessage(cid);

                _scheduledPingPongRequest.GetOrAdd(frame, _commonScheduler.Schedule<ISocketCommandService>(service => service.SendCommand(pingPongMessage), TimeSpan.FromMinutes(frame)));
                _cids.GetOrAdd(cid, frame);

                _logger.LogInformation($"Задача запросов времени на бирже для интервала {frame} запущена");
            }
        }

        public void RemoveScheduledRequest(int frame)
        {
            if (_scheduledPingPongRequest.TryRemove(frame, out var scheduled))
            {
                scheduled.Dispose();

                var keyCID = _cids.Where(r => r.Value == frame)
                    .Select(r => r.Key)
                    .Single();
                _cids.Remove(keyCID, out var _);

                _logger.LogInformation($"Задача запросв времени на бирже для интервала {frame} остановлена");
            }
            else
            {
                _logger.LogInformation($"Задача запросв времени на бирже для интервала {frame} не найдена");
            }
        }

        public int TryGetFrameByCID(int cid)
        {
            return _cids.TryGetValue(cid, out var result) ? result : -1;
        }
    }
}
