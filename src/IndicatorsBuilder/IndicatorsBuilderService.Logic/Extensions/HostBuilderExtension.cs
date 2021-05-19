
using System;
using System.Linq;

using Microsoft.Extensions.Hosting;

namespace IndicatorsBuilderService.Logic.Extensions
{
    public static class HostBuilderExtension
    {
        public static IHostBuilder UseStartup<TStartup>(this IHostBuilder builder)
            where TStartup : IStartup
        {
            builder.ConfigureServices((context, services) =>
            {
                var contextProps = context.GetType().GetProperties();
                var constructors = typeof(TStartup).GetConstructors()
                    .OrderByDescending(u => u.GetParameters().Count())
                    .First();

                var parametrs = constructors.GetParameters()
                    .SelectMany(param => contextProps.Where(prop => prop.PropertyType == param.ParameterType)
                        .Select(prop => prop.GetValue(context)))
                    .ToArray();

                if (constructors.GetParameters().Count() != parametrs.Count())
                {
                    throw new ArgumentException($"Доступные параметры конструктора `Startup`:`[{string.Join(",", contextProps.Select(u => u.Name))}]`");
                }

                (Activator.CreateInstance(typeof(TStartup), parametrs) as IStartup)
                    .ConfigureServices(services);
            });
            return builder;
        }
    }
}
