using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_Cart_2.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; } = default!;
        public bool IsDeleted { get; set; } = false;
        public List<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
        
       // public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
