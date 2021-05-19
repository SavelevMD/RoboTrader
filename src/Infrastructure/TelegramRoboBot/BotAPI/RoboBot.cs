using System;
using System.Threading;
using System.Threading.Tasks;

using Models.Results;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramRoboBot.BotAPI
{
    public class RoboBot : IRoboBot
    {
        private readonly TelegramBotClient _botClient;
        private readonly ChatId _chatId;

        public RoboBot()
        {
            _botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("ROBO_ACCESS_TOKEN"));
            _chatId = new ChatId(Environment.GetEnvironmentVariable("ROBO_CHAT_ID"));
        }

        public async Task<OperationResult> SendMessageAsync(string message, CancellationToken cancellationToken = default)
        {
            try
            {
                await _botClient.SendTextMessageAsync(_chatId, message, cancellationToken: cancellationToken);
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(ex.Message);
            }
        }
    }
}
