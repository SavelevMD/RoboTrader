using System;
using System.Collections.Generic;
using System.Linq;

using Bogus;

using Models.Business;

namespace Generator.Candles
{
    public static class CandleGenerator
    {
        public static CandlesModel GetCandles(int count)
        {
            var candles = new Faker<CandleModel>()
                .RuleFor(r => r.Close, r => r.Random.Decimal(1000, 2000))
                .RuleFor(r => r.Open, r => r.Random.Decimal(1000, 2000))
                .RuleFor(r => r.High, r => r.Random.Decimal(1000, 2000))
                .RuleFor(r => r.Low, r => r.Random.Decimal(1000, 2000))
                .RuleFor(r => r.TimeFrame, r => "1m")
                .RuleFor(r => r.Volume, r => r.Random.Decimal(0, 50000));

            var generatedCandles = candles.Generate(count);

            var aaa = GetDate(count).Zip(generatedCandles, (d, c) => { c.ReceiptTime = d; return c; });

            return new CandlesModel
            {
                CandleCollection = aaa.ToList(),
                ChannelId = 666,
                CurrencyName = "ETHUSD",
                TimeFrame = 1
            };
        }

        public static IEnumerable<DateTime> GetDate(int rangeLimit)
        {
            var startDate = DateTime.Now.AddMinutes(-rangeLimit);
            startDate = startDate.AddSeconds(-startDate.Second);
            foreach (var d in Enumerable.Range(0, rangeLimit))
            {
                yield return startDate.AddMinutes(d);
            }
        }
    }
}
