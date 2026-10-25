using System.ComponentModel.DataAnnotations;

namespace MobileShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        public string? Name { get; set; }
        public string? Model { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public string? MainImageUrl { get; set; }
        public string? Description { get; set; }
        
        public bool IsNewArrival { get; set; }
        public bool IsBestseller { get; set; }

        // ভিউ ফাইলগুলোর এরর দূর করার জন্য প্রয়োজনীয় প্রোপার্টিগুলো:
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string? Specifications { get; set; }
        public string? ShortDescription { get; set; }
        public int StockQuantity { get; set; }
        public bool IsFeatured { get; set; }
    }
}