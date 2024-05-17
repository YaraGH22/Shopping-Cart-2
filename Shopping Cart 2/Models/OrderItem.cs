//using System.ComponentModel.DataAnnotations.Schema;

//namespace Shopping_Cart_2.Models
//{
//    public class OrderItem
//    {
//        // this class for M -> M relation
//        [ForeignKey("Item")]
//        public int ItemId { get; set; }
//        [ForeignKey("Order")]
//        public int OrderId { get; set; }

//        public Order Order { get; set; } = default!;
//        public Item Item { get; set; } = default!;
//    }
//}
//wewe