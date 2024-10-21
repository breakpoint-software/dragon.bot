using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain
{
    [Table("Bot")]
    public class Bot
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ShortName { get; set; }
        public bool Active { get; set; }
    }
}
