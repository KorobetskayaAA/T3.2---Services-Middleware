using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatsWepApiWithDb.DAL.Configuration
{
    class CatCategoryConfiguration : IEntityTypeConfiguration<CatCategory>
    {
        public void Configure(EntityTypeBuilder<CatCategory> builder)
        {
            builder.ToTable("Cat_Catgory");

            builder.HasKey("CatId", "CategoryId");
        }
    }
}
