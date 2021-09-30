using BlogApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly BlogContext _context;

        public LoginController(BlogContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Superuser user)
        {

            //var admin = _context.Superusers.FirstOrDefault();
            //if (user.KullaniciAdi == admin.KullaniciAdi && user.Sifre == admin.Sifre)
            //{
            //    //Sessiona giriş yapan kullanıcının id'sini kayıt eder.
            //    HttpContext.Session.SetString("Login", (user.KullaniciAdi).ToString());
            //    return RedirectToAction("Index", "Superuser");
            //}
            bool IsCorrect = _context.Superusers.Any(s => s.KullaniciAdi ==user.KullaniciAdi && s.Sifre ==user.Sifre);
            if (IsCorrect)
            {
                //Sessiona giriş yapan kullanıcının id'sini kayıt eder.
                HttpContext.Session.SetString("Login", (user.Id).ToString());
                return RedirectToAction("Index", "Superuser");
            }
            else
            {
                return View();
            }
        }
    }
}
