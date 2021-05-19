using IndicatorsBuilderService.Logic.HostedServices;
using IndicatorsBuilderService.Logic.Repositories.Candles;
using IndicatorsBuilderService.Logic.Repositories.Indicators;
using IndicatorsBuilderService.Logic.Services.Builders;
using IndicatorsBuilderService.Logic.Services.Processing;

using Microsoft.Extensions.DependencyInjection;

namespace IndicatorsBuilderService.Logic.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSingleton<IIndicatorsRepositiry, IndicatorsRepositiry>();
            services.AddSingleton<ICandleRepository, CandleRepository>();
            services.AddSingleton<IIndicatorsValueRepository, IndicatorsValueRepository>();
            return services;
        }

        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            services.AddRepositories();

            services.AddSingleton<IIndicatorBuilder, IndicatorBuilder>();
            services.AddSingleton<ICandleProcessing, CandleProcessing>();
            services.AddSingleton<ITaskProcessing, TaskProcessing>();
            services.AddSingleton<ICandleProcessing, CandleProcessing>();

            services.AddHostedService<ChannelListnerHostedService>();
            services.AddHostedService<PublisherHostedService>();

            return services;
        }
    }
}
