namespace Models.SystemModels
{
    /// <summary>
    /// Специфицирует ноду переходов
    /// </summary>
    public class TaskTransition
    {
        /// <summary>
        /// Id ноды откуда
        /// </summary>
        public int From { get; set; }
        /// <summary>
        /// Id ноды куда
        /// </summary>
        public int To { get; set; }

        /// <summary>
        /// Если условие не сработало
        /// </summary>
        public int? ThenConditionIsNotTrue { get; set; }
        /// <summary>
        /// Если условие равно True, тогда переходим в узел to: иначе в узел 
        /// </summary>
        public string Condition { get; set; }

        public int? BackTo { get; set; }
    }
}
