using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using StackExchange.Redis;

using static StackExchange.Redis.RedisChannel;

using ISubscriber = MessageBroker.Subscriber.ISubscriber;

namespace MessageBroker.Redis
{
    public class DefaultSubscriber : ISubscriber
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly StackExchange.Redis.ISubscriber _publisher;

        public DefaultSubscriber(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            _publisher = _redisConnection.GetSubscriber();
        }

        public void Subscribe<TValue>(string channelSubscription, Action<string, TValue> messageHandler)
        {
            ThrowExceptionIfIncorrect(channelSubscription, messageHandler);

            _publisher.Subscribe(new RedisChannel(channelSubscription, PatternMode.Pattern), (rc, rv) =>
            {
                messageHandler.Invoke(rc, JsonConvert.DeserializeObject<TValue>(rv));
            });
        }

        public async Task SubscribeAsync<TValue>(string channelSubscription, Action<string, TValue> messageHandler)
        {
            ThrowExceptionIfIncorrect(channelSubscription, messageHandler);

            await _publisher.SubscribeAsync(new RedisChannel(channelSubscription, PatternMode.Pattern), (rc, rv) =>
            {
                messageHandler.Invoke(rc, JsonConvert.DeserializeObject<TValue>(rv));
            });
        }

        public async Task SubscribeAsync<TValue>(string channelSubscription, Func<string, TValue, Task> messageHandler)
        {
            await _publisher.SubscribeAsync(new RedisChannel(channelSubscription, PatternMode.Pattern), async (rc, rv) =>
            {
                await messageHandler.Invoke(rc, JsonConvert.DeserializeObject<TValue>(rv));
            });
        }

        public void Unsubscribe(string channelSubscription)
        {
            if (string.IsNullOrWhiteSpace(channelSubscription))
            {
                throw new ArgumentNullException($"Входной параметр {nameof(channelSubscription)} не может быть null или пустой строкой");
            }

            _publisher.Unsubscribe(channelSubscription);
        }

        private static void ThrowExceptionIfIncorrect<T>(string channelSubscription, Action<string, T> messageHandler)
        {
            if (string.IsNullOrWhiteSpace(channelSubscription))
            {
                throw new ArgumentNullException($"Входной параметр {nameof(channelSubscription)} не может быть null или пустой строкой");
            }

            if (messageHandler.Equals(default))
            {
                throw new ArgumentNullException($"Входной параметр {nameof(messageHandler)} не может быть null");
            }
        }

    }
}
