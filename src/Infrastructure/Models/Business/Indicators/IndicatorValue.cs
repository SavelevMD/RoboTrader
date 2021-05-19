using System;

using Newtonsoft.Json;

namespace Models.Business.Indicators
{
    /// <summary>
    /// Единица значения индикатора
    /// </summary>
    [JsonObject()]
    public class IndicatorValue
    {
        /// <summary>
        /// Дата/время значения индикатора, играет роль ключа
        /// </summary>
        [JsonProperty("ValueDatetTime")]
        public DateTime ValueDatetTime { get; set; }
        /// <summary>
        /// Значение индикатора в определенный момоент времени заданный в свойстве <paramref name="ValueDateTime"/>
        /// </summary>
        [JsonProperty("Value")]
        public decimal Value { get; set; }

    }
}
