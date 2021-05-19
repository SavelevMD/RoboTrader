using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Robo.Database.Context;

namespace Robo.Database.Extensions
{
    public static class ServiceExtensions
    {
        private const string PGConnectionString = "PG_CONNECTION_STRING";
        public static string DebugConnection = "Host=127.0.0.1;Port=5432;Database=ticker;User ID=postgres;Password=norobo007";

        public static IServiceCollection AddMainStorage(this IServiceCollection services)
        {

#if DEBUG
            services.AddDbContext<TickerContext>(options => { options.UseNpgsql(DebugConnection); }, ServiceLifetime.Singleton);
#else
            services.AddDbContext<TickerContext>(options => { options.UseNpgsql(Environment.GetEnvironmentVariable(PGConnectionString)); }, ServiceLifetime.Singleton);
#endif
            return services;
        }
    }
}
