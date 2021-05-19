using Microsoft.Extensions.DependencyInjection;

using TelegramRoboBot.BotAPI;

namespace TelegramRoboBot.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddTelegramNotification(this IServiceCollection services)
        {
            return services.AddSingleton<IRoboBot, RoboBot>();
        }
    }
}
