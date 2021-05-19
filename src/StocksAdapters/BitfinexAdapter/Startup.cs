
using AutoMapper;

using BitfinexAdapter.Logic.Extensions;
using BitfinexAdapter.Logic.Options;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Models.Extensions;

using Robo.Database.Extensions;

using TaskManager.Storage.Repositories;

namespace BitfinexAdapter
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddMainStorage();
            services.AddSingleton<TaskRepository>();
            services.AddCandleRepository();

            services.AddAutoMapper(config =>
            {
                config.CreateMap<Logic.Models.Candles.CandleModel, Models.Business.CandleModel>();
            }, typeof(Logic.Models.Candles.CandleModel).Assembly, typeof(Models.Business.CandleModel).Assembly);

            services.AddMessageGenerator();

            services.AddServiceLogic();

            services.Configure<StockApiConnections>(Configuration.GetSection(nameof(StockApiConnections)));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}
