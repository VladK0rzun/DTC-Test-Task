namespace WpfApp1.Models
{
    public class Currency
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Supply { get; set; }
        public decimal MarketCapUsd { get; set; }
        public decimal ChangePercent24Hr { get; set; }
    }
}
