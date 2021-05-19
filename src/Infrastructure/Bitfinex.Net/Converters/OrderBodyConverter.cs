using Bitfinex.Net.Objects.Custom;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Bitfinex.Net.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderBodyConverter : JsonConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<>);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(JToken))
                return JToken.Load(reader);

            var result = Activator.CreateInstance(objectType);
            var arr = JArray.Load(reader);
            return ParseObject(arr, result, objectType);
        }

        private object? ParseObject(JArray arr, object result, Type objectType)
        {
            foreach (var property in objectType.GetProperties())
            {
                var attribute =
                    (ArrayPropertyAttribute)property.GetCustomAttribute(typeof(ArrayPropertyAttribute));

                var value = arr[attribute.Index].ToObject<List<BitfinexOrderBody>>();
                property.SetValue(result, value);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
