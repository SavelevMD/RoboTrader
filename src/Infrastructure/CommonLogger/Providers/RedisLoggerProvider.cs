using CommonLogger.Loggers;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Concurrent;

namespace CommonLogger.Providers
{
    [ProviderAlias("Redis")]
    public class RedisLoggerProvider : ILoggerProvider
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly ConcurrentDictionary<string, ILogger> _loggers = new ConcurrentDictionary<string, ILogger>();

        public RedisLoggerProvider(IConnectionMultiplexer connection) => _connection = connection;

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new RedisLogger(name, _connection));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
