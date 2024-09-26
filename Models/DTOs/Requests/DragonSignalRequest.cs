namespace Models.DTOs.Requests
{
    public class DragonSignalRequest

    {
        public string Ticker { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        //[FirestoreProperty]
        //public string Interval { get; set; }
        public string Side { get; set; }
    }
}
