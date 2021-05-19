using System;
using System.Threading.Tasks;

namespace MessageBroker.Subscriber
{
    public interface ISubscriber
    {
        void Subscribe<TValue>(string channelSubscription, Action<string, TValue> messageHandler);

        Task SubscribeAsync<TValue>(string channelSubscription, Action<string, TValue> messageHandler);

        Task SubscribeAsync<TValue>(string channelSubscription, Func<string, TValue, Task> messageHandler);

        void Unsubscribe(string channelSubscription);
    }
}
