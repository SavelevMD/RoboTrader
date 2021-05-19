using System;

using BitfinexAdapter.Logic.Services;
using BitfinexAdapter.Logic.Subscription;

using Models.Business.BusMessages;
using Models.Enums;
using Models.Results;
using Models.SystemModels;

using Newtonsoft.Json;

namespace BitfinexAdapter.Logic.Extensions
{
    internal static class BitfinexMessageGeneratorExtension
    {
        public static OperationResult<string> GenerateMessage(this BitfinexMessageGenerator messageGenerator, BaseMessage<CandleListnerParameters> message, SubscriptionManager subscriptionManager)
        {
            var channelId = subscriptionManager.GetPairByParams(message.Content.ChannelName, message.Content.Period);

            switch (message.MessageType)
            {
                case BusMessageType.UnsubscribeChannel:
                    if (channelId == default)
                    {
                        return OperationResult<string>.Failure($"Данная пара при попытке отписки не найдена {message.Content.ChannelName}:{message.Content.Period}");
                    }
                    return messageGenerator.Unsubscribe(message.Content.ChannelName, message.Content.Period, channelId.ChannelId);
                case BusMessageType.SubscribeChannel:
                    if (channelId != default)
                    {
                        return OperationResult<string>.Failure($"На данную пару {message.Content.ChannelName}:{message.Content.Period} уже существует подписка");
                    }
                    return messageGenerator.Subscribe(message.Content.ChannelName, message.Content.Period);
                default:
                    return OperationResult<string>.Failure($"Не корректный тип сообщения {message.MessageType} для данного канала {Environment.NewLine}{JsonConvert.SerializeObject(message)}");
            }
        }
    }
}
