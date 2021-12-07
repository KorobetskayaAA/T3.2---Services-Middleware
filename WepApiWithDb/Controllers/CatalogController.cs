using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatsWepApiWithDb.DAL;

namespace CatsWepApiWithDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly MurcatContext _context;

        public CatalogController(MurcatContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cat>>> GetCats()
        {
            return await _context.Cats.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Model.ViewCat>> GetCat(string id)
        {
            var cat = await _context.Cats.FindAsync(id);

            if (cat == null)
            {
                return NotFound();
            }

            await _context.Entry(cat).Reference(p => p.Owner).LoadAsync();
            await _context.Entry(cat).Collection(p => p.Categories).LoadAsync();
            foreach (var catCategory in cat.Categories)
            {
                await _context.Entry(catCategory).Reference(cc => cc.Category).LoadAsync();
            }

            return new Model.ViewCat(cat);
        }

        [HttpPost("{id}/vote/{cuteness}")]
        public async Task<ActionResult<float>> PostVote(string id, float cuteness)
        {
            var cat = await _context.Cats.FindAsync(id);

            if (cat == null)
            {
                return NotFound();
            }

            cat.CutenessSum += cuteness;
            cat.VotesCount++;

            await _context.SaveChangesAsync();

            return Ok(cat.CutenessSum / cat.VotesCount);
        }
    }
}
