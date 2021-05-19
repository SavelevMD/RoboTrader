using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MessageBroker.Publisher;

using Microsoft.Extensions.Hosting;

using Models.Enums;
using Models.Tasks;

namespace IndicatorsBuilderService.Logic.HostedServices
{
    internal class PublisherHostedService : IHostedService
    {
        private readonly IPublisher _publisher;

        public PublisherHostedService(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {


            var wrappedTask = new TaskWrapper { CurPair = "ETHUSD", TimeFrame = 1, Indicators = new List<string> { "i_RSI_14", "i_MA_101", "i_MA_252", "i_CCI_50" } };
            await _publisher.PublishAsync("task_indicators_add", wrappedTask, BusMessageType.Tasks);

            wrappedTask = new TaskWrapper { CurPair = "BTCUSD", TimeFrame = 1, Indicators = new List<string> { "i_RSI_14", "i_MA_101", "i_MA_252", "i_CCI_50" } };
            await _publisher.PublishAsync("task_indicators_add", wrappedTask, BusMessageType.Tasks);

            //var candles = CandleGenerator.GetCandles(9000);
            //await _publisher.PublishAsync("candle", candles, BusMessageType.Candles);

            //await Task.Delay(10000);

            //var sw = new Stopwatch();
            //sw.Start();
            //Enumerable.Range(0, 100_000_000).Select(r => DateTime.Now.AddMinutes(r))/*.AsParallel()*/.ToList();
            //sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds);

            //await _publisher.PublishAsync("candle_ETHUSD_1m_ping_pong",
            //    new CandleListnerParameters { ChannelName = "ETHUSD", Period = "1m" },
            //    BusMessageType.Candles);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
