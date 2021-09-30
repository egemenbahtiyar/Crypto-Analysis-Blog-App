using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.ViewComponents
{
    public class IndexNewsViewComponent : ViewComponent
    {
        private readonly BlogContext _context;

        public IndexNewsViewComponent(BlogContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var news = _context.News;
            return View(news);
        }
    }
}
