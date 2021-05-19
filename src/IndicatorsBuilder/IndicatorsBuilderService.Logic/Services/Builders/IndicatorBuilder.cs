using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using IndicatorBuilderService.Logic.Converters;

using Models.Business.Indicators;
using Models.SystemModels;

using Newtonsoft.Json;

namespace IndicatorsBuilderService.Logic.Services.Builders
{
    internal class IndicatorBuilder : IIndicatorBuilder
    {
        private const string TAServiceHost = "TA_SERVICE_HOST";
        private const string TAServicePort = "TA_SERVICE_PORT";

        /// <summary>
        /// Send candles and indicators list pack to rest service for build indicators
        /// </summary>
        /// <param name="data">indicators list and candles data</param>
        /// <param name="localhost">host rest service</param>
        /// <param name="port">port rest service</param>
        /// <param name="cancellationToken">token</param>
        /// <returns>constructed indicators</returns>
        public async Task<IEnumerable<Indicator>> GetIndicatorsByCandleAsync(IndicatorServicePack data, CancellationToken cancellationToken = default)
        {
            if (data == null)
            {
                throw new ArgumentNullException("Входной параметр со списком индикаторов и массивом свечей не может быть null", nameof(data));
            }

            var localhost = Environment.GetEnvironmentVariable(TAServiceHost);
            var port = int.Parse(Environment.GetEnvironmentVariable(TAServicePort));

            if (string.IsNullOrWhiteSpace(localhost) ||
                string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(TAServicePort)))
            {
                throw new ArgumentException($"Параметр хоста {nameof(localhost)} и порта {nameof(port)} не может быть пустым");
            }

            using var webSocket = new ClientWebSocket();
            await webSocket.ConnectAsync(new Uri($"ws://{localhost}:{port}"), cancellationToken);
            await SendRequestAsync(data, webSocket);
            var stringForDeserialize = await ReceiveDataAsync(webSocket, cancellationToken);

            if (stringForDeserialize.Equals("incorrect request data scheme", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException($"По какой то причине не построились индикаторы: {stringForDeserialize}");
            }

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new IndicatorJsonConverter());
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "well done", cancellationToken);
            return JsonConvert.DeserializeObject<IEnumerable<Indicator>>(stringForDeserialize.Replace("nan", "0"),
                                                                         settings);
        }

        private async Task<string> ReceiveDataAsync(ClientWebSocket webSocket, CancellationToken cancellationToken)
        {
            var totalReceivedBuffer = new List<byte>();
            var receivedBuffer = new byte[1024 * 50];

            var resultOfRequest = await webSocket.ReceiveAsync(new ArraySegment<byte>(receivedBuffer),
                                                                                  cancellationToken);
            totalReceivedBuffer.AddRange(receivedBuffer);
            Array.Clear(receivedBuffer, 0, receivedBuffer.Length);

            while (!resultOfRequest.EndOfMessage)
            {
                resultOfRequest = await webSocket.ReceiveAsync(new ArraySegment<byte>(receivedBuffer),
                                                               cancellationToken);
                totalReceivedBuffer.AddRange(receivedBuffer);
                Array.Clear(receivedBuffer, 0, receivedBuffer.Length);
            }
            return Encoding.UTF8.GetString(totalReceivedBuffer.ToArray());
        }

        private async Task SendRequestAsync(IndicatorServicePack data, ClientWebSocket webSocket)
        {
            var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length),
                                      WebSocketMessageType.Binary,
                                      true,
                                      CancellationToken.None);
        }
    }
}
