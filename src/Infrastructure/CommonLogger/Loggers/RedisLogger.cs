using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using StackExchange.Redis;

namespace CommonLogger.Loggers
{
    public class RedisLogger : ILogger
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly Lazy<ISubscriber> _publisher;
        private const string ErrorsChannel = "Errors";

        public RedisLogger(string categotyName, IConnectionMultiplexer connection)
        {
            _connection = connection;
            _publisher = new Lazy<ISubscriber>(() => _connection.GetSubscriber(), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter?.Invoke(state, exception);

            Task.Run(async () =>
            {
                await _publisher.Value.PublishAsync(ErrorsChannel, message);
            });
        }
    }
}
