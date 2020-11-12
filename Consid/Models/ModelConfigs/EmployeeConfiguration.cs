using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Consid.Models.ModelConfigs
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> modelBuilder)
        {
            modelBuilder.Property(e => e.IsCEO)
               .HasColumnType("bit");

            modelBuilder.Property(e => e.IsManager)
                .HasColumnType("bit");

            modelBuilder.HasOne(e => e.Manager)
                .WithMany(e => e.Employees)
                .HasForeignKey(e => e.ManagerId);
        }
    }
}
