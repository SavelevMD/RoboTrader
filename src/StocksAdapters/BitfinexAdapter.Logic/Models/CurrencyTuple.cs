using System;

namespace Brocker.Model
{
    public class CurrencyTuple
    {
        public CurrencyTuple(string currencyPair, int frame, int channelId = -1)
        {
            CurrencyPair = currencyPair;
            Frame = frame;
            ChannelId = channelId;
        }

        private volatile bool unsubscribeMark;
        public bool UnsubscribeMark { get => unsubscribeMark; set => unsubscribeMark = value; }

        private volatile bool _underReconnectSubscription;

        public bool UnderReconnectSubscription
        {
            get => _underReconnectSubscription;
            set => _underReconnectSubscription = value;
        }

        public int Frame { get; }

        public string CurrencyPair { get; private set; }

        public int ChannelId { get; set; }

        public int CID { get; set; }

        public IDisposable Scheduled { get; set; }
    }
}
