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
    }

    public enum OrderPosition { Short, Long }
}
