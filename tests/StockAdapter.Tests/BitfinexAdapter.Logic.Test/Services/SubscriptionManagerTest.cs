using System;
using System.Linq;

using BitfinexAdapter.Logic.Extensions.Bitfinex.Net.Extensions.Enums;
using BitfinexAdapter.Logic.UnitTests.DataGenerators;
using BitfinexAdapter.Logic.UnitTests.Fixtures;

using Brocker.Model;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;
using Xunit.Priority;

namespace BitfinexAdapter.Logic.UnitTests.Services
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class SubscriptionManagerTest : IClassFixture<SubscriptionManagerFixture>
    {
        private readonly SubscriptionManagerFixture _fixture;

        public SubscriptionManagerTest(SubscriptionManagerFixture fixture) => _fixture = fixture;

        [Theory(DisplayName = "Проверка что в Subscription не бросает exception")]
        [Priority(0), CorrectSubscription]
        public void AddSubscription_NotThrow_Exception(SubscribeMessage message)
        {
            Action method = () =>
            {
                _fixture.SubscriptionManager.SubscribedAction(JsonConvert.SerializeObject(message));
            };

            method.Should().NotThrow<Exception>();
        }

        [Fact(DisplayName = "Проверка что в Subscription все подписки в состоянии UnderReconnectSubscription == true")]
        [Priority(2)]
        public void Subscription_In_Resubscription_State()
        {
            _fixture.SubscriptionManager.SetResubscriptionState();
            _fixture.SubscriptionManager
                .Pairs
                .Select(r => r.UnderReconnectSubscription)
                .Should()
                .OnlyContain(x => x);
        }

        [Fact(DisplayName = "Проверка что в Subscription имеется одна подписка в состоянии UnderReconnectSubscription == false")]
        [Priority(3)]
        public void One_Subscription_NotIn_Resubscription_State()
        {
            var subscription = new SubscribeMessage
            {
                ChannelId = 115,
                ChannelName = "ETHUSD",
                Event = MessageType.Subscribed,
                Key = "trade:1m:tETHUSD"
            };

            _fixture.SubscriptionManager.SubscribedAction(JsonConvert.SerializeObject(subscription));

            _fixture.SubscriptionManager
                .Pairs
                .Select(r => r.UnderReconnectSubscription)
                .Should()
                .ContainSingle(x => !x);
        }

        [Fact(DisplayName = "Проверка что после прихода сообщений об отписке останется 2 элемента с подпиской")]
        [Priority(4)]
        public void Two_Subscription_Are_Present()
        {
            var subscription = new UnsubscribeMessage
            {
                ChannelId = 115,
                ChannelName = "ETHUSD",
                Event = MessageType.Unsubscribed,
                Key = "trade:1m:tETHUSD"
            };

            _fixture.SubscriptionManager.UnsubscribedAction(JsonConvert.SerializeObject(subscription), _fixture.MessageGenerator, _fixture.CommandService);

            _fixture.SubscriptionManager
                .Pairs
                .Should()
                .HaveCount(2);
        }
    }
}
