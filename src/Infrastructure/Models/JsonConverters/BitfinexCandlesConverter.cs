using Models.Business;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.JsonConverters
{
    public class BitfinexCandlesConverter : JsonConverter
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = JArray.Load(reader);
            var result = new CandlesModel();

            if (data.Count >= 2)
            {
                result.ChannelId = (int)data[0];
                result.CandleCollection = new List<CandleModel>();
                if (((JArray)data[1]).Count > 6)
                {
                    result.CandleCollection = data[1].Select(r => CreateCandle((JArray)r))
                        .ToList();
                }
                else
                {
                    result.CandleCollection = new CandleModel[] { CreateCandle((JArray)data[1]) };
                    result.StockTime = DateTime.UnixEpoch.AddMilliseconds((long)data[2]);
                }
            }
            return result;
        }

        private CandleModel CreateCandle(JArray childJArray)
        {
            return new CandleModel()
            {
                ReceiptTime = DateTime.UnixEpoch.AddMilliseconds((long)childJArray[0]),
                Open = (decimal)childJArray[1],
                Close = (decimal)childJArray[2],
                High = (decimal)childJArray[3],
                Low = (decimal)childJArray[4],
                Volume = (decimal)childJArray[5]
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var result = value.GetType()
                .GetProperties()
                .Aggregate(new JObject(), (result, info) =>
                {
                    if (info.CanRead)
                    {
                        var propVal = info.GetValue(value);
                        if (propVal != default)
                        {
                            result.Add(info.Name, JToken.FromObject(propVal, serializer));
                        }
                    }

                    return result;
                });

            result.WriteTo(writer);
        }
    }
}
