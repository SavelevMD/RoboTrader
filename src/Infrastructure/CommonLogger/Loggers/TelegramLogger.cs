using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using TelegramRoboBot.BotAPI;

namespace CommonLogger.Loggers
{
    public class TelegramLogger : ILogger
    {
        private readonly IRoboBot _roboBot;

        public TelegramLogger(IRoboBot roboBot) => _roboBot = roboBot;

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
                await _roboBot.SendMessageAsync(message);
            });
        }
    }
}
