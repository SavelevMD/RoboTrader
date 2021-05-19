
using System;

using MessageBroker.Publisher;

using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;

namespace MessageBroker.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException($"Входной параметр {nameof(connectionString)} не может быть null или пустой строкой");
            }

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(connectionString)));

            services.Scan(u => u.FromAssemblies(typeof(ServiceExtensions).Assembly)
               .AddClasses(v => v.AssignableToAny(
                        typeof(IPublisher), typeof(Subscriber.ISubscriber)
                    ))
                   .AsImplementedInterfaces()
                   .WithSingletonLifetime()
                );
            return services;
        }
    }
}
