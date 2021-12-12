using CatsWepApiWithDb.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatsWepApiWithDb.DAL.Repositories
{
    public class CatRepository : MurcatRepositoryBase
    {
        public CatRepository(MurcatContext context) : base(context) { }

        public async Task<IEnumerable<Cat>> GetAsync()
        {
            return await _context.Cats
                .Include(cat => cat.Owner)
                .Include(cat => cat.Categories)
                .ThenInclude(cc => cc.Category)
                .ToListAsync();
        }

        public async Task<Cat> GetAsync(string id)
        {
            return await _context.Cats.FindAsync(id);
        }

        public async Task LoadOwnerAsync(Cat cat)
        {
            await _context.Entry(cat).Reference(p => p.Owner).LoadAsync();
        }

        public async Task LoadCategoriesAsync(Cat cat)
        {
            await _context.Entry(cat).Collection(p => p.Categories).LoadAsync();
            foreach (var catCategory in cat.Categories)
            {
                await _context.Entry(catCategory).Reference(cc => cc.Category).LoadAsync();
            }
        }

        public async Task<Cat> GetWithOwnerCategoresAsync(string id)
        {
            return await _context.Cats
                .Include(cat => cat.Owner)
                .Include(cat => cat.Categories)
                .ThenInclude(cc => cc.Category)
                .FirstOrDefaultAsync(cat => cat.Id == id);
        }

        public async Task<IEnumerable<Cat>> GetOfOwner(int ownerId)
        {
            return await _context.Cats
                .Where(cat => cat.OwnerId == ownerId)
                .Include(cat => cat.Owner)
                .Include(cat => cat.Categories)
                .ThenInclude(cc => cc.Category)
                .ToListAsync();
        }

        public void Create(Cat cat)
        {
            _context.Cats.Add(cat);
        }

        public void Delete(Cat cat)
        {
            _context.Cats.Remove(cat);
        }

        public bool Exists(string id)
        {
            return _context.Cats.Any(e => e.Id == id);
        }
    }
}
