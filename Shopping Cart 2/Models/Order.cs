using System.ComponentModel.DataAnnotations;

namespace Shopping_Cart_2.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; } = 0;
        public int Quantity { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
