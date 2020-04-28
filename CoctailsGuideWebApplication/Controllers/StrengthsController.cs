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
    public class StrengthsController : Controller
    {
        private readonly DBCoctailsGuideContext _context;

        public StrengthsController(DBCoctailsGuideContext context)
        {
            _context = context;
        }

        // GET: Strengths
        public async Task<IActionResult> Index()
        {
            return View(await _context.Strengths.ToListAsync());
        }

        // GET: Strengths/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strengths = await _context.Strengths
                .FirstOrDefaultAsync(m => m.Id == id);
            if (strengths == null)
            {
                return NotFound();
            }

            return View(strengths);
        }

        // GET: Strengths/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Strengths/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Strengths strengths)
        {
            if (ModelState.IsValid)
            {
                _context.Add(strengths);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(strengths);
        }

        // GET: Strengths/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strengths = await _context.Strengths.FindAsync(id);
            if (strengths == null)
            {
                return NotFound();
            }
            return View(strengths);
        }

        // POST: Strengths/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Strengths strengths)
        {
            if (id != strengths.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(strengths);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StrengthsExists(strengths.Id))
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
            return View(strengths);
        }

        // GET: Strengths/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strengths = await _context.Strengths
                .FirstOrDefaultAsync(m => m.Id == id);
            if (strengths == null)
            {
                return NotFound();
            }

            return View(strengths);
        }

        // POST: Strengths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var strengths = await _context.Strengths.FindAsync(id);
            _context.Strengths.Remove(strengths);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StrengthsExists(int id)
        {
            return _context.Strengths.Any(e => e.Id == id);
        }
    }
}
