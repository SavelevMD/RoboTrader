using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models.Business.Indicators
{
    /// <summary>
    /// Определяет значение индикатора на определенном отрезке времени заданном в списке <paramref name="DateTimeValueSegment"/>
    /// </summary>
    public class Indicator
    {
        /// <summary>
        /// Имя индикатора
        /// </summary>
        public string IndicatorName { get; set; }
        /// <summary>
        /// Значения индикатора за определенный отрезок времени
        /// </summary>
        [JsonProperty("IndicatorValues")]
        public IEnumerable<IndicatorValue> DateTimeValueSegment { get; set; }
    }
}
