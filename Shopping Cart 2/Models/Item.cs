using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_Cart_2.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public string Cover {  get; set; } = string.Empty;

        //for M -> 1 items -> category  
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; } = 0;
        public Category Category { get; set; } = default!;

        // for M -> M
       // public ICollection<OrderItem> Orders { get; set; } = new List<OrderItem>();

        public List<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
        public List<CartDetail> CartDetail { get; set; } = new List<CartDetail>();

    }
}
