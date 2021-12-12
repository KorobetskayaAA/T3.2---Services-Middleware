using CatsWepApiWithDb.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatsWepApiWithDb.BL
{
    public class CatsService
    {
        private readonly CatRepository _catRepository;
        private readonly OwnerRepository _ownerRepository;

        public CatsService(CatRepository catRepository, OwnerRepository ownerRepository)
        {
            _catRepository = catRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<MurcatResult<IEnumerable<Model.ViewCat>>> GetCats(int ownerId)
        {
            if (!_ownerRepository.Exists(ownerId))
            {
                return new MurcatResult<IEnumerable<Model.ViewCat>>(MurcatResultStatus.NotFound);
            }
            var cats = await _catRepository.GetAsync();

            var catViews = cats.Select(cat => new Model.ViewCat(cat));

            return new MurcatResult<IEnumerable<Model.ViewCat>>(catViews);
        }

        public async Task<MurcatResult<Model.ViewCat>> GetCat(int ownerId, string id)
        {
            if (!_ownerRepository.Exists(ownerId))
            {
                return new MurcatResult<Model.ViewCat>(MurcatResultStatus.NotFound);
            }

            var cat = await _catRepository.GetWithOwnerCategoresAsync(id);

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
            if (!_ownerRepository.Exists(ownerId))
            {
                return new MurcatResult(MurcatResultStatus.NotFound);
            }

            if (id != cat.Id || cat.OwnerId != ownerId)
            {
                return new MurcatResult(MurcatResultStatus.WrongInput);
            }

            if (!_catRepository.Exists(id))
            {
                return new MurcatResult(MurcatResultStatus.NotFound);
            }

            var catToUpdate = await _catRepository.GetAsync(cat.Id);
            await _catRepository.LoadCategoriesAsync(catToUpdate);
            cat.Update(catToUpdate);

            try
            {
                 await _catRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return new MurcatResult(MurcatResultStatus.DataSaveFailed);
            }

            return new MurcatResult(MurcatResultStatus.Ok);
        }

        public async Task<MurcatResult<Model.ViewCat>> CreateCat(int ownerId, Model.PostedCat cat)
        {
            cat.OwnerId = ownerId;
            var createdCat = cat.Create();

            _catRepository.Create(createdCat);

            try
            {
                await _catRepository.SaveAsync();
            }
            catch (DbUpdateException)
            {
                if (_catRepository.Exists(cat.Id))
                {
                    new MurcatResult<Model.ViewCat>(MurcatResultStatus.AlreadyExists);
                }
                return new MurcatResult<Model.ViewCat>(MurcatResultStatus.DataSaveFailed);
            }

            await _catRepository.LoadCategoriesAsync(createdCat);

            return new MurcatResult<Model.ViewCat>(new Model.ViewCat(createdCat));
        }

        public async Task<MurcatResult<Model.ViewCat>> DeleteCat(int ownerId, string id)
        {
            var cat = await _catRepository.GetAsync(id);
            if (cat == null)
            {
                return new MurcatResult<Model.ViewCat>(MurcatResultStatus.NotFound);
            }

            if (cat.OwnerId != ownerId)
            {
                return new MurcatResult<Model.ViewCat>(MurcatResultStatus.WrongInput);
            }

            _catRepository.Delete(cat);
            try
            {
                await _catRepository.SaveAsync();
            }
            catch (DbUpdateException)
            {
                return new MurcatResult<Model.ViewCat>(MurcatResultStatus.DataSaveFailed);
            }

            return new MurcatResult<Model.ViewCat>(new Model.ViewCat(cat));
        }
    }
}
