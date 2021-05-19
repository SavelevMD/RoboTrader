using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using ArrayConverter = CryptoExchange.Net.Converters.ArrayConverter;

namespace Bitfinex.Net.Objects.Custom
{
    /// <summary>
    /// Body of one transaction in order
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class BitfinexOrderBody
    {
        /// <summary>
        /// order id
        /// </summary>
        [ArrayProperty(0)]
        public long? Id { get; set; }

        /// <summary>
        /// Group id
        /// </summary>
        [ArrayProperty(1)]
        public long? GId { get; set; }

        /// <summary>
        /// Group id
        /// </summary>
        [ArrayProperty(2)]
        public long? CId { get; set; }

        /// <summary>
        /// Pair name
        /// </summary>
        [ArrayProperty(3)]
        public string? Symbol { get; set; }

        /// <summary>
        /// Millisecond timestamp of creation
        /// </summary>
        [ArrayProperty(4), JsonConverter(typeof(TimestampConverter))]
        public DateTime MTSCreate { get; set; }

        /// <summary>
        /// Millisecond timestamp of update
        /// </summary>
        [ArrayProperty(5), JsonConverter(typeof(TimestampConverter))]
        public DateTime MTSUpdate { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [ArrayProperty(6)]
        public decimal Amount { get; set; }

        /// <summary>
        /// Amount orig
        /// </summary>
        [ArrayProperty(7)]
        public decimal AmountOrig { get; set; }

        /// <summary>
        /// Type of order
        /// </summary>
        [ArrayProperty(8)]
        public string? TypeOfOrder { get; set; }

        /// <summary>
        /// Type of order previous
        /// </summary>
        [ArrayProperty(9)]
        public string? TypePrev { get; set; }

        /// <summary>
        /// Order status
        /// </summary>
        [ArrayProperty(13)]
        public string? OrderStatus { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [ArrayProperty(16)]
        public decimal Price { get; set; }
    }
}
