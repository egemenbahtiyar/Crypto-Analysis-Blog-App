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
using Microsoft.AspNetCore.Http;

namespace BlogApp.Controllers
{
    
    public class BlogController : Controller
    {
        private readonly BlogContext _context;
        private readonly IWebHostEnvironment _host;

        public BlogController(BlogContext context, IWebHostEnvironment host)
        {
            _context = context;
            this._host = host;
        }

        [IsLogin]
        // GET: Blog
        public async Task<IActionResult> Index()
        {
            var blogContext = _context.Blogs.Include(b => b.Category);

            return View(await blogContext.OrderByDescending(blog => blog.EklenmeTarihi).ToListAsync());


        }

        public IActionResult List(int? id, string q)
        {
            var bloglar = _context.Blogs
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
                    CategoryId = i.CategoryId


                }).AsQueryable();
            if (string.IsNullOrEmpty(q)==false)
            {
                bloglar = bloglar.Where(i => i.Baslik.Contains(q) || i.Aciklama.Contains(q));
            }
            if (id != null)
            {
                bloglar = bloglar.Where(i => i.CategoryId == id);
            }
            return View(bloglar.OrderByDescending(bloglar => bloglar.EklenmeTarihi).ToList());
                

        }

        
        // GET: Blog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        [IsLogin]
        // GET: Blog/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "KategoriAdi");
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [IsLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Baslik,Aciklama,ImageFile,Icerik,Onay,Anasayfa,CategoryId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                // save image to wwwroot/image
                string wwwRootPath = _host.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(blog.ImageFile.FileName);
                string extension = Path.GetExtension(blog.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);

                using (var fileStream = new FileStream(path,FileMode.Create))
                {
                    await blog.ImageFile.CopyToAsync(fileStream);
                }
                

                blog.Resim = fileName;
                blog.EklenmeTarihi = DateTime.Now;
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "KategoriAdi", blog.CategoryId);
            return View(blog);
        }

        [IsLogin]
        [HttpPost]
        public IActionResult UploadCKEditor(IFormFile upload)
        {
            
            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
            var path = Path.Combine(_host.WebRootPath + "/uploads/", filename);
            var stream = new FileStream(path, FileMode.Create);
            upload.CopyToAsync(stream);
            return new JsonResult(new
            {
                uploaded = 1,
                fileName = upload.FileName,
                url = "/uploads/" + filename
            });
            
        }

        [IsLogin]
        // GET: Blog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "KategoriAdi", blog.CategoryId);
            return View(blog);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [IsLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Baslik,Aciklama,Resim,ImageFile,Icerik,Onay,Anasayfa,CategoryId")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var entity = _context.Blogs.Find(id);
                    if (entity != null)
                    {
                        entity.Baslik = blog.Baslik;
                        entity.Aciklama = blog.Aciklama;
                        entity.Icerik = blog.Icerik;
                        entity.Onay = blog.Onay;
                        entity.Anasayfa = blog.Anasayfa;
                        entity.CategoryId = blog.CategoryId;
                        
                    }
                    var imagepath = Path.Combine(_host.WebRootPath, "image", entity.Resim);
                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }

                    string wwwRootPath = _host.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(blog.ImageFile.FileName);
                    string extension = Path.GetExtension(blog.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await blog.ImageFile.CopyToAsync(fileStream);
                    }


                    entity.ImageFile = blog.ImageFile;
                    entity.Resim = fileName;

                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "KategoriAdi", blog.CategoryId);
            return View(blog);
        }
        [IsLogin]
        // GET: Blog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blog/Delete/5
        [IsLogin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var blog = await _context.Blogs.FindAsync(id);
            if (blog.Resim!=null)
            {
                var imagepath = Path.Combine(_host.WebRootPath, "image", blog.Resim);
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
            }
            
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
