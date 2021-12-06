using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CatsWepApiWithDb.DAL
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<CatCategory> Cats { get; set; }
    }
}
