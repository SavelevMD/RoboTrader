using System.Threading.Tasks;

using MessageBroker.Test.Fixrure;

using Models.Business.BusMessages;

using Xunit;

namespace MessageBroker.Test.PubSubTest
{
    public class PubSubPatternTest : IClassFixture<PubSubFixture>
    {
        private readonly PubSubFixture _fixture;

        public PubSubPatternTest(PubSubFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Check_Correct_Work_PatternAsync()
        {
            //Arrange
            var testRC = string.Empty;
            BaseMessage<string> testRV = default;
            var message = "afafa";
            //Act
            await _fixture.Subscription.SubscribeAsync<BaseMessage<string>>("candle*ping_pong", (rc, rv) => { testRC = rc; testRV = rv; });
            await _fixture.Publisher.PublishAsync("candle_ETHUSD_ping_pong", new BaseMessage<string> { Content = message });
            //вот этот костыль для того что бы сообщение успели докинуть через шину
            await Task.Delay(1000);
            //Assert
            Assert.True(testRV != default && testRV.Content.Equals(message));
        }
    }
}
