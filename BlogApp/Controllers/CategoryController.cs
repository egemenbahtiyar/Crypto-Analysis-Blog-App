using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogApp.Models;
using BlogApp.Filters;

namespace BlogApp.Controllers
{

    public class CategoryController : Controller
    {
        private readonly BlogContext _context;

        public CategoryController(BlogContext context)
        {
            _context = context;
        }

        //public IViewComponentResult Invoke()
        //{
        //    return ViewComponent("KategoriListViewComponent","Category");
        //}

        // GET: Category
        [IsLogin]
        public async Task<IActionResult> Index()
        {
            var categories = _context.Categories
                .Select(i => new CategoryModel()
                {

                    KategoriAdi = i.KategoriAdi,
                    BlogSayisi = i.Bloglar.Count(),
                    Id = i.Id

                }
                );
            return View(await categories.ToListAsync());
        }

        // GET: Category/Details/5
        [IsLogin]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        [IsLogin]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [IsLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KategoriAdi")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Categories.Any(r=>r.KategoriAdi==category.KategoriAdi))
                {
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ViewBag.Message = "Bu kategori zaten var.";
                    return View();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [IsLogin]
        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [IsLogin]
        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KategoriAdi")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Delete/5
        [IsLogin]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [IsLogin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
