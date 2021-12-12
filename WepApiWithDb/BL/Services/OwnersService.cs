using CatsWepApiWithDb.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CatsWepApiWithDb.BL
{
    public class OwnersService
    {
        private readonly MurcatContext _context;

        public OwnersService(MurcatContext context)
        {
            _context = context;
        }

        public async Task<MurcatResult<Model.Owner>> GetOwner(int id)
        {
            var owner = await _context.Owners.FindAsync(id);

            if (owner == null)
            {
                return new MurcatResult<Model.Owner>(MurcatResultStatus.NotFound);
            }

            return new MurcatResult<Model.Owner>(Model.Owner.Map(owner));
        }

        public async Task<MurcatResult> UpdateOwner(int id, Model.Owner owner)
        {
            if (id != owner.Id)
            {
                return new MurcatResult<Model.Owner>(MurcatResultStatus.WrongInput);
            }

            _context.Update(Model.Owner.Map(owner));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnerExists(id))
                {
                    return new MurcatResult<Model.Owner>(MurcatResultStatus.NotFound);
                }
                return new MurcatResult<Model.Owner>(MurcatResultStatus.DataSaveFailed);
            }

            return new MurcatResult<Model.Owner>(MurcatResultStatus.Ok);
        }

        private bool OwnerExists(int id)
        {
            return _context.Owners.Any(e => e.Id == id);
        }
    }
}
