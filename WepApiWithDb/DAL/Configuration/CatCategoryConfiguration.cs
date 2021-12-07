using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatsWepApiWithDb.DAL.Configuration
{
    class CatCategoryConfiguration : IEntityTypeConfiguration<CatCategory>
    {
        public void Configure(EntityTypeBuilder<CatCategory> builder)
        {
            builder.ToTable("Cat_Category");

            builder.HasKey("CatId", "CategoryId");

            builder
                .Property(cc => cc.CategoryId)
                .HasColumnName("CategoriesId");

            builder
                .Property(cc => cc.CatId)
                .HasColumnName("CatsId");

            builder
                .HasOne(cc => cc.Category)
                .WithMany(c => c.Cats)
                .HasForeignKey(cc => cc.CategoryId);

            builder
                .HasOne(cc => cc.Cat)
                .WithMany(c => c.Categories)
                .HasForeignKey(cc => cc.CatId);
        }
    }
}
