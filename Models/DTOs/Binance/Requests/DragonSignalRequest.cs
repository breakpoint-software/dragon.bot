namespace Models.DTOs.Binance.Requests
{
    public class DragonSignalRequest

    {
        public string Ticker { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public string Side { get; set; }
        public string OrderId { get; set; }
    }
}
