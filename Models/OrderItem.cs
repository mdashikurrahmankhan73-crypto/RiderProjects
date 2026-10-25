using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileShop.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Discount")]
        public decimal Discount { get; set; }

        // 👇 এই অংশটি ঠিক করা হয়েছে 👇
        // ক্যালকুলেটেড প্রপার্টি থেকে [Required] এবং [Column] সরিয়ে দেওয়া হয়েছে
        [Display(Name = "Total Price")]
        public decimal TotalPrice => (UnitPrice * Quantity) - Discount;

        // Foreign Keys
        [Display(Name = "Order")]
        public int OrderId { get; set; }

        [Display(Name = "Product")]
        public int ProductId { get; set; }

        // Navigation Properties
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
}