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
    public class CompoundsController : Controller
    {
        private readonly DBCoctailsGuideContext _context;

        public CompoundsController(DBCoctailsGuideContext context)
        {
            _context = context;
        }

        // GET: Compounds
        public async Task<IActionResult> Index()
        {
            var dBCoctailsGuideContext = _context.Compounds.Include(c => c.Coctail).Include(c => c.Ingredient);
            return View(await dBCoctailsGuideContext.ToListAsync());
        }

        // GET: Compounds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compounds = await _context.Compounds
                .Include(c => c.Coctail)
                .Include(c => c.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compounds == null)
            {
                return NotFound();
            }

            return View(compounds);
        }

        // GET: Compounds/Create
        public IActionResult Create()
        {
            ViewData["CoctailId"] = new SelectList(_context.Coctails, "Id", "Name");
            //ViewData["CoctailId"] = _context.Coctails.FirstOrDefaultAsync(m => m.Id == id);
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name");
            return View();
        }

        // POST: Compounds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IngredientId,Amount,CoctailId")] Compounds compounds)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compounds);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CoctailId"] = new SelectList(_context.Coctails, "Id", "Name", compounds.CoctailId);
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name", compounds.IngredientId);
            return View(compounds);
        }

        // GET: Compounds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compounds = await _context.Compounds.FindAsync(id);
            if (compounds == null)
            {
                return NotFound();
            }
            ViewData["CoctailId"] = new SelectList(_context.Coctails, "Id", "Name", compounds.CoctailId);
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name", compounds.IngredientId);
            return View(compounds);
        }

        // POST: Compounds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IngredientId,Amount,CoctailId")] Compounds compounds)
        {
            if (id != compounds.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compounds);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompoundsExists(compounds.Id))
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
            ViewData["CoctailId"] = new SelectList(_context.Coctails, "Id", "Name", compounds.CoctailId);
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name", compounds.IngredientId);
            return View(compounds);
        }

        // GET: Compounds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compounds = await _context.Compounds
                .Include(c => c.Coctail)
                .Include(c => c.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compounds == null)
            {
                return NotFound();
            }

            return View(compounds);
        }

        // POST: Compounds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compounds = await _context.Compounds.FindAsync(id);
            _context.Compounds.Remove(compounds);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompoundsExists(int id)
        {
            return _context.Compounds.Any(e => e.Id == id);
        }
    }
}
