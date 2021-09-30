using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly BlogContext _context;

        public HomeController(BlogContext context)
        {
            _context = context;
        }

   
        public IActionResult Index()
        {
            var bloglar = _context.Blogs
                .Include(b => b.Category)
                .Where(i => i.Onay == true && i.Anasayfa == true)
                .Select(i => new BlogModel()
                {
                    Id = i.Id,
                    Baslik = i.Baslik,
                    Aciklama = i.Aciklama,
                    EklenmeTarihi = i.EklenmeTarihi,
                    Onay = i.Onay,
                    Anasayfa = i.Anasayfa,
                    Resim = i.Resim,
                    KategoriAdi = i.Category.KategoriAdi

                });
            return View(bloglar.OrderByDescending(bloglar => bloglar.EklenmeTarihi).Take(7).ToList());

        }

       
        public IActionResult Contact()
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
