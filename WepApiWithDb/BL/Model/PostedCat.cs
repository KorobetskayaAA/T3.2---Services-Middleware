using CatsWepApiWithDb.DAL;
using System.Collections.Generic;
using System.Linq;

namespace CatsWepApiWithDb.BL.Model
{
    public class PostedCat
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public int OwnerId { get; set; }

        public IEnumerable<int> CategoryIds { get; set; }

        public DAL.Cat Create()
        {
            var newCat = new DAL.Cat()
            {
                Id = Id,
                Name = Name,
                CutenessSum = 0,
                VotesCount = 0,
                Price = Price,
                Description = Description,
                OwnerId = OwnerId,
                Categories = CreateCategories()
            };
            return newCat;
        }


        public void Update(DAL.Cat catToUpdate)
        {
            catToUpdate.Name = Name;
            catToUpdate.Description = Description;
            if (Price != catToUpdate.Price)
            {
                catToUpdate.OldPrice = catToUpdate.Price;
                catToUpdate.Price = Price;
            }
            catToUpdate.Categories = CreateCategories();
        }

        private List<CatCategory> CreateCategories()
        {
            return CategoryIds
                .Select(categoryId => new DAL.CatCategory { CatId = Id, CategoryId = categoryId })
                .ToList();
        }
    }
}
