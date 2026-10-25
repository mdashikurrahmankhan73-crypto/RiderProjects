using Microsoft.AspNetCore.Mvc;
using MobileShop.Data;
using MobileShop.Models;
using System;
using System.Linq;

namespace MobileShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Product/Details/5
        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: /Product/Checkout/5
        public IActionResult Checkout(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: /Product/PlaceOrder
        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now; // DateTime টাইপের সাথে সরাসরি মিলে যাবে
                order.PaymentMethod = "Cash on Delivery";

                _context.Orders.Add(order);
                _context.SaveChanges();

                return RedirectToAction("OrderSuccess", new { id = order.Id });
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == order.ProductId);
            return View("Checkout", product);
        }

        // GET: /Product/OrderSuccess/5
        public IActionResult OrderSuccess(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(order);
        }
    }
}