using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoctailsGuideWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly DBCoctailsGuideContext _context;

        public ChartsController(DBCoctailsGuideContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]

        public JsonResult JsonData()
        {
            var ingredients = _context.Country.Include(b => b.Coctails).ToList();
            List<object> ingCoc = new List<object>();
            ingCoc.Add(new[] { "Інгредієнт", "Кількість коктейлів" });

            foreach (var i in ingredients)
            {
                ingCoc.Add(new object[] { i.Name, i.Coctails.Count() });
            }

            return new JsonResult(ingCoc);
        }
    }
}