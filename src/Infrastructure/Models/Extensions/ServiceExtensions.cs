using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

using Models.Mappers;
using Models.Repositories.Candle;
using Models.Repositories.Candles;
using Models.Repositories.Currency;

namespace Models.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(config =>
            {
                config.AddProfile<TaskModelToTasksProfile>();
                config.AddProfile<TaskHistoryModelToTaskHistoryProfile>();
                config.AddProfile<CandleModelToCandleProfile>();
            }, typeof(TaskModelToTasksProfile).Assembly);
        }

        public static IServiceCollection AddCandleRepository(this IServiceCollection services)
        {
            return services.AddScoped<ICandleRepository, CandleRepository>();
        }

        public static IServiceCollection AddCurrencyRepository(this IServiceCollection services)
        {
            return services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        }
    }
}
