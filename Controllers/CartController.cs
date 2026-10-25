using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileShop.Models;
using MobileShop.Data; // আপনার Data ফোল্ডারের সঠিক নেমস্পেস যুক্ত করা হলো
using System.Text.Json;

namespace MobileShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "MobileShop_Cart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Render Cart Index (Cart/Index)
        public async Task<IActionResult> Index()
        {
            var cartItems = GetCartItemsFromSession();
            
            if (cartItems.Any())
            {
                foreach (var item in cartItems)
                {
                    var dbProduct = await _context.Products.FindAsync(item.ProductId);
                    if (dbProduct != null)
                    {
                        item.Product = dbProduct;
                    }
                }
            }

            SaveCartItemsToSession(cartItems);
            return View(cartItems);
        }

        // 2. Add Item to Cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cartItems = GetCartItemsFromSession();
            var existingItem = cartItems.FirstOrDefault(c => c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cartItems.Add(new ShoppingCartItem
                {
                    Id = cartItems.Count + 1,
                    ProductId = productId,
                    Quantity = quantity,
                    Product = product
                });
            }

            SaveCartItemsToSession(cartItems);
            
            TempData["SuccessMessage"] = $"{product.Name} সফলভাবে কার্টে যোগ করা হয়েছে!";
            return RedirectToAction("Index", "Home");
        }

        // 3. Update Quantity from Cart Page
        [HttpPost]
        public IActionResult UpdateQuantity(int cartItemId, int quantity)
        {
            var cartItems = GetCartItemsFromSession();
            var item = cartItems.FirstOrDefault(c => c.Id == cartItemId);

            if (item != null)
            {
                item.Quantity = quantity;
            }

            SaveCartItemsToSession(cartItems);
            return RedirectToAction(nameof(Index));
        }

        // 4. Remove Item from Cart
        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var cartItems = GetCartItemsFromSession();
            var item = cartItems.FirstOrDefault(c => c.Id == cartItemId);

            if (item != null)
            {
                cartItems.Remove(item);
            }

            SaveCartItemsToSession(cartItems);
            return RedirectToAction(nameof(Index));
        }

        // 5. Render Checkout Page (Cart/Checkout)
        public IActionResult Checkout()
        {
            var cartItems = GetCartItemsFromSession();
            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // 6. Ambiguous এরর দূর করতে সেশন মেথডের নাম এবং ইন্টারনাল লজিক ফিক্স করা হলো
        private List<ShoppingCartItem> GetCartItemsFromSession()
        {
            var sessionString = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(sessionString))
            {
                return new List<ShoppingCartItem>();
            }
            return JsonSerializer.Deserialize<List<ShoppingCartItem>>(sessionString) ?? new List<ShoppingCartItem>();
        }

        private void SaveCartItemsToSession(List<ShoppingCartItem> cartItems)
        {
            var sessionString = JsonSerializer.Serialize(cartItems);
            HttpContext.Session.SetString(CartSessionKey, sessionString);
        }
    }
}
