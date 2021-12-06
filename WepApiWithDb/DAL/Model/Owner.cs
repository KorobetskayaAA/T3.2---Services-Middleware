using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CatsWepApiWithDb.DAL
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contacts { get; set; }
        public int Rating { get; set; }

        public ICollection<Cat> Cats { get; set; } = new HashSet<Cat>();
    }
}
