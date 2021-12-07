using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CatsWepApiWithDb.DAL
{
    public class Cat
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float CutenessSum { get; set; } = 0;
        public int VotesCount { get; set; } = 0;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string Description { get; set; }

        public int OwnerId { get; set; }
        public Owner Owner { get; set; }

        public ICollection<CatCategory> Categories { get; set; }
    }
}
