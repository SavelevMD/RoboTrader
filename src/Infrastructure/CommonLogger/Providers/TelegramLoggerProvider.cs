using System.Collections.Concurrent;

using CommonLogger.Loggers;

using Microsoft.Extensions.Logging;

using TelegramRoboBot.BotAPI;

namespace CommonLogger.Providers
{
    [ProviderAlias("Telegram")]
    public class TelegramLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, ILogger> _loggers = new ConcurrentDictionary<string, ILogger>();
        private readonly IRoboBot _roboBot;

        public TelegramLoggerProvider(IRoboBot roboBot) => _roboBot = roboBot;

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new TelegramLogger(_roboBot));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
