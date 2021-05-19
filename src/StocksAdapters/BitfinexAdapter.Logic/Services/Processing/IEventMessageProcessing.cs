using System.Threading.Tasks;

namespace BitfinexAdapter.Logic.Services.Processing
{
    internal interface IEventMessageProcessing
    {
        Task StatusMessageHandleAsync(string message);
    }
}