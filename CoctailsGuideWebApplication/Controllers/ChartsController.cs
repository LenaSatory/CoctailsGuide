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

        [HttpGet("JsonData1")]

        public JsonResult JsonData1()
        {
            var techniue = _context.Techniques.Include(b => b.Coctails).ToList();
            List<object> techCoc = new List<object>();
            techCoc.Add(new[] { "Technique", "Number of coctails" });

            foreach (var i in techniue)
            {
                techCoc.Add(new object[] { i.Name, i.Coctails.Count() });
            }

            return new JsonResult(techCoc);
        }

        [HttpGet("JsonData2")]

        public JsonResult JsonData2()
        {
            var country = _context.Country.Include(b => b.Coctails).ToList();
            List<object> countryCoc = new List<object>();
            countryCoc.Add(new[] { "Country", "Number of cocktails invented in the country" });

            foreach (var i in country)
            {
                countryCoc.Add(new object[] { i.Name, i.Coctails.Count() });
            }

            return new JsonResult(countryCoc);
        }

        [HttpGet("JsonData3")]

        public JsonResult JsonData3()
        {
            var strengths = _context.Strengths.Include(b => b.Coctails).ToList();
            List<object> strCoc = new List<object>();
            strCoc.Add(new[] { "Strengths", "Number of coctails" });

            foreach (var i in strengths)
            {
                strCoc.Add(new object[] { i.Name, i.Coctails.Count() });
            }

            return new JsonResult(strCoc);
        }


    }
}