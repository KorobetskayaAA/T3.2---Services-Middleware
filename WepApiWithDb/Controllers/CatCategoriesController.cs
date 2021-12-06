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
    public class CatCategoriesController : ControllerBase
    {
        private readonly MurcatContext _context;

        public CatCategoriesController(MurcatContext context)
        {
            _context = context;
        }

        // GET: api/CatCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatCategory>>> GetCatCategory()
        {
            return await _context.CatCategory.ToListAsync();
        }

        // GET: api/CatCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CatCategory>> GetCatCategory(string id)
        {
            var catCategory = await _context.CatCategory.FindAsync(id);

            if (catCategory == null)
            {
                return NotFound();
            }

            return catCategory;
        }

        // PUT: api/CatCategories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCatCategory(string id, CatCategory catCategory)
        {
            if (id != catCategory.CatId)
            {
                return BadRequest();
            }

            _context.Entry(catCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CatCategories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CatCategory>> PostCatCategory(CatCategory catCategory)
        {
            _context.CatCategory.Add(catCategory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CatCategoryExists(catCategory.CatId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCatCategory", new { id = catCategory.CatId }, catCategory);
        }

        // DELETE: api/CatCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CatCategory>> DeleteCatCategory(string id)
        {
            var catCategory = await _context.CatCategory.FindAsync(id);
            if (catCategory == null)
            {
                return NotFound();
            }

            _context.CatCategory.Remove(catCategory);
            await _context.SaveChangesAsync();

            return catCategory;
        }

        private bool CatCategoryExists(string id)
        {
            return _context.CatCategory.Any(e => e.CatId == id);
        }
    }
}
