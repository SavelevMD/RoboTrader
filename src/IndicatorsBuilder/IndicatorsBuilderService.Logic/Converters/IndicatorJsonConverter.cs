using System;
using System.Collections.Generic;
using System.Linq;

using Models.Business.Indicators;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IndicatorBuilderService.Logic.Converters
{
    internal class IndicatorJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var convertingResult = new List<Indicator>();
            var obj = JObject.Load(reader);
            foreach (var ind in obj)
            {
                switch (ind.Key)
                {
                    //потому что особенный
                    case var _ when ind.Key.Contains("i_MACD"):
                        var macdInstance = ConvertMACD(ind.Value as JObject);
                        var convertedIndicatorsName = convertingResult.Select(r => r.IndicatorName);
                        var newIndicatorsName = macdInstance.Select(r => r.IndicatorName);

                        if (convertedIndicatorsName.Except(newIndicatorsName).Any() ||
                            newIndicatorsName.Except(convertedIndicatorsName).Any())
                        {
                            convertingResult.AddRange(macdInstance);
                        }
                        break;
                    default:
                        convertingResult.Add(new Indicator()
                        {
                            IndicatorName = ind.Key,
                            DateTimeValueSegment = ind.Value
                                                      .Cast<JProperty>()
                                                      .Select(r => CreateIndicatorValue(r))
                        });
                        break;
                }
            }
            return convertingResult;
        }

        private IEnumerable<Indicator> ConvertMACD(JObject data)
        {
            var result = new List<Indicator>();
            if (data == null)
            {
                throw new ArgumentException("Входные данные по MACD не могут быть null", nameof(data));
            }

            foreach (var macdBit in data)
            {
                result.Add(new Indicator()
                {
                    IndicatorName = macdBit.Key,
                    DateTimeValueSegment = macdBit.Value.Cast<JProperty>()
                                                  .Select(r => CreateIndicatorValue(r))
                });
            }
            return result;
        }

        private static IndicatorValue CreateIndicatorValue(JProperty r) 
            => new IndicatorValue()
            {
                ValueDatetTime = DateTime.Parse(r.Name),
                Value = (decimal)r.Value
            };

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) 
            => throw new NotImplementedException();
    }
}
