using System.Collections.Generic;
using System.Linq;

namespace CatsWepApiWithDb.BL.Model
{
    public class ViewCat
    {
        public class ViewOwner
        {
            public string Name { get; set; }
            public string Contacts { get; set; }
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public float? Cuteness { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string Description { get; set; }

        public ViewOwner Owner { get; set; }

        public IEnumerable<string> Categories { get; set; }
        
        public ViewCat(DAL.Cat cat)
        {
            Id = cat.Id;
            Name = cat.Name;
            Price = cat.Price;
            OldPrice = cat.OldPrice;
            Description = cat.Description;

            if (cat.VotesCount > 0)
            {
                Cuteness = cat.CutenessSum / cat.VotesCount;
            }

            Owner = cat.Owner != null
                    ? new ViewOwner {
                        Name = cat.Owner.Name,
                        Contacts = cat.Owner.Contacts
                    }
                    : null;

            Categories = cat.Categories?.Select(category => category.Category?.Name);
        }
    }
}
