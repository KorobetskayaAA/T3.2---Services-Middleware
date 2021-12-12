using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatsWepApiWithDb.DAL.Repositories
{
    public class MurcatRepositoryBase
    {
        protected readonly MurcatContext _context;

        public MurcatRepositoryBase(MurcatContext context)
        {
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
