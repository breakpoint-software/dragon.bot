namespace Models.DTOs.Binance.Responses
{
    public class DragonSignalResponse
    {
        public string Symbol { get; set; }
        public string Position { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Fee { get; set; }
        public decimal UsdAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public long BinanceOrderId { get; set; }
        public string SignalOrderId { get; set; }
    }
}
