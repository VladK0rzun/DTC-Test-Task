namespace WpfApp1.Models
{
    public class CurrencyDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal PriceUsd { get; set; }
        public decimal VolumeUsd24Hr { get; set; }
        public decimal ChangePercent24Hr { get; set; }
        public decimal MarketCapUsd { get; set; }
    }
}
