using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogApp.Models;
using BlogApp.Filters;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BlogApp.Controllers
{
    
    public class NewController : Controller
    {
        private readonly BlogContext _context;
        private readonly IWebHostEnvironment _host;

        public NewController(BlogContext context, IWebHostEnvironment host)
        {
            _context = context;
            this._host = host;
        }
        [IsLogin]
        // GET: New
        public async Task<IActionResult> Index()
        {
            return View(await _context.News.OrderByDescending(r=>r.HaberEklenmeTarihi).ToListAsync());
        }

        // GET: New/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @new = await _context.News
                .FirstOrDefaultAsync(m => m.NewId == id);
            if (@new == null)
            {
                return NotFound();
            }

            return View(@new);
        }
        [IsLogin]
        // GET: New/Create
        public IActionResult Create()
        {
            return View();
        }
        [IsLogin]
        // POST: New/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewId,HaberBaslik,HaberIcerik,ImageFile,Resim")] New @new)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _host.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(@new.ImageFile.FileName);
                string extension = Path.GetExtension(@new.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await @new.ImageFile.CopyToAsync(fileStream);
                }
                @new.Resim = fileName;
                @new.HaberEklenmeTarihi = DateTime.Now;
                _context.Add(@new);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@new);
        }
        [IsLogin]
        // GET: New/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @new = await _context.News.FindAsync(id);
            if (@new == null)
            {
                return NotFound();
            }
            return View(@new);
        }
        [IsLogin]
        // POST: New/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewId,HaberBaslik,HaberIcerik,ImageFile,Resim")] New @new)
        {
            //if (id != @new.NewId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    var entity = _context.News.Find(id);
                    if (entity!=null)
                    {
                        entity.HaberBaslik = @new.HaberBaslik;
                        entity.HaberIcerik = @new.HaberIcerik;
                        var imagepath = Path.Combine(_host.WebRootPath, "image", entity.Resim);
                        if (System.IO.File.Exists(imagepath))
                        {
                            System.IO.File.Delete(imagepath);
                        }
                        string wwwRootPath = _host.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(@new.ImageFile.FileName);
                        string extension = Path.GetExtension(@new.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/Image/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await @new.ImageFile.CopyToAsync(fileStream);
                        }
                        entity.ImageFile = @new.ImageFile;
                        entity.Resim = fileName;


                    }
                    
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewExists(@new.NewId))
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
            return View(@new);
        }
        [IsLogin]
        // GET: New/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @new = await _context.News
                .FirstOrDefaultAsync(m => m.NewId == id);
            if (@new == null)
            {
                return NotFound();
            }

            return View(@new);
        }
        [IsLogin]
        // POST: New/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @new = await _context.News.FindAsync(id);
            
            if (@new.Resim != null)
            {
                var imagepath = Path.Combine(_host.WebRootPath, "image", @new.Resim);
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
            }
            _context.News.Remove(@new);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewExists(int id)
        {
            return _context.News.Any(e => e.NewId == id);
        }
    }
}
