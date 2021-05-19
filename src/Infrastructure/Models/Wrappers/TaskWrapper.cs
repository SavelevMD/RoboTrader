using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.Tasks
{
    public class TransitionElement
    {
        [JsonProperty("from")]
        public int From { get; set; }

        [JsonProperty("to")]
        public int To { get; set; }

        [JsonProperty("false_condition_result")]
        public int? IfNotTrue { get; set; }

        [JsonProperty("condition")]
        public string Condition { get; set; }

        [JsonProperty("back_to")]
        public int? BackTo { get; set; }
    }

    public class Transition
    {
        [JsonProperty("buy")]
        public IEnumerable<TransitionElement> Buy { get; set; }

        [JsonProperty("sell")]
        public IEnumerable<TransitionElement> Sell { get; set; }
    }

    public class SuperCondition
    {
        [JsonProperty("buy")]
        public string Buy { get; set; }

        [JsonProperty("sell")]
        public string Sell { get; set; }
    }

    public class TaskWrapper
    {
        [JsonProperty("buy_conditions")]
        public Dictionary<int, string> BuyConditions { get; set; }

        [JsonProperty("sell_conditions")]
        public Dictionary<int, string> SellConditions { get; set; }

        [JsonProperty("transitions")]
        public Transition Transitions { get; set; }

        [JsonProperty("cur_pair")]
        public string CurPair { get; set; }

        [JsonProperty("time_frame")]
        public int TimeFrame { get; set; }

        [JsonProperty("thread_name")]
        public string ThreadName { get; set; }

        [JsonProperty("p_Margine")]
        public decimal P_Margine { get; set; }

        [JsonProperty("amount_in")]
        public decimal AmountIn { get; set; }

        [JsonProperty("amount_out")]
        public decimal AmountOut { get; set; }

        [JsonProperty("limit_trade_amount")]
        public decimal LimitTradeAmount { get; set; }

        [JsonProperty("indicators")]
        public List<string> Indicators { get; set; }

        [JsonProperty("is_not_real_task")]
        public bool IsNotRealTask { get; set; }

        [JsonProperty("supercondition")]
        public SuperCondition SuperConditions { get; set; }
    }
}
