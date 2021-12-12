using CatsWepApiWithDb.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatsWepApiWithDb.DAL.Repositories
{
    public class OwnerRepository : MurcatRepositoryBase
    {
        public OwnerRepository(MurcatContext context) : base(context) { }

        public async Task<Owner> GetAsync(int id)
        {
            return await _context.Owners.FindAsync(id);
        }

        public void Update(Owner owner)
        {
            _context.Update(owner);
        }

        public bool Exists(int id)
        {
            return _context.Owners.Any(e => e.Id == id);
        }
    }
}
