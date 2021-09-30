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
    [IsLogin]
    public class SuperuserController : Controller
    {
        private readonly BlogContext _context;

        public SuperuserController(BlogContext context)
        {
            _context = context;
        }
        
        
        // GET: Superuser
        public async Task<IActionResult> Index()
        {
            return View(await _context.Superusers.ToListAsync());
        }

        // GET: Superuser/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superuser = await _context.Superusers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (superuser == null)
            {
                return NotFound();
            }

            return View(superuser);
        }

        // GET: Superuser/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Superuser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KullaniciAdi,Sifre")] Superuser superuser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(superuser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(superuser);
        }

        // GET: Superuser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superuser = await _context.Superusers.FindAsync(id);
            if (superuser == null)
            {
                return NotFound();
            }
            return View(superuser);
        }

        // POST: Superuser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KullaniciAdi,Sifre")] Superuser superuser)
        {
            if (id != superuser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(superuser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuperuserExists(superuser.Id))
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
            return View(superuser);
        }

        // GET: Superuser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superuser = await _context.Superusers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (superuser == null)
            {
                return NotFound();
            }

            return View(superuser);
        }

        // POST: Superuser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var superuser = await _context.Superusers.FindAsync(id);
            _context.Superusers.Remove(superuser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuperuserExists(int id)
        {
            return _context.Superusers.Any(e => e.Id == id);
        }
    }
}
