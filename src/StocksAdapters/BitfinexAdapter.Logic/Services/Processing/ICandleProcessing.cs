using System.Threading.Tasks;

namespace BitfinexAdapter.Logic.Services.Processing
{
    public interface ICandleProcessing
    {
        Task CandleMessageHandleAsync(string message);
    }
}