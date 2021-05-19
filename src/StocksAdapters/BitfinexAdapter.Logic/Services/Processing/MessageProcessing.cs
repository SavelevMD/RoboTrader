using System;
using System.Threading.Tasks;

using BitfinexAdapter.Logic.Services.Processing;

using Microsoft.Extensions.Logging;

namespace Brocker.Services.Processing
{
    internal class MessageProcessing
    {
        private readonly ICandleProcessing _candleProcessing;
        private readonly IEventMessageProcessing _eventMessageProcessing;
        private readonly ILogger<MessageProcessing> _logger;

        public MessageProcessing(ICandleProcessing candleProcessing,
                                 IEventMessageProcessing eventMessageProcessing,
                                 ILogger<MessageProcessing> logger)
        {
            _candleProcessing = candleProcessing;
            _eventMessageProcessing = eventMessageProcessing;
            _logger = logger;
        }

        public bool IsMaintananceMode { get; private set; }

        public void MessageHandler(string message)
        {
            Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(message))
                    {
                        return;
                    }

                    if (message.StartsWith("{"))
                    {
                        await _eventMessageProcessing.StatusMessageHandleAsync(message);
                    }
                    else if (message.StartsWith("["))
                    {
                        await _candleProcessing.CandleMessageHandleAsync(message);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "В процессе обработки сообщения от биржи произошла ошибка");
                }
            });
        }
    }
}
