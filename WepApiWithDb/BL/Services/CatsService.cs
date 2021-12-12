using CatsWepApiWithDb.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatsWepApiWithDb.BL
{
    public class CatsService
    {
        private readonly MurcatContext _context;

        public CatsService(MurcatContext context)
        {
            _context = context;
        }

        public async Task<MurcatResult<IEnumerable<Model.ViewCat>>> GetCats(int ownerId)
        {
            if (!OwnerExists(ownerId))
            {
                return new MurcatResult<IEnumerable<Model.ViewCat>>(MurcatResultStatus.NotFound);
            }
            var cats = await _context.Cats
                .Where(cat => cat.OwnerId == ownerId)
                .Include(cat => cat.Owner)
                .Include(cat => cat.Categories)
                .ThenInclude(cc => cc.Category)
                .ToListAsync();

            var catViews = cats.Select(cat => new Model.ViewCat(cat));

            return new MurcatResult<IEnumerable<Model.ViewCat>>(catViews);
        }

        public async Task<MurcatResult<Model.ViewCat>> GetCat(int ownerId, string id)
        {
            if (!OwnerExists(ownerId))
            {
                return new MurcatResult<Model.ViewCat>(MurcatResultStatus.NotFound);
            }

            var cat = await _context.Cats
                .Include(cat => cat.Owner)
                .Include(cat => cat.Categories)
                .ThenInclude(cc => cc.Category)
                .FirstOrDefaultAsync(cat => cat.Id == id);

            if (cat == null)
            {
                return new MurcatResult<Model.ViewCat>(MurcatResultStatus.NotFound);
            }

            if (cat.OwnerId != ownerId)
            {
                return new MurcatResult<Model.ViewCat>(MurcatResultStatus.WrongInput);
            }

            return new MurcatResult<Model.ViewCat>(new Model.ViewCat(cat));
        }

        public async Task<MurcatResult> UpdateCat(int ownerId, string id, Model.PostedCat cat)
        {
            if (!OwnerExists(ownerId))
            {
                return new MurcatResult(MurcatResultStatus.NotFound);
            }

            if (id != cat.Id || cat.OwnerId != ownerId)
            {
                return new MurcatResult(MurcatResultStatus.WrongInput);
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
                    new MurcatResult(MurcatResultStatus.NotFound);
                }
                return new MurcatResult(MurcatResultStatus.DataSaveFailed);
            }

            return new MurcatResult(MurcatResultStatus.Ok);
        }

        public async Task<MurcatResult<Model.ViewCat>> CreateCat(int ownerId, Model.PostedCat cat)
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
                    new MurcatResult<Model.ViewCat>(MurcatResultStatus.AlreadyExists);
                }
                return new MurcatResult<Model.ViewCat>(MurcatResultStatus.DataSaveFailed);
            }

            foreach (var catCategory in createdCat.Categories)
            {
                await _context.Entry(catCategory).Reference(cc => cc.Category).LoadAsync();
            }

            return new MurcatResult<Model.ViewCat>(new Model.ViewCat(createdCat));
        }

        public async Task<MurcatResult<Model.ViewCat>> DeleteCat(int ownerId, string id)
        {
            var cat = await _context.Cats.FindAsync(id);
            if (cat == null)
            {
                return new MurcatResult<Model.ViewCat>(MurcatResultStatus.NotFound);
            }

            if (cat.OwnerId != ownerId)
            {
                return new MurcatResult<Model.ViewCat>(MurcatResultStatus.WrongInput);
            }

            _context.Cats.Remove(cat);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return new MurcatResult<Model.ViewCat>(MurcatResultStatus.DataSaveFailed);
            }

            return new MurcatResult<Model.ViewCat>(new Model.ViewCat(cat));
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
