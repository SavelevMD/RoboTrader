using System.Threading.Tasks;

using Models.Business;
using Models.Business.BusMessages;
using Models.SystemModels;

namespace IndicatorsBuilderService.Logic.Services.Processing
{
    internal interface ICandleProcessing
    {
        Task CandlesProcessingAsync(string channelName, BaseMessage<CandlesModel> candles);
        Task PingPongProcessingAsync(string channelName, BaseMessage<CandleListnerParameters> pingPongParameters);
    }
}