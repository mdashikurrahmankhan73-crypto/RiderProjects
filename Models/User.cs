using System.ComponentModel.DataAnnotations;

namespace MobileShop.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string? FullName { get; set; }
        
        [Required]
        public string? Email { get; set; }
        
        public string? Password { get; set; }
        
        public string? Phone { get; set; }
        
        public string Role { get; set; } = "Customer";
    }
}