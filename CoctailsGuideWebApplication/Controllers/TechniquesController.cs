using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using CoctailsGuideWebApplication;
using CoctailsGuideWebApplication.Models;

using Microsoft.AspNetCore.Http;
using System.IO;
using ClosedXML.Excel;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workCoctail = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workCoctail.Worksheets)
                            {
                                Techniques newtech;
                                var t = (from tech in _context.Techniques
                                         where tech.Name.Contains(worksheet.Name)
                                         select tech).ToList();
                                if (t.Count > 0)
                                {
                                    newtech = t[0];
                                }
                                else
                                {
                                    newtech = new Techniques();
                                    newtech.Name = worksheet.Name;
                                    newtech.Description = "from EXCEL";
                                    _context.Techniques.Add(newtech);
                                }
                                //
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Coctails coctail = new Coctails();
                                        var coc = (from coct in _context.Coctails
                                               where coct.Name.Contains(row.Cell(1).Value.ToString())
                                               select coct).ToList();
                                        if (coc.Count == 0)
                                        {
                                            coctail.Name = row.Cell(1).Value.ToString();
                                            coctail.YearofCreation = row.Cell(2).Value.ToString();
                                            coctail.CreationHistory = row.Cell(4).Value.ToString();
                                            coctail.Recipe = row.Cell(7).Value.ToString();
                                            coctail.Technique = newtech;

                                            if (row.Cell(3).Value.ToString().Length > 0)
                                            {
                                                var k1 = row.Cell(3).Value.ToString();
                                                Country country;
                                                var c = (from ct in _context.Country
                                                         where ct.Name.Contains(row.Cell(3).Value.ToString())
                                                         select ct).ToList();
                                                if (c.Count > 0)
                                                {
                                                    country = c[0];
                                                }
                                                else
                                                {
                                                    country = new Country();
                                                    country.Name = row.Cell(3).Value.ToString();
                                                    _context.Add(country);
                                                    await _context.SaveChangesAsync();//
                                                }
                                                coctail.CountryofCreation = country;
                                            }
                                            if (row.Cell(5).Value.ToString().Length > 0)
                                            {
                                                Strengths strengths;
                                                var k2 = row.Cell(5).Value.ToString();
                                                var s = (from str in _context.Strengths
                                                         where str.Name.Contains(row.Cell(5).Value.ToString())
                                                         select str).ToList();
                                                if (s.Count > 0)
                                                {
                                                    strengths = s[0];
                                                }
                                                else
                                                {
                                                    strengths = new Strengths();
                                                    strengths.Name = row.Cell(5).Value.ToString();
                                                    _context.Add(strengths);
                                                    await _context.SaveChangesAsync();//
                                                }
                                                coctail.Strength = strengths;
                                            }
                                            if (row.Cell(6).Value.ToString().Length > 0)
                                            {
                                                Glass glass;
                                                var k3 = row.Cell(6).Value.ToString();
                                                var g = (from gl in _context.Glass
                                                         where gl.Name.Contains(row.Cell(6).Value.ToString())
                                                         select gl).ToList();
                                                if (g.Count > 0)
                                                {
                                                    glass = g[0];
                                                }
                                                else
                                                {
                                                    glass = new Glass();
                                                    glass.Name = row.Cell(6).Value.ToString();
                                                    _context.Add(glass);
                                                    await _context.SaveChangesAsync();//
                                                }
                                                coctail.Glass = glass;
                                            }
                                            _context.Coctails.Add(coctail);
                                        }
                                        
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }

                            }
                            
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult ExportToExcel()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var techniques = _context.Techniques.Include("Coctails").ToList();

                foreach (var t in techniques)
                {
                    var worksheet = workbook.Worksheets.Add(t.Name);
                    worksheet.Cell("A1").Value = "Description";
                    worksheet.Cell("B1").Value = "Name";
                    worksheet.Cell("C1").Value = "Year of creation";
                    worksheet.Cell("D1").Value = "Country";
                    worksheet.Cell("E1").Value = "Creation History";
                    worksheet.Cell("F1").Value = "Strength";
                    worksheet.Cell("G1").Value = "Glass";
                    worksheet.Cell("H1").Value = "Recipe";
                    worksheet.Row(1).Style.Font.Bold = true;

                    var coctails = (from coc in t.Coctails
                              join country in _context.Country on coc.CountryofCreationId equals country.Id
                              join str in _context.Strengths on coc.StrengthId equals str.Id
                              join glass in _context.Glass on coc.GlassId equals glass.Id
                              select new
                              {
                                  NameofCoctail = coc.Name,
                                  Year = coc.YearofCreation,
                                  Country = country.Name,
                                  CreationHistoryCoc = coc.CreationHistory,
                                  Strength = str.Name,
                                  Glass = glass.Name,
                                  RecipeofCoctail = coc.Recipe
                              }).ToList();

                    worksheet.Cell(2, 1).Value = t.Description;
                    for (int i = 0; i < coctails.Count; i++)
                    {
                        worksheet.Cell(i + 2, 2).Value = coctails[i].NameofCoctail;
                        worksheet.Cell(i + 2, 3).Value = coctails[i].Year;
                        worksheet.Cell(i + 2, 4).Value = coctails[i].Country;
                        worksheet.Cell(i + 2, 5).Value = coctails[i].CreationHistoryCoc;
                        worksheet.Cell(i + 2, 6).Value = coctails[i].Strength;
                        worksheet.Cell(i + 2, 7).Value = coctails[i].Glass;
                        worksheet.Cell(i + 2, 8).Value = coctails[i].RecipeofCoctail;
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"CoctailDataBase_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        public ActionResult ExportToDocx()
        {

            var stream = new MemoryStream();

            using (WordprocessingDocument doc = WordprocessingDocument.Create(stream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
            {
                var techniques = _context.Techniques.Include("Coctails").ToList();

                MainDocumentPart mainPart = doc.AddMainDocumentPart();

                /*mainPart.Document = new Document();

                foreach (var t in techniques)
                {
                    var runTechniqueName = new Run();
                    runTechniqueName.Append(new Text("Technique: " + t.Name));

                    var paragraph = new Paragraph(runTechniqueName);
                    var body = new Body(paragraph);
                    mainPart.Document.Append(body);

                    var runProp = new RunProperties();
                    var runFont = new RunFonts { Ascii = "Times New Roman" };
                    var size = new FontSize { Val = new StringValue("48") };

                    runProp.Append(runFont);
                    runProp.Append(size);

                    runTechniqueName.PrependChild(runProp);

                    var runDescription = new Run();
                    runDescription.Append(new Text("Technique: " + t.Description));

                    var paragraphD = new Paragraph(runDescription);
                    var bodyD = new Body(paragraphD);
                    mainPart.Document.Append(bodyD);

                    var runPropD = new RunProperties();
                    var runFontD = new RunFonts { Ascii = "Times New Roman" };
                    var sizeD = new FontSize { Val = new StringValue("20") };

                    runPropD.Append(runFontD);
                    runPropD.Append(sizeD);

                    runDescription.PrependChild(runPropD);
                }
                */

                new Document(new Body()).Save(mainPart);

                Body body = mainPart.Document.Body;

                foreach (var t in techniques)
                {
                    body.Append(new Paragraph(
                        new Run(
                            new Text("Technique: " + t.Name))));
                    body.Append(new Paragraph(
                        //new ParagraphProperties(
                            //new RunFonts() { Ascii = "Times New Roman" },
                        new Run(
                            new Text("Technique description: " + t.Description))));
                    var coctails = (from coc in t.Coctails
                                    join country in _context.Country on coc.CountryofCreationId equals country.Id
                                    join str in _context.Strengths on coc.StrengthId equals str.Id
                                    join glass in _context.Glass on coc.GlassId equals glass.Id
                                    select new
                                    {
                                        NameofCoctail = coc.Name,
                                        Year = coc.YearofCreation,
                                        Country = country.Name,
                                        CreationHistoryCoc = coc.CreationHistory,
                                        Strength = str.Name,
                                        Glass = glass.Name,
                                        RecipeofCoctail = coc.Recipe
                                    }).ToList();
                    for (int i = 0; i < coctails.Count; i++)
                    {
                        body.Append(new Paragraph(
                            new Run(
                                new Text("Coctail: " + coctails[i].NameofCoctail))));
                        //list of year and td 6p
                        body.Append(new Paragraph(
                            new Run(
                                new Text("Year creation: " + coctails[i].Year))));
                        body.Append(new Paragraph(
                            new Run(
                                new Text("Country: " + coctails[i].Country))));
                        body.Append(new Paragraph(
                            new Run(
                                new Text("CreationHistory: " + coctails[i].CreationHistoryCoc))));
                        body.Append(new Paragraph(
                            new Run(
                                new Text("Strength: " + coctails[i].Strength))));
                        body.Append(new Paragraph(
                            new Run(
                                new Text("Glass: " + coctails[i].Glass))));
                        body.Append(new Paragraph(
                            new Run(
                                new Text("RecipeofCoctail: " + coctails[i].RecipeofCoctail))));
                    }
                }
                mainPart.Document.Save();
            }
            stream.Position = 0;
            return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = $"CoctailDataBase_{DateTime.UtcNow.ToShortDateString()}.docx"
            };
        }
    }
}
