using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MobileShop.Models;

namespace MobileShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Marketplace()
        {
            return View();
        }

        public IActionResult Corporate()
        {
            return View();
        }

        public IActionResult Offers()
        {
            return View();
        }

        public IActionResult Event()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Details(string name, decimal price, string image)
        {
            ViewBag.ProductName = name ?? "iPhone 15 Pro Max";
            ViewBag.ProductPrice = price > 0 ? price : 1199;
            ViewBag.ProductImage = image ?? "https://images.unsplash.com/photo-1592750475338-74b7b21085ab?w=400";
            return View();
        }

        public IActionResult Checkout(string name, decimal price)
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Home/Checkout" });
            }

            ViewBag.ProductName = name;
            ViewBag.ProductPrice = price;
            return View();
        }

        [HttpPost]
        public IActionResult OrderSuccess()
        {
            TempData["OrderSuccess"] = "Your Order has been placed successfully via Cash on Delivery!";
            return View();
        }

        // GET: /Home/MyOrders (Only accessible for Logged-in Customers)
        public IActionResult MyOrders()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Home/MyOrders" });
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}