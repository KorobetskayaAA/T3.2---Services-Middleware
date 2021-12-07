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
    public class CatsController : ControllerBase
    {
        private readonly MurcatContext _context;

        public CatsController(MurcatContext context)
        {
            _context = context;
        }

        [HttpGet("{ownerId}")]
        public async Task<ActionResult<IEnumerable<Model.ViewCat>>> GetCats(int ownerId)
        {
            if (!OwnerExists(ownerId))
            {
                return NotFound();
            }
            var cats = await _context.Cats
                .Where(cat => cat.OwnerId == ownerId)
                .Include(cat => cat.Owner)
                .Include(cat => cat.Categories)
                .ThenInclude(cc => cc.Category)
                .ToListAsync();
            return Ok(cats.Select(cat => new Model.ViewCat(cat)));
        }

        [HttpGet("{ownerId}/{id}")]
        public async Task<ActionResult<Model.ViewCat>> GetCat(int ownerId, string id)
        {
            if (!OwnerExists(ownerId))
            {
                return NotFound();
            }

            var cat = await _context.Cats
                .Include(cat => cat.Owner)
                .Include(cat => cat.Categories)
                .ThenInclude(cc => cc.Category)
                .FirstOrDefaultAsync(cat => cat.Id == id);

            if (cat == null)
            {
                return NotFound();
            }

            if (cat.OwnerId != ownerId)
            {
                return BadRequest();
            }

            return new Model.ViewCat(cat);
        }

        [HttpPut("{ownerId}/{id}")]
        public async Task<IActionResult> PutCat(int ownerId, string id, Model.PostedCat cat)
        {
            if (!OwnerExists(ownerId))
            {
                return NotFound();
            }

            if (id != cat.Id || cat.OwnerId != ownerId)
            {
                return BadRequest();
            }

            var catToUpdate = _context.Cats.Find(cat.Id);
            _context.UpdateRange(_context.CatCategory
                .Where(cc => cc.CatId == cat.Id));
            cat.Update(catToUpdate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpPost("{ownerId}")]
        public async Task<ActionResult<Model.ViewCat>> PostCat(int ownerId, Model.PostedCat cat)
        {
            cat.OwnerId = ownerId;
            var createdCat = cat.Create();

            _context.Cats.Add(createdCat);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CatExists(cat.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            foreach (var catCategory in createdCat.Categories)
            {
                await _context.Entry(catCategory).Reference(cc => cc.Category).LoadAsync();
            }

            return CreatedAtAction("GetCat", 
                new { ownerId = cat.OwnerId, id = cat.Id },
                new Model.ViewCat(createdCat));
        }

        [HttpDelete("{ownerId}/{id}")]
        public async Task<ActionResult<Cat>> DeleteCat(int ownerId, string id)
        {
            var cat = await _context.Cats.FindAsync(id);
            if (cat == null)
            {
                return NotFound();
            }

            if (cat.OwnerId != ownerId)
            {
                return BadRequest();
            }

            _context.Cats.Remove(cat);
            await _context.SaveChangesAsync();

            return cat;
        }

        private bool OwnerExists(int id)
        {
            return _context.Owners.Any(e => e.Id == id);
        }

        private bool CatExists(string id)
        {
            return _context.Cats.Any(e => e.Id == id);
        }
    }
}
