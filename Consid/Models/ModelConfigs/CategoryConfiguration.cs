using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Consid.Models.ModelConfigs
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> modelBuilder)
        {
            modelBuilder.Property(c => c.CategoryName)
                .IsRequired();

            modelBuilder.HasIndex(c => c.CategoryName)
                .IsUnique();

        }
    }
}
