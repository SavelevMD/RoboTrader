using System;
using System.Collections.Generic;

using BitfinexAdapter.Logic.Models.Candles;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BitfinexAdapter.Logic.Converters
{
    public class CandleJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        //TODO: разобраться что тут не так с if, почему больше 6?
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jArray = JArray.Load(reader);
            var target = new List<CandleModel>();
            //пришло много свечей, больше одной
            if (jArray.Count > 6)
            {
                foreach (var childJArray in jArray.Children<JArray>())
                {
                    var item = CreateCandle(childJArray);
                    target.Add(item);
                }
            }
            else
            {
                target.Add(CreateCandle(jArray));
            }
            return target;
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
                Volume = (decimal)childJArray[5],
                TimeFrame = "1m",
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
