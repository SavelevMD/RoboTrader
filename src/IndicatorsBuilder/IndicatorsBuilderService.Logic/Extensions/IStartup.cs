using Microsoft.Extensions.DependencyInjection;

namespace IndicatorsBuilderService.Logic.Extensions
{
    public interface IStartup
    {
        public void ConfigureServices(IServiceCollection services);
    }
}
