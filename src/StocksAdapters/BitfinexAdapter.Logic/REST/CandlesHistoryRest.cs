using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using BitfinexAdapter.Logic.Converters;
using BitfinexAdapter.Logic.Models.Candles;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace BitfinexAdapter.Logic.REST
{
    internal class CandlesHistoryRest : ICandlesHistoryRest
    {
        private readonly HttpClient _client;
        private readonly ILogger<CandlesHistoryRest> _logger;
        private readonly JsonSerializerSettings _settings;

        public CandlesHistoryRest(HttpClient client, ILogger<CandlesHistoryRest> logger)
        {
            _client = client;
            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new CandleJsonConverter());
            _logger = logger;
        }

        public async Task<IEnumerable<CandleModel>> GetCandleHistoryAsync(string pairName, int period)
        {
            try
            {
                var response = await _client.GetAsync($"https://api-pub.bitfinex.com/v2/candles/trade:{period}m:t{pairName}/hist/?limit=2000");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<CandleModel>>(content, _settings);
                }
                else
                {
                    _logger.LogError($"В процессы обработки запроса получения свечей произошла ошибка: {response.StatusCode}:{response.ReasonPhrase}");
                    return Enumerable.Empty<CandleModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "В процессе получения свечей произошла ошибка");
                throw ex;
            }
        }
    }
}
