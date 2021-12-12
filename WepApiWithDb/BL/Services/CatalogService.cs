using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatsWepApiWithDb.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CatsWepApiWithDb.BL
{
    public class CatalogService
    {
        private readonly CatRepository _repository;

        public CatalogService(CatRepository repository)
        {
            _repository = repository;
        }

        // Для демонстрационных целей данный сервис не использует MurcatResult
        // В реальном проекте сервисы должны быть единообразными!
        public async Task<IEnumerable<Model.ViewCat>> GetCats()
        {
            var cats = await _repository.GetAsync();
            return cats.Select(cat => new Model.ViewCat(cat));
        }

        public async Task<Model.ViewCat> GetCat(string id)
        {
            var cat = await _repository.GetAsync(id);

            if (cat == null)
            {
                return null;
            }

            await _repository.LoadOwnerAsync(cat);
            await _repository.LoadCategoriesAsync(cat);

            return new Model.ViewCat(cat);
        }

        public async Task<Model.ViewCat> Vote(string id, float cuteness)
        {
            var cat = await _repository.GetAsync(id);

            if (cat == null)
            {
                return null;
            }

            cat.CutenessSum += cuteness;
            cat.VotesCount++;

            await _repository.SaveAsync();

            return new Model.ViewCat(cat);
        }
    }
}
