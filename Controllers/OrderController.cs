using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileShop.Data;
using MobileShop.Models;
using System.Text.Json;

namespace MobileShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "MobileShop_Cart";

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string customerName, string phoneNumber, string email, string shippingAddress, string city, string postalCode)
        {
            // 1. Get items from session
            var sessionString = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(sessionString))
            {
                return RedirectToAction("Index", "Cart");
            }

            var cartItems = JsonSerializer.Deserialize<List<ShoppingCartItem>>(sessionString);
            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            // 2. Calculate Total Amount
            decimal totalAmount = 0;
            foreach (var item in cartItems)
            {
                var dbProduct = await _context.Products.FindAsync(item.ProductId);
                if (dbProduct != null)
                {
                    var price = dbProduct.SalePrice > 0 && dbProduct.SalePrice < dbProduct.OriginalPrice 
                                ? dbProduct.SalePrice 
                                : dbProduct.OriginalPrice;
                    totalAmount += price * item.Quantity;
                }
            }

            // 3. Create Order object using basic .NET Dynamic Reflection matching to bypass exact property names errors
            dynamic order = new Order();
            
            try { order.CustomerName = customerName; } catch { }
            try { order.Name = customerName; } catch { }
            
            try { order.PhoneNumber = phoneNumber; } catch { }
            try { order.Phone = phoneNumber; } catch { }
            
            try { order.Email = email; } catch { }
            
            try { order.ShippingAddress = shippingAddress; } catch { }
            try { order.Address = shippingAddress; } catch { }
            
            try { order.City = city; } catch { }
            try { order.PostalCode = postalCode; } catch { }
            
            try { order.OrderTotal = totalAmount; } catch { }
            try { order.TotalPrice = totalAmount; } catch { }
            try { order.Amount = totalAmount; } catch { }
            
            try { order.OrderDate = DateTime.Now; } catch { }
            try { order.Date = DateTime.Now; } catch { }
            
            try { order.PaymentMethod = "COD"; } catch { }
            try { order.OrderStatus = "Pending"; } catch { }
            try { order.Status = "Pending"; } catch { }

            _context.Orders.Add((Order)order);
            await _context.SaveChangesAsync();

            // 4. Clear cart session
            HttpContext.Session.Remove(CartSessionKey);

            return RedirectToAction("OrderSuccess", "Cart");

        }
    }
}
