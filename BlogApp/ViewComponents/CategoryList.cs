using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly BlogContext _context;

        public CategoryListViewComponent(BlogContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _context.Categories;
            return View(categories);
        } 
    }
}
