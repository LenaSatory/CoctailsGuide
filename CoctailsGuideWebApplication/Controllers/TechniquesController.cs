using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoctailsGuideWebApplication;

namespace CoctailsGuideWebApplication.Controllers
{
    public class TechniquesController : Controller
    {
        private readonly DBCoctailsGuideContext _context;

        public TechniquesController(DBCoctailsGuideContext context)
        {
            _context = context;
        }

        // GET: Techniques
        public async Task<IActionResult> Index()
        {
            return View(await _context.Techniques.ToListAsync());
        }

        // GET: Techniques/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techniques = await _context.Techniques
                .FirstOrDefaultAsync(m => m.Id == id);
            if (techniques == null)
            {
                return NotFound();
            }

            return View(techniques);
        }

        // GET: Techniques/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Techniques/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Techniques techniques)
        {
            if (ModelState.IsValid)
            {
                _context.Add(techniques);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(techniques);
        }

        // GET: Techniques/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techniques = await _context.Techniques.FindAsync(id);
            if (techniques == null)
            {
                return NotFound();
            }
            return View(techniques);
        }

        // POST: Techniques/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Techniques techniques)
        {
            if (id != techniques.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(techniques);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TechniquesExists(techniques.Id))
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
            return View(techniques);
        }

        // GET: Techniques/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techniques = await _context.Techniques
                .FirstOrDefaultAsync(m => m.Id == id);
            if (techniques == null)
            {
                return NotFound();
            }

            return View(techniques);
        }

        // POST: Techniques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var techniques = await _context.Techniques.FindAsync(id);
            _context.Techniques.Remove(techniques);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TechniquesExists(int id)
        {
            return _context.Techniques.Any(e => e.Id == id);
        }
    }
}
