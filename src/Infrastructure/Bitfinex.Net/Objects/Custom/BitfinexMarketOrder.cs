using Bitfinex.Net.Converters;
using Bitfinex.Net.Objects.Custom;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bitfinex.Net.Objects
{

    /// <summary>
    /// Order info
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class BitfinexMarketOrder
    {
        /// <summary>
        /// The id of the order
        /// </summary>
        //[ArrayProperty(0), JsonConverter(typeof(TimestampConverter))]
        //public DateTime MTC { get; set; }

        /// <summary>
        /// Purpose of notification ('on-req', 'oc-req', 'uca', 'fon-req', 'foc-req')
        /// </summary>
        [ArrayProperty(1)]
        public string? TypeOfOrderMessage { get; set; }

        /// <summary>
        /// Message id
        /// </summary>
        [ArrayProperty(2)]
        public long? MessageID { get; set; }

        /// <summary>
        /// Order ID
        /// </summary>
        [ArrayProperty(3)]
        public long? OrderId { get; set; }

        /// <summary>
        /// The creation time of the order
        /// </summary>
        [ArrayProperty(4), JsonConverter(typeof(OrderBodyConverter))]
        public List<BitfinexOrderBody>? Body { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [ArrayProperty(6)]
        public string? Status { get; set; }
    }
}
