using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileShop.Data;
using MobileShop.Models;
using System.Linq;

namespace MobileShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password, string returnUrl = null)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.FullName ?? "Customer");
                HttpContext.Session.SetString("UserEmail", user.Email ?? "");
                HttpContext.Session.SetString("UserRole", user.Role ?? "Customer");

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                // If returnUrl exists, redirect to requested page (e.g. MyOrders or Checkout)
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                // Default fallback is Home Index
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid email or password!";
            return View();
        }

        [HttpPost]
        public IActionResult LoginWithOtp(string phone, string returnUrl = null)
        {
            var user = _context.Users.FirstOrDefault(u => u.Phone == phone);
            if (user == null)
            {
                user = new User
                {
                    FullName = "User_" + phone,
                    Email = phone + "@techshop.com",
                    Phone = phone,
                    Role = "Customer"
                };
                _context.Users.Add(user);
                _context.SaveChanges();
            }

            HttpContext.Session.SetString("UserName", user.FullName ?? "Customer");
            HttpContext.Session.SetString("UserEmail", user.Email ?? "");
            HttpContext.Session.SetString("UserRole", user.Role);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(user.Role))
                {
                    user.Role = "Customer";
                }

                _context.Users.Add(user);
                _context.SaveChanges();

                HttpContext.Session.SetString("UserName", user.FullName ?? "Customer");
                HttpContext.Session.SetString("UserEmail", user.Email ?? "");
                HttpContext.Session.SetString("UserRole", user.Role);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        public IActionResult Profile()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Content("<script>localStorage.removeItem('techShopCart'); sessionStorage.clear(); window.location.href='/Home/Index';</script>", "text/html");
        }
    }
}