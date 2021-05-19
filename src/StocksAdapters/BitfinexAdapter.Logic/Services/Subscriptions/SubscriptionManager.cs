using System;

using BitfinexAdapter.Logic.Services;
using BitfinexAdapter.Logic.Services.Subscriptions;

using Brocker.Model;

using Models.Business.BusMessages;
using Models.SystemModels;

using Newtonsoft.Json;

namespace BitfinexAdapter.Logic.Subscription
{
    internal class SubscriptionManager : SubscriptionManagerBase
    {
        public (string pairName, int timeFrame) SubscribedAction(string message)
        {
            var msg = JsonConvert.DeserializeObject<SubscribeMessage>(message);
            var pairName = msg.Key.Split(":")[2][1..];
            var frame = int.TryParse(
                msg.Key.Split(":")[1].Replace("m", string.Empty),
                out var parsedFrame) ? parsedFrame : throw new InvalidOperationException($"Не удалось распарсить входной параметр {msg.Key.Split(":")[1]} в значение для frame");

            if (GetPairByParams(pairName, frame) is var currencyPair &&
                currencyPair != default)
            {
                currencyPair.ChannelId = msg.ChannelId;
                currencyPair.UnderReconnectSubscription = false;
            }
            else
            {
                AddPair(pairName, frame, msg.ChannelId);
            }

            return (pairName, frame);
        }

        public CurrencyTuple UnsubscribedAction(string message, BitfinexMessageGenerator messageGenerator, ISocketCommandService socketCommandService)
        {
            var unsubMsg = JsonConvert.DeserializeObject<UnsubscribeMessage>(message);
            if (GetPairByChannel(unsubMsg.ChannelId) is var unsubPair &&
                unsubPair != default)
            {
                if (!unsubPair.UnderReconnectSubscription)
                {
                    RemovePair(unsubPair.CurrencyPair, unsubPair.Frame);
                    return unsubPair;
                }
                else
                {
                    unsubPair.UnderReconnectSubscription = false;
                    var subscribeMessage = messageGenerator.Subscribe(unsubPair.CurrencyPair, unsubPair.Frame);
                    if (subscribeMessage.IsSuccess)
                    {
                        socketCommandService.SendCommand(subscribeMessage.Result);
                    }
                    else
                    {
                        //_logger
                    }
                }
            }
            return default;
        }

        internal void MarkOnUnsubscribe(BaseMessage<CandleListnerParameters> subscription)
        {
            if (GetPairByParams(subscription.Content.ChannelName, subscription.Content.Period) is var pair &&
                pair != default)
            {
                pair.UnsubscribeMark = true;
            }
        }
    }
}
