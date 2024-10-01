using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string Pair { get; set; }
        public OrderPosition Position { get; set; }
        [Column(TypeName = "decimal(16, 8)")]
        public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(16, 8)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(16, 8)")]
        public decimal Fee { get; set; }
        [Column(TypeName = "decimal(16, 2)")]
        public decimal UsdAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public long BinanceOrderId { get; set; }
        public string SignalOrderId { get; set; }
    }

    public enum OrderPosition
    {
        Sell,
        Buy
    }
    public enum OrderStatus
    {
        /// <summary>
        /// Order is new
        /// </summary>
        New,
        /// <summary>
        /// Order is partly filled, still has quantity left to fill
        /// </summary>
        PartiallyFilled,
        /// <summary>
        /// The order has been filled and completed
        /// </summary>
        Filled,
        /// <summary>
        /// The order has been canceled
        /// </summary>
        Canceled,
        /// <summary>
        /// The order is in the process of being canceled  (currently unused)
        /// </summary>
        PendingCancel,
        /// <summary>
        /// The order has been rejected
        /// </summary>
        Rejected,
        /// <summary>
        /// The order has expired
        /// </summary>
        Expired,
        /// <summary>
        /// Liquidation with Insurance Fund
        /// </summary>
        Insurance,
        /// <summary>
        /// Counterparty Liquidation
        /// </summary>
        Adl,
        /// <summary>
        /// Expired because of trigger SelfTradePrevention
        /// </summary>
        ExpiredInMatch
    }
}
