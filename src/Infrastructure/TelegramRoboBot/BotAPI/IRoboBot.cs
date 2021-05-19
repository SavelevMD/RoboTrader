using Models.Results;
using System.Threading;
using System.Threading.Tasks;

namespace TelegramRoboBot.BotAPI
{
    public interface IRoboBot
    {
        Task<OperationResult> SendMessageAsync(string message, CancellationToken cancellationToken = default);
    }
}
