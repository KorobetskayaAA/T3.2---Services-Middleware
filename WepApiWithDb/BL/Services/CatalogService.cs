using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatsWepApiWithDb.DAL;
using Microsoft.EntityFrameworkCore;

namespace CatsWepApiWithDb.BL
{
    public class CatalogService
    {
        private readonly MurcatContext _context;

        public CatalogService(MurcatContext context)
        {
            _context = context;
        }

        // Для демонстрационных целей данный сервис не использует MurcatResult
        // В реальном проекте сервисы должны быть единообразными!
        public async Task<IEnumerable<Model.ViewCat>> GetCats()
        {
            var cats = await _context.Cats
                .Include(cat => cat.Owner)
                .Include(cat => cat.Categories)
                .ThenInclude(cc => cc.Category)
                .ToListAsync();
            return cats.Select(cat => new Model.ViewCat(cat));
        }

        public async Task<Model.ViewCat> GetCat(string id)
        {
            var cat = await _context.Cats.FindAsync(id);

            if (cat == null)
            {
                return null;
            }

            await _context.Entry(cat).Reference(p => p.Owner).LoadAsync();
            await _context.Entry(cat).Collection(p => p.Categories).LoadAsync();
            foreach (var catCategory in cat.Categories)
            {
                await _context.Entry(catCategory).Reference(cc => cc.Category).LoadAsync();
            }

            return new Model.ViewCat(cat);
        }

        public async Task<Model.ViewCat> Vote(string id, float cuteness)
        {
            var cat = await _context.Cats.FindAsync(id);

            if (cat == null)
            {
                return null;
            }

            cat.CutenessSum += cuteness;
            cat.VotesCount++;

            await _context.SaveChangesAsync();

            return new Model.ViewCat(cat);
        }
    }
}
