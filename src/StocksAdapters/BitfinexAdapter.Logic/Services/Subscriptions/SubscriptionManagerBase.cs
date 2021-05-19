using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Brocker.Model;

namespace BitfinexAdapter.Logic.Services.Subscriptions
{
    internal class SubscriptionManagerBase
    {
        private readonly Dictionary<int, CurrencyTuple> _collection = new Dictionary<int, CurrencyTuple>();
        public readonly object _syncObject = new object();

        public void SetResubscriptionState()
        {
            IEnumerable<CurrencyTuple> snapshot;

            lock (_syncObject)
            {
                snapshot = _collection.Values;
            }
            Parallel.ForEach(snapshot, item => item.UnderReconnectSubscription = true);

        }

        public List<CurrencyTuple> Pairs
        {
            get
            {
                lock (_syncObject)
                {
                    return _collection.Values.ToList();
                }
            }
        }

        protected void AddPair(string pairName, int frame, int channelId)
        {
            lock (_syncObject)
            {
                _collection.TryAdd(channelId, new CurrencyTuple(pairName, frame, channelId));
            }
        }

        protected void RemovePair(string pairName, int frame)
        {
            lock (_syncObject)
            {
                var elementForRemove = _collection.Where(r => r.Value.CurrencyPair == pairName && r.Value.Frame == frame).SingleOrDefault();
                if (!elementForRemove.Equals(default(KeyValuePair<int, CurrencyTuple>)))
                {
                    _collection.Remove(elementForRemove.Key);
                }
            }
        }

        public CurrencyTuple GetPairByParams(string pairName, int frame)
        {
            IEnumerable<CurrencyTuple> snapshot;

            bool LocalCondition(CurrencyTuple r)
            {
                return r.CurrencyPair.Equals(pairName, StringComparison.InvariantCulture) &&
                       r.Frame == frame;
            }

            lock (_syncObject)
            {
                snapshot = _collection.Values;
            }

            return snapshot.Any(LocalCondition) ? snapshot.Single(LocalCondition) : default;
        }

        public CurrencyTuple GetPairByChannel(int channelId)
        {
            lock (_syncObject)
            {
                var element = _collection.SingleOrDefault(r => r.Value.ChannelId == channelId);
                return !element.Equals(default) ? element.Value : default;
            }
        }

        public CurrencyTuple GetPairByPingPongCID(int cid)
        {
            lock (_syncObject)
            {
                var searchedElement = _collection.Values.Where(r => r.CID == cid).SingleOrDefault();
                return searchedElement != default ? searchedElement : default;
            }
        }
    }
}
