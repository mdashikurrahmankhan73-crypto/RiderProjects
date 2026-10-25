using System.ComponentModel.DataAnnotations;

namespace MobileShop.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal TotalPrice { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? ZipCode { get; set; }
        public string PaymentMethod { get; set; } = "Cash on Delivery";
        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}