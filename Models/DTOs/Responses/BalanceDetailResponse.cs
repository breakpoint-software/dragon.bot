namespace Models.DTOs.Responses
{
    public class BalanceDetailResponse
    {
        public int OrderId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal AccountBalance { get; set; }
        public string Position { get; set; }
        public decimal UsdAmount { get; set; }
    }
}
