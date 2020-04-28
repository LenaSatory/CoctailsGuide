using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using CoctailsGuideWebApplication;
using CoctailsGuideWebApplication.Models;

using Microsoft.AspNetCore.Authorization;

namespace CoctailsGuideWebApplication.Controllers
{
    [Authorize(Roles ="admin, user")]
    public class CoctailsController : Controller
    {
        private readonly DBCoctailsGuideContext _context;

        public CoctailsController(DBCoctailsGuideContext context)       
        {
            _context = context;
        }

        // GET: Coctails
        public async Task<IActionResult> Index()
        {
            var dBCoctailsGuideContext = _context.Coctails.Include(c => c.CountryofCreation).Include(c => c.Glass).Include(c => c.Strength).Include(c => c.Technique);
            return View(await dBCoctailsGuideContext.ToListAsync());
        }
        public async Task<IActionResult> HashtagIndexStrengths(int? id, string? name)
        {
            if (id == null)
                return RedirectToAction("Coctails", "Index");
            //find coctails for strenghts REWRITECOMENT!!!
            ViewBag.StrengthsID = id;
            ViewBag.StrengthsName = name;
            var coctailsByStrenghts = _context.Coctails.Where(b => b.Strength.Id == id).Include(b => b.Strength);

            return View(await coctailsByStrenghts.ToListAsync());
        }
        public async Task<IActionResult> HashtagIndexTechniques(int? id, string? name)
        {
            if (id == null)
                return RedirectToAction("Coctails", "Index");
            //find coctails for strenghts REWRITECOMENT!!!
            ViewBag.TechniqueId = id;
            ViewBag.TechniqueName = name;
            ViewBag.TechniqueDescription = (_context.Coctails.Include(b => b.Technique.Description).Where(b => b.Technique.Id == id)).ToString();
            //ViewBag.TechniqueDescription = _context.Coctails.Where(b => b.Technique.Id == id).Include(b => b.Technique.Description);//Include(b => b.Technique.Description);
            var coctailsByTechnique = _context.Coctails.Where(b => b.Technique.Id == id).Include(b => b.Technique);

            return View(await coctailsByTechnique.ToListAsync());
        }
        public async Task<IActionResult> HashtagIndexGlass(int? id, string? name)
        {
            if (id == null)
                return RedirectToAction("Coctails", "Index");
            //find coctails for strenghts REWRITECOMENT!!!
            ViewBag.GlassId = id;
            ViewBag.GlassName = name;
            var coctailsByTechnique = _context.Coctails.Where(b => b.Glass.Id == id).Include(b => b.Glass);

            return View(await coctailsByTechnique.ToListAsync());
        }

        // GET: Coctails/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            //var compund = _context.Compounds.Include(k => k.Ingredient).FirstOrDefault(p => p.Id == id);
            var coctails = await _context.Coctails
                .Include(c => c.CountryofCreation)
                .Include(c => c.Glass)
                .Include(c => c.Strength)
                .Include(c => c.Technique)
                .Include(c => c.Compounds)
                .ThenInclude(i => i.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coctails == null)
            {
                return NotFound();
            }
            return View(coctails);
        }
        public async Task<IActionResult> HeshStrDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coctails = await _context.Coctails
                .Include(c => c.CountryofCreation)
                .Include(c => c.Glass)
                .Include(c => c.Strength)
                .Include(c => c.Technique)
                .Include(c => c.Compounds)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (coctails == null)
            {
                return NotFound();
            }
            return RedirectToAction("HashtagIndexStrengths", "Coctails", new { id = coctails.StrengthId, name = coctails.Strength.Name });
        }
        public async Task<IActionResult> HeshTehDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coctails = await _context.Coctails
                .Include(c => c.CountryofCreation)
                .Include(c => c.Glass)
                .Include(c => c.Strength)
                .Include(c => c.Technique)
                .Include(c => c.Compounds)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (coctails == null)
            {
                return NotFound();
            }
            return RedirectToAction("HashtagIndexTechniques", "Coctails", new { id = coctails.TechniqueId, name = coctails.Technique.Name});
        }
        public async Task<IActionResult> HeshGlassDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coctails = await _context.Coctails
                .Include(c => c.CountryofCreation)
                .Include(c => c.Glass)
                .Include(c => c.Strength)
                .Include(c => c.Technique)
                .Include(c => c.Compounds)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (coctails == null)
            {
                return NotFound();
            }
            return RedirectToAction("HashtagIndexGlass", "Coctails", new { id = coctails.Glass.Id, name = coctails.Glass.Name });
        }

        // GET: Coctails/Create
        public IActionResult Create()
        {
            ViewData["CountryofCreationId"] = new SelectList(_context.Country, "Id", "Name");
            ViewData["GlassId"] = new SelectList(_context.Glass, "Id", "Name");
            ViewData["StrengthId"] = new SelectList(_context.Strengths, "Id", "Name");
            ViewData["TechniqueId"] = new SelectList(_context.Techniques, "Id", "Name");
            return View();
        }

        // POST: Coctails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,YearofCreation,CountryofCreationId,CreationHistory,StrengthId,TechniqueId,GlassId,Recipe")] Coctails coctails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coctails);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Compounds");
            }
            ViewData["CountryofCreationId"] = new SelectList(_context.Country, "Id", "Name", coctails.CountryofCreationId);
            ViewData["GlassId"] = new SelectList(_context.Glass, "Id", "Name", coctails.GlassId);
            ViewData["StrengthId"] = new SelectList(_context.Strengths, "Id", "Name", coctails.StrengthId);
            ViewData["TechniqueId"] = new SelectList(_context.Techniques, "Id", "Name", coctails.TechniqueId);
            return View(coctails);
        }

        // GET: Coctails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coctails = await _context.Coctails.FindAsync(id);
            if (coctails == null)
            {
                return NotFound();
            }
            ViewData["CountryofCreationId"] = new SelectList(_context.Country, "Id", "Name", coctails.CountryofCreationId);
            ViewData["GlassId"] = new SelectList(_context.Glass, "Id", "Name", coctails.GlassId);
            ViewData["StrengthId"] = new SelectList(_context.Strengths, "Id", "Name", coctails.StrengthId);
            ViewData["TechniqueId"] = new SelectList(_context.Techniques, "Id", "Name", coctails.TechniqueId);
            return View(coctails);
        }

        // POST: Coctails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,YearofCreation,CountryofCreationId,CreationHistory,StrengthId,TechniqueId,GlassId,Recipe")] Coctails coctails)
        {
            if (id != coctails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coctails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoctailsExists(coctails.Id))
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
            ViewData["CountryofCreationId"] = new SelectList(_context.Country, "Id", "Name", coctails.CountryofCreationId);
            ViewData["GlassId"] = new SelectList(_context.Glass, "Id", "Name", coctails.GlassId);
            ViewData["StrengthId"] = new SelectList(_context.Strengths, "Id", "Name", coctails.StrengthId);
            ViewData["TechniqueId"] = new SelectList(_context.Techniques, "Id", "Name", coctails.TechniqueId);
            return View(coctails);
        }

        // GET: Coctails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coctails = await _context.Coctails
                .Include(c => c.CountryofCreation)
                .Include(c => c.Glass)
                .Include(c => c.Strength)
                .Include(c => c.Technique)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coctails == null)
            {
                return NotFound();
            }

            return View(coctails);
        }

        // POST: Coctails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coctails = await _context.Coctails.FindAsync(id);
            _context.Coctails.Remove(coctails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoctailsExists(int id)
        {
            return _context.Coctails.Any(e => e.Id == id);
        }
    }
}
