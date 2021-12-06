using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatsWepApiWithDb.DAL.Configuration
{
    class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.ToTable("Owner");

            builder.Property(o => o.Id)
                .ValueGeneratedNever();

            builder.Property(o => o.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(o => o.Contacts)
                .HasMaxLength(200);
        }
    }
}
