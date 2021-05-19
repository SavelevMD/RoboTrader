using System.Threading.Tasks;

namespace BitfinexAdapter.Logic.Services.History
{
    internal interface ICandleHistoryPublisher
    {
        Task ReceiveAndPublishCandleAsync(string pairName, int frame);
    }
}