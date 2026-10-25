using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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

        // GET: /Home/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Marketplace
        public IActionResult Marketplace()
        {
            return View();
        }

        // GET: /Home/Corporate
        public IActionResult Corporate()
        {
            return View();
        }

        // GET: /Home/Offers
        public IActionResult Offers()
        {
            return View();
        }

        // GET: /Home/Event
        public IActionResult Event()
        {
            return View();
        }

        // GET: /Home/Blog
        public IActionResult Blog()
        {
            return View();
        }

        // GET: /Home/Privacy
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