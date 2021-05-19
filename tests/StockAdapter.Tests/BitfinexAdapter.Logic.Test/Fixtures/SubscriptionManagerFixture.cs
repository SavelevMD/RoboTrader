using BitfinexAdapter.Logic.Services;
using BitfinexAdapter.Logic.Services.SocketService;
using BitfinexAdapter.Logic.Subscription;
using Microsoft.Extensions.DependencyInjection;

namespace BitfinexAdapter.Logic.UnitTests.Fixtures
{
    public class SubscriptionManagerFixture
    {
        public SubscriptionManagerFixture()
        {
            var services = new ServiceCollection();
            services.AddSingleton<SubscriptionManager>();
            services.AddSingleton<ISocketCommandService, SocketCommandService>();
            services.AddSingleton<BitfinexMessageGenerator>();
            services.AddLogging();
            ServiceProvider = services.BuildServiceProvider();

            SubscriptionManager = ServiceProvider.GetService<SubscriptionManager>();
            CommandService = ServiceProvider.GetService<ISocketCommandService>();
            MessageGenerator = ServiceProvider.GetService<BitfinexMessageGenerator>();
        }

        public ServiceProvider ServiceProvider { get; }
        internal SubscriptionManager SubscriptionManager { get; }
        internal ISocketCommandService CommandService { get; }
        internal BitfinexMessageGenerator MessageGenerator { get; }
    }
}
