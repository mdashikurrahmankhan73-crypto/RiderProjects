using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MobileShop.Models;

namespace MobileShop.Controllers
{
    public class AdminController : Controller
    {
        // GET: /Admin/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Admin/Login
        [HttpPost]
        public IActionResult Login(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Dummy Admin verification (আপনি চাইলে এখানে DB check করতে পারেন)
                if (model.Email == "admin@gulshan.com" && model.Password == "admin123")
                {
                    HttpContext.Session.SetString("UserRole", "Admin");
                    HttpContext.Session.SetString("UserName", "Admin");
                    TempData["SuccessMessage"] = "Successfully Logged In as Admin!";
                    return RedirectToAction("Dashboard");
                }
                
                // সাধারণ সেশন টেস্টের জন্য
                HttpContext.Session.SetString("UserRole", "Admin");
                HttpContext.Session.SetString("UserName", model.Email.Split('@')[0]);
                TempData["SuccessMessage"] = "Successfully Logged In!";
                return RedirectToAction("Dashboard");
            }

            return View(model);
        }

        // POST: /Admin/Register
        [HttpPost]
        public IActionResult Register(AdminRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // এখানে ডাটাবেজে সেভ করার কোড যুক্ত করবেন
                TempData["SuccessMessage"] = "Admin Account Created Successfully! Please Login.";
                return RedirectToAction("Login");
            }
            TempData["ErrorMessage"] = "Registration failed. Please check the inputs.";
            return RedirectToAction("Login");
        }

        // GET: /Admin/Dashboard
        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}