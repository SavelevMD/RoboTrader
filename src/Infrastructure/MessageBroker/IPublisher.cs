using Models.Business.BusMessages;
using Models.Enums;

using System.Threading;
using System.Threading.Tasks;

namespace MessageBroker.Publisher
{
    public interface IPublisher
    {
        void Publish<T>(string channel, BaseMessage<T> message);
        Task PublishAsync<T>(string channel, BaseMessage<T> message, CancellationToken cancellationToken = default);
        Task PublishAsync<T>(string channel, T message, BusMessageType messageType, CancellationToken cancellationToken = default);
    }
}
