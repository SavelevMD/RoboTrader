using System;

using BitfinexAdapter.BackgroundWorker;
using BitfinexAdapter.Logic.BackgroundWorker;
using BitfinexAdapter.Logic.REST;
using BitfinexAdapter.Logic.Services;
using BitfinexAdapter.Logic.Services.History;
using BitfinexAdapter.Logic.Services.Processing;
using BitfinexAdapter.Logic.Services.SocketService;
using BitfinexAdapter.Logic.Subscription;

using Brocker.BackgroundWorker;
using Brocker.Services.Processing;

using Connectors;

using MessageBroker.Extensions;

using Microsoft.Extensions.DependencyInjection;

using Scheduler.Interfaces;
using Scheduler.RxImplementation;

using TaskManager.Storage.Repositories;

using TelegramRoboBot.Extensions;

namespace BitfinexAdapter.Logic.Extensions
{
    public static class ServiceExtension
    {
        private const string RedisConnectionEnvVariableName = "REDIS_CONNECTION_STRING";

        public static IServiceCollection AddMessageGenerator(this IServiceCollection services)
        {
            return services.AddSingleton<BitfinexMessageGenerator>();
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddSingleton<ITaskRepository, TaskRepository>();
        }

        public static IServiceCollection AddServiceLogic(this IServiceCollection services)
        {
            services.AddHostedService<BrokerChannelService>();
            services.AddHostedService<BitfinexSocketService>();
            services.AddHostedService<WarmupSubscriptionsService>();

            services.AddTelegramNotification();
            services.AddSingleton<ISocketCommandService, SocketCommandService>();
            services.AddSingleton<SubscriptionManager>();
            services.AddSingleton<MessageProcessing>();
            services.AddSingleton<IEventMessageProcessing, EventMessageProcessing>();
            services.AddSingleton<ICandleProcessing, CandleProcessing>();
            services.AddSingleton<ICandleHistoryPublisher, CandleHistoryPublisher>();

            services.AddSingleton<PingPongRequestSchedulerService>();

#if DEBUG
            services.AddMessageBroker(RedisConnections.DebugConnection);
#else
            services.AddMessageBroker(Environment.GetEnvironmentVariable(RedisConnectionEnvVariableName));
#endif

            services.AddSingleton<ICommonScheduler, CommonScheduler>();
            services.AddLogging();

            services.AddHttpClient<ICandlesHistoryRest, CandlesHistoryRest>();

            return services;
        }
    }
}
