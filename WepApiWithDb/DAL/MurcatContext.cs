using Microsoft.EntityFrameworkCore;
using CatsWepApiWithDb.DAL.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using CatsWepApiWithDb.DAL;

namespace CatsWepApiWithDb.DAL
{
    public class MurcatContext : DbContext
    {
        public MurcatContext() : base() { }

        public MurcatContext(DbContextOptions options) : base(options) { }

        public DbSet<Cat> Cats { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Owner> Owners { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CatConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CatCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new OwnerConfiguration());
        }


        public DbSet<CatsWepApiWithDb.DAL.CatCategory> CatCategory { get; set; }
    }
}
