using CatsWepApiWithDb.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CatsWepApiWithDb.BL
{
    public class OwnersService
    {
        private readonly OwnerRepository _repository;

        public OwnersService(OwnerRepository repository)
        {
            _repository = repository;
        }

        public async Task<MurcatResult<Model.Owner>> GetOwner(int id)
        {
            var owner = await _repository.GetAsync(id);

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

            _repository.Update(Model.Owner.Map(owner));

            try
            {
                await _repository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.Exists(id))
                {
                    return new MurcatResult<Model.Owner>(MurcatResultStatus.NotFound);
                }
                return new MurcatResult<Model.Owner>(MurcatResultStatus.DataSaveFailed);
            }

            return new MurcatResult<Model.Owner>(MurcatResultStatus.Ok);
        }
    }
}
