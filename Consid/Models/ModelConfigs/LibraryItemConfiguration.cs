using System;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Consid.Models.ModelConfigs
{
    public class LibraryItemConfiguration : IEntityTypeConfiguration<LibraryItem>

    {
        public void Configure(EntityTypeBuilder<LibraryItem> modelBuilder)
        {
            modelBuilder.HasOne(l => l.Category)
                    .WithMany(l => l.LibraryItems)
                    .HasForeignKey(l => l.CategoryId)
                    .IsRequired();

            modelBuilder.Property(l => l.IsBorrowable)
                    .HasColumnType("bit");

            modelBuilder.Property(l => l.Type)
                    .IsRequired();

            modelBuilder.Property(l => l.BorrowDate);
                    
        }
    }
}
