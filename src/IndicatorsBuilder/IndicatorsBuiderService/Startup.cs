using IndicatorsBuilderService.Logic.Extensions;

using MessageBroker.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace IndicatorsBuiderService
{
    public class Startup : IStartup
    {
        public static string DebugConnection = "localhost:32768,abortConnect=false";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRepositories();
            services.AddLogic();
            services.AddMessageBroker(DebugConnection);
        }
    }
}
