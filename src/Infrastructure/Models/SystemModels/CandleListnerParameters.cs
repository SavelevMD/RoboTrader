namespace Models.SystemModels
{
    /// <summary>
    /// Модель аргументов для прослушки канала
    /// </summary>
    public class CandleListnerParameters
    {
        public string ChannelName { get; set; }
        //TODO: тикет #34 перевести на int
        public int Period { get; set; }
    }
}
