
using MessageBroker.Extensions;
using MessageBroker.Publisher;
using MessageBroker.Subscriber;

using Microsoft.Extensions.DependencyInjection;

namespace MessageBroker.Test.Fixrure
{
    public class PubSubFixture
    {
        private static readonly string _debugConnection = "localhost:32768,abortConnect=false";
        public PubSubFixture()
        {
            ServiceCollection = new ServiceCollection();
            ServiceCollection.AddMessageBroker(_debugConnection);
            ServiceProvider = ServiceCollection.BuildServiceProvider();
            Subscription = ServiceProvider.GetService<ISubscriber>();
            Publisher = ServiceProvider.GetService<IPublisher>();
        }

        public ServiceProvider ServiceProvider { get; }
        public ISubscriber Subscription { get; }
        public IPublisher Publisher { get; }
        private ServiceCollection ServiceCollection { get; }
    }
}
