using System;
using System.Threading;
using System.Threading.Tasks;

using MessageBroker.Publisher;

using Models.Business.BusMessages;
using Models.Enums;

using Newtonsoft.Json;

using StackExchange.Redis;

namespace MessageBroker.Redis
{
    public class DefaultPublisher : IPublisher
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly ISubscriber _publisher;

        public DefaultPublisher(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            _publisher = _redisConnection.GetSubscriber();
        }

        public void Publish<T>(string channel, BaseMessage<T> message)
        {
            ThrowExceptionIfIncorrect(channel, message);
            _publisher.Publish(channel, JsonConvert.SerializeObject(message));
        }

        public async Task PublishAsync<T>(string channel, BaseMessage<T> message, CancellationToken cancellationToken = default)
        {
            ThrowExceptionIfIncorrect(channel, message);
            await _publisher.PublishAsync(channel, JsonConvert.SerializeObject(message));
        }

        public async Task PublishAsync<T>(string channel, T message, BusMessageType messageType, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var baseMessage = new BaseMessage<T> { Content = message, MessageType = messageType };

            ThrowExceptionIfIncorrect(channel, baseMessage);
            await _publisher.PublishAsync(channel, JsonConvert.SerializeObject(baseMessage));
        }

        private static void ThrowExceptionIfIncorrect<T>(string channel, BaseMessage<T> message)
        {
            if (string.IsNullOrWhiteSpace(channel))
            {
                throw new ArgumentNullException($"Входной параметр {nameof(channel)} не может быть null или пустой строкой");
            }

            if (message == default)
            {
                throw new ArgumentNullException($"Входной параметр {nameof(message)} не может быть null");
            }
        }
    }
}
