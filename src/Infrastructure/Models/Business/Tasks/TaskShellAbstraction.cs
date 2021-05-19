using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models.Business.Tasks
{
    public class Indicator
    {
        public string Name { get; set; }
        public int Frame { get; set; }
        public int LoopBack { get; set; }
    }

    public class TransitionScope
    {
        public IEnumerable<TaskTransition> Buy { get; set; }

        public IEnumerable<TaskTransition> Sell { get; set; }
    }

    public class Supercondition
    {
        public string Buy { get; set; }

        public string Sell { get; set; }
    }

    //TODO: подумать как защитить бизнес важные поля от того что бы их случайно не 
    public class TaskShellAbstraction
    {
        public decimal StockFee { get; } = 0.002m;

        /// <summary>
        /// Баланс для операции покупки (вход)
        /// </summary>
        public decimal BuyBalance { get; set; }

        /// <summary>
        /// Баланс для операции продажи (выход)
        /// </summary>
        public decimal SellBalance { get; set; }

        /// <summary>
        /// Предельное значение прибыли, выше которого заработанная часть отправляется на счет текущего таска
        /// </summary>
        public decimal LimitTradeAmount { get; set; }

        /// <summary>
        /// Сохраненный остаток текущего таска, инкапсулирует в себе всю прибыль с каждого цикла, sum(AmointIn - LimitTradeAmount)
        /// </summary>
        public decimal SavedValue { get; set; }

        /// <summary>
        /// Условия покупки
        /// Key = StatesKey
        /// Value = условие которое нужно вычислить
        /// </summary>
        public Dictionary<int, string> BuyCondition { get; set; }

        /// <summary>
        /// Условия продажи
        /// Key = StatesKey
        /// Value = условие которое нужно вычислить
        /// </summary>
        public Dictionary<int, string> SellCondition { get; set; }

        /// <summary>
        /// Наименование криптовалютной пары
        /// </summary>
        [JsonProperty("cur_pair")]
        public string CurrencyPairName { get; set; }

        /// <summary>
        /// Состояния переходов объекта
        /// </summary>

        public TransitionScope TransitionsScope { get; set; }

        /// <summary>
        /// Текущее состояние объекта == Key (States)
        /// </summary>
        public int CurrentState { get; set; }

        /// <summary>
        /// Пердыдущее состояние объекта == Key (States)
        /// </summary>
        public int LastState { get; set; }

        /// <summary>
        /// Интервал получения цены, используется в подписке на канал свечей
        /// </summary>
        public int TimeFrame { get; set; }

        /// <summary>
        /// Наименование task
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// Значение маржи на основании которой совершаем покупку
        /// </summary>
        public decimal MargineValue { get; set; }

        /// <summary>
        /// Список используемых индикаторов task
        /// </summary>
        public IEnumerable<string> Indicators { get; set; }

        public IEnumerable<Indicator> ProcessedIndicators { get; set; }

        public decimal LastPrice { get; set; }

        public decimal TotalMargine { get; set; }

        public decimal CurrentPrice { get; set; }

        public DateTime ReceiptCandleTime { get; set; }

        private volatile int periodCounter;
        public int PeriodCounter { get => periodCounter; set => periodCounter = value; }

        private volatile int periodLimitCounter;
        public int PeriodLimitCounter { get => periodLimitCounter; set => periodLimitCounter = value; }

        /// <summary>
        /// Начальная сумма, на которую будут осуществляться торги, в том случае, 
        /// если торгуемая пара совпадает с парой(направлением) отдаваемой биржей
        /// Пример: eht -> btc -> eth, биржа отдает candle = ETHBTC, в таком случае AmountIn > 0, иначе = 0
        /// </summary>
        public decimal AmountIn { get; set; }

        /// <summary>
        /// Начальная сумма, на которую будут осуществляться торги, в том случае, 
        /// если торгуемая пара НЕ совпадает с парой(направлением) отдаваемой биржей
        /// Пример: торгуем btc -> eth -> btc, биржа отдает candle = ETHBTC, в таком случае AmountOut > 0, иначе = 0
        /// </summary>
        public decimal AmountOut { get; set; }

        public Dictionary<string, decimal> LastIndicatorsValues { get; set; } = new Dictionary<string, decimal>();

        public CandleModel Candles { get; set; }
        public CandleModel LastCandles { get; set; }

        public bool IsBuyThread => BuyBalance > 0 && BuyCondition.ContainsKey(CurrentState);
        public bool IsNotRealTask { get; set; }

        public Supercondition Supercondition { get; set; }
        public bool IsSuperCondition { get; set; }
        public string CalcSuperCondition { get; set; }
    }
}
