using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileShop.Data;
using MobileShop.Models;
using System.Linq;

namespace MobileShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Dashboard
        public IActionResult Dashboard()
        {
            var products = _context.Products.ToList();
            ViewBag.Orders = _context.Orders.ToList();
            return View(products);
        }

        // GET: /Admin/CreateProduct
        public IActionResult CreateProduct()
        {
            return View();
        }

        // POST: /Admin/CreateProduct
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View(product);
        }

        // POST: /Admin/DeleteProduct/5
        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }
    }
}