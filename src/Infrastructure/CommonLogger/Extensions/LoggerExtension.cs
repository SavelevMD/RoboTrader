using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace CommonLogger.Extensions
{
    public static class LoggerExtension
    {
        public static ILoggingBuilder AddCustomLogger(this ILoggingBuilder loggerBuilder)
        {
            loggerBuilder.Services.Scan(u => u.FromAssemblies(typeof(LoggerExtension).Assembly)
                .AddClasses(v => v.AssignableTo<ILoggerProvider>())
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime());

            return loggerBuilder;
        }
    }
}
