using EngineeringAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure.Data.Configurations {
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier> {
        public void Configure(EntityTypeBuilder<Supplier> builder) {
            builder.ToTable("Suppliers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SupplierCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.SupplierCode)
                .IsUnique();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Country)
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasMany(x => x.Equipment)
                .WithOne(x => x.Supplier)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
