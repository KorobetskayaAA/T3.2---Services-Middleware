using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatsWepApiWithDb.DAL
{
    public class CatCategory
    {
        public string CatId { get; set; }
        public Cat Cat { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
